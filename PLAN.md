# Improvement Plan

Findings from a code review of `src/HttpClientToCurl`. Line numbers refer to the code at commit `9da234f`.

## Issues found

### P0 — Security / reliability

1. **Redaction ignored in automatic mode.**
   `RedactedHeaders` is defined on `Config/BaseConfig.cs:8`. The per-sink lambdas in `HttpMessageHandlers/CurlGeneratorHttpMessageHandler.cs:22-55` copy every other option but not `RedactedHeaders`. Authorization and API-key headers reach logs, console and files unredacted. The only redaction tests (`tests/HttpClientToCurlTest/FunctionalTest/SuccessScenariosTests.cs:487-548`) do not go through the handler.

2. **Curl generation can fail the real HTTP request.**
   `CurlGeneratorHttpMessageHandler.SendAsync` has no try/catch.
   - HEAD/OPTIONS/other methods throw `NotSupportedException` (`Builder/Generator.cs:29`), so the request fails.
   - A file-append collision in `Utility/Helpers.cs` `WriteInFile` under concurrent requests could fail it too (not reproduced).

3. **Quotes not escaped (broken output, shell injection when pasted).**
   Body (`Extensions.cs` `AppendBodyItem`), headers and URL are wrapped in `'...'` without escaping `'`. A body such as `{"name":"O'Brien"}` produces a broken curl, and crafted input can run extra shell commands.

### P1 — Correctness

4. **`ApplyEncodeUri` (`Builder/Concrete/Common/Extensions.cs` ~88-120):**
   - single-parameter queries are never encoded;
   - parameters without `=` are dropped;
   - values containing `=` (e.g. base64) are truncated;
   - already percent-encoded values are encoded again.

5. **Multi-value headers joined with `"; "`** (`Extensions.cs:134, 153, 171, 204, 222`). Should be `", "`; `;` has a different meaning (e.g. `Accept`).

6. **Sync-over-async body read** in `AddBody` (`ReadAsStringAsync().GetAwaiter().GetResult()`), executed inside a handler on every request. With multiple sinks the body is read and the curl rebuilt once per sink (up to 3x).

7. **Multipart bodies are wrong.** Emitted as a raw `-d '<body with boundary>'`, not a working curl. Binary content has no `--data-binary` support.

### P2 — Maintainability

8. **Duplication.** The redaction ternary is copied five times (`Extensions.cs:132, 151, 169, 202, 220`) and the two `AddHeaders` overloads are near-copies. The five builders (`HttpGet/Post/Put/Patch/DeleteBuilder`) differ only by whether they add a body. This duplication is how issue 1 slipped in.

9. **Typo** in `Directory.Build.props`: `netstansard2.1` (masked by the csproj override).

10. **Test suite overlaps.** `HttpClientToCurlTest` and `HttpRequestMessageToCurlTest` largely duplicate each other; there are no regression tests for the issues above.

11. **`NeedAddDefaultHeaders` does nothing in automatic mode.** `HttpClient` merges its default headers into `request.Headers` before the handler runs, and the handler uses the `HttpRequestMessage` overload, which never reads that flag. Setting it to `false` strips nothing. Decide whether to document this or implement it (e.g. by tracking default headers separately).

12. **Form-urlencoded bodies were decoded before being split on `&`** (found during Phase 2 review). `q=a%26b&x=1` produced three `-d` fields. Fixed in Phase 2: the body is split first; values containing `&`, `+` or `%` use `--data-urlencode`.

### Corrections to the earlier assessment
Form-urlencoded bodies and `--compressed` are already supported. Missing HTTP methods is a crash in automatic mode (issue 2), not just a feature gap.

## Plan

Each step ships with its own regression test, written first and confirmed to fail.

### Phase 1 — Stop the bleeding (P0)
1. Copy `RedactedHeaders` through in all three handler lambdas. Prefer a single `CopyTo(BaseConfig)` helper so new options cannot be forgotten.
   Test: handler with `RedactedHeaders = ["Authorization"]` for console, file and logger sinks.
2. Wrap curl generation in the handler in try/catch. Log at warning level through `ILogger` when available, and always call `base.SendAsync`.
   Test: HEAD and OPTIONS through the handler still send the request.
