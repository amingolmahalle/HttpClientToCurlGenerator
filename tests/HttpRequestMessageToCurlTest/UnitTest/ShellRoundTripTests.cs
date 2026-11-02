using System.Diagnostics;
using System.Text;
using FluentAssertions;
using HttpClientToCurl.Builder;
using Xunit;

namespace HttpRequestMessageToCurlTest.UnitTest;

/// <summary>
/// Runs the generated command through a real POSIX shell (with "curl" swapped for a printer)
/// to prove that quoted values arrive as single, unmodified arguments and nothing is executed.
/// </summary>
public class ShellRoundTripTests
{
    private static readonly Uri BaseAddress = new("http://localhost:1213/v1/");

    private static List<string> RunThroughShell(string script)
    {
        script.Should().StartWith("curl");
        var command = "printf '[%s]\\n'" + script["curl".Length..];

        var startInfo = new ProcessStartInfo("sh") { RedirectStandardOutput = true, RedirectStandardError = true };
        startInfo.ArgumentList.Add("-c");
        startInfo.ArgumentList.Add(command);

        using var process = Process.Start(startInfo)!;
        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();
        process.WaitForExit();

        process.ExitCode.Should().Be(0, error);
        return output.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    [Fact]
    public void Hostile_Body_And_Header_Arrive_As_Single_Unmodified_Arguments_And_Nothing_Executes()
    {
        if (OperatingSystem.IsWindows())
        {
            return; // POSIX shell quoting; no sh available on Windows runners.
        }

        var marker = Path.Combine(Path.GetTempPath(), $"curl-injection-{Guid.NewGuid():N}");
        var body = $@"{{""name"":""O'Brien'; touch {marker}; echo '""}}";
        var header = $"x'; touch {marker}; echo 'y";

        var request = new HttpRequestMessage(HttpMethod.Post, "api/test")
        {
            Content = new StringContent(body, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("X-Evil", header);

        try
        {
            var args = RunThroughShell(Generator.GenerateCurl(request, BaseAddress, null));

            args.Should().Contain($"[{body}]");
            args.Should().Contain($"[X-Evil: {header}]");
            File.Exists(marker).Should().BeFalse("escaped quotes must not let the shell run injected commands");
        }
        finally
        {
            File.Delete(marker);
        }
    }

    [Fact]
    public void Hostile_Url_Arrives_As_Single_Argument()
    {
        if (OperatingSystem.IsWindows())
        {
            return;
        }

        var request = new HttpRequestMessage(HttpMethod.Get, "api/it's");

        var args = RunThroughShell(Generator.GenerateCurl(request, BaseAddress, null));

        args.Should().Contain("[http://localhost:1213/v1/api/it's]");
    }

    [Fact]
    public void Hostile_Http_Method_Arrives_As_Single_Unmodified_Argument()
    {
        if (OperatingSystem.IsWindows())
        {
            return;
        }

        var request = new HttpRequestMessage(new HttpMethod("A`id`"), "api/test");

        var args = RunThroughShell(Generator.GenerateCurl(request, BaseAddress, null));

        args.Should().Contain("[A`id`]", "the backticks must not be executed by the shell");
    }
}
