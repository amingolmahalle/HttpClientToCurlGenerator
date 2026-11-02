# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

`HttpClientToCurl` is a NuGet library (netstandard2.1, no runtime deps beyond `Microsoft.Extensions.*`) that turns an `HttpClient` / `HttpRequestMessage` into a curl command string. Solution: `HttpClientToCurlGenerator.sln`.

## Commands

```bash
dotnet build
dotnet test                                                     # all test projects
dotnet test tests/HttpClientToCurlTest                          # one test project
dotnet test --filter "FullyQualifiedName~SomeTestName"          # single test
```

CI (`.github/workflows/dotnet.yml`) uses .NET 9 SDK: restore, build, test.

## Git hooks / conventions

- Building any project copies `pre-commit` and `pre-push` from the repo root into `.git/hooks/` (via `Directory.Build.props`). `pre-commit` runs `dotnet clean; dotnet build; dotnet test` when staged files include `.cs`.
- `pre-push` enforces branch names like `feature|bugfix|hotfix|refactor|optimize|document/HCG-<1-5 digits>` (or main/master/dev/develop).

## Architecture

Two entry modes share one generation core:

- **Manual mode** — extension methods in `src/HttpClientToCurl/Extensions/HttpClientExtensions.cs` and `HttpRequestMessageExtensions.cs` (`GenerateCurlInConsole/InFile/InString`). Each takes a config lambda.
- **Automatic mode** — `ServiceCollectionExtensions` registers `CurlGeneratorHttpMessageHandler` (a `DelegatingHandler`) globally (all `IHttpClientFactory` clients) or per named client, via `HttpMessageHandlerAppender`. The handler reads `IOptionsMonitor<CompositConfig>` and, per request, delegates to the same manual extension methods for console / file / `ILogger` output. New config options must be copied through in the handler's per-sink config lambdas, or they are silently dropped in automatic mode.

Generation core (`src/HttpClientToCurl/Builder/`): `Generator.GenerateCurl` picks an `IBuilder` by HTTP method (GET/POST/PUT/PATCH/DELETE only; others throw `NotSupportedException`). Builders in `Builder/Concrete/` derive from `BaseBuilder`, with shared logic in `Concrete/Common/` (`Extensions.cs`, `Constants.cs`). There are two `CreateCurl` overloads: with an `HttpClient` (uses its BaseAddress/default headers) and with a `Uri baseAddress`.

Config classes (`src/HttpClientToCurl/Config/`): `BaseConfig` → `ConsoleConfig`, `FileConfig`, `StringConfig`; `CompositConfig` bundles sinks for DI/appsettings; `LoggerConfig` adds `LogLevel`. Options such as `EnableCompression`, `NeedAddDefaultHeaders`, and header redaction live here.

## Tests

`tests/` has three projects: `HttpClientToCurlTest` (HttpClient extensions), `HttpRequestMessageToCurlTest` (HttpRequestMessage extensions), and `HttpMessageHandlerTest` (handler/DI, with hand-written fakes under `UnitTest/Fakes`). Unit tests are organised by media type (`Json`, `Xml`), split into Success/Failed. `examples/` holds runnable sample apps (console, file, string, global DI, specific-client DI).

## Notes

- `Directory.Build.props` sets `Nullable` to `annotations` and enables `EnforceCodeStyleInBuild`; follow `.editorconfig`.
- Package version is in `src/HttpClientToCurl/HttpClientToCurl.csproj`.