3. Escape `'` as `'\''` for body, header and URL values in one shared helper.
   Test: apostrophe in body, header value and query string.

### Phase 2 — Correctness (P1) — DONE (limitations below)
4. Rewrite `ApplyEncodeUri` to encode each query parameter once, keeping valueless params, splitting on the first `=` only, and not re-encoding existing `%XX` sequences.
5. Join multi-value headers with `", "`.
6. Read the body once per request, reuse the string across sinks, and avoid blocking where the target allows it.
7. Multipart: emit `-F` parts. Binary: emit `--data-binary`. Check non-seekable stream behaviour with a test before changing anything.

### Phase 3 — Refactor (P2) — DONE (see status below)
8. Replace the five builders with a single generic builder. Add HEAD and OPTIONS. Unknown methods fall back to `-X <METHOD>` instead of throwing.
9. Extract one `FormatHeader(name, values, redacted)` and merge the two `AddHeaders` overloads.
10. Fix the `Directory.Build.props` typo.
    Also resolve issue 11 (`NeedAddDefaultHeaders` in automatic mode).
11. Merge the duplicated test cases into shared case sets run against both entry points.

### Phase 4 — Features
12. PowerShell and cmd quoting targets.
13. Safe-by-default redaction (`Authorization`, `Cookie`, `Proxy-Authorization`, `X-Api-Key`), plus optional query-string and body-field redaction.
14. Extra curl options: `--insecure`, `--proxy`, `--max-time`, cookies, `--http2`.
15. Extra sinks: generic callback, URL/host filters.

### Phase 5 — Hygiene and AI-friendliness
16. Code coverage in CI, CHANGELOG, Dependabot, SourceLink, `<Nullable>enable</Nullable>`.
17. XML doc examples on public APIs, `llms.txt`, README refresh, and keeping `CLAUDE.md` current.
18. Optional and last: a separate `HttpClientToCurl.Mcp` package (tools such as curl-from-request-JSON and redact-curl). Only if there is a concrete use case.

### Suggested order
Phase 1 (one PR per issue) → Phase 2 → Phase 3 → the rest as demand dictates. Branch names must follow `feature|bugfix|hotfix|refactor|optimize|document/HCG-<n>`.

### Phase 2 status and known limitations
- Done: issues 4, 5, 6, 7 and 12, each with regression tests.
- Issue 6 is only fixed for the handler (the body is buffered asynchronously first, and binary or multipart-file bodies are no longer read). The manual extension methods still call `GetAwaiter().GetResult()`, and the curl is still built once per sink.
- Multipart text parts use `--form-string`, which cannot carry `;type=`, so a part such as a JSON metadata field loses its content type. Verify the `-F 'name="value";type=...'` syntax against a real curl before changing it.
- Binary bodies and multipart file parts render as placeholders (`@payload.bin`, `@<filename>`); the command needs real local files to run.
- Output changes callers may notice: multi-value headers use `, ` (`; ` for Cookie, a space for User-Agent), single-parameter queries are encoded, a body starting with `@` uses `--data-raw`, and form bodies may use `--data-urlencode`. This justifies a minor version bump and a CHANGELOG entry.

### Phase 3 status and notes
- Done: issue 8 (one `RequestBuilder` base; GET/POST/PUT/PATCH/DELETE builders are now one-line subclasses and an internal `FallbackBuilder` handles HEAD, OPTIONS, TRACE and custom verbs), issue 9 (single `AddHeaders`), issue 10 (typo), issue 11 (documented on `BaseConfig.NeedAddDefaultHeaders` and pinned by a test; not implemented).
- Behaviour/API changes: `Generator.GenerateCurl` no longer throws `NotSupportedException` for other methods; HEAD emits `--head` (and never a body); non-letter method names are shell-quoted; `RequestBuilder` is a new public base type of the five public builders.
- Test dedup is partial: the `MediaTypes` unit tests and the escaping / multi-value-header / method tests now live in `tests/Shared` and run against both entry points. The two `FunctionalTest/SuccessScenariosTests.cs` files (556 vs 262 lines) are still duplicated.
- DELETE still never emits a body. Some APIs send JSON with DELETE; with the shared builder this is now a one-word change (`BodyMode.WhenContentPresent`).

