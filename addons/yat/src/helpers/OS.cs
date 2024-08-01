using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace YAT.Helpers;

public static class OS
{
    public static OperatingSystem Platform { get; private set; }
    public static string DefaultTerminal { get; private set; } = string.Empty;

    public enum OperatingSystem
    {
        Unknown = 0,
        Windows = 1,
        Linux = 2,
        OSX = 3
    }

    public enum ExecutionResult
    {
        Success = 0,
        CannotExecute = 1,
        ErrorExecuting = 2,
        UnknownPlatform = 3,
        Timeout = 4
    }

    static OS()
    {
        CheckOSPlatform();
        CheckDefaultTerminal();
    }

    public static StringBuilder RunCommand(
        string command,
        out ExecutionResult result,
        string program = "",
        string args = "",
        int timeoutMilis = 10000
    )
    {
        StringBuilder output = new();
        result = ExecutionResult.Success;

        if (Platform == OperatingSystem.Unknown)
        {
            _ = output.AppendLine("Cannot run command, unknown platform.");
            result = ExecutionResult.UnknownPlatform;

            return output;
        }

        if (string.IsNullOrEmpty(program))
        {
            program = DefaultTerminal;
        }

        ProcessStartInfo startInfo = new()
        {
            FileName = program,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using Process process = new() { StartInfo = startInfo };

        StringBuilder outputStandard = new();
        StringBuilder outputError = new();

        try
        {
            _ = process.Start();
            process.StandardInput.WriteLine(command + ' ' + args);

            process.StandardInput.Flush();
            process.StandardInput.Close();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.OutputDataReceived +=
                (sender, data) => outputStandard.AppendLine(data.Data);
            process.ErrorDataReceived +=
                (sender, data) => outputError.AppendLine(data.Data);

            if (!process.WaitForExit(timeoutMilis))
            {
                process.Kill();
                result = ExecutionResult.Timeout;
            }
            else
            {
                result = ExecutionResult.Success;
            }

            _ = output.Append(outputStandard);
            _ = output.Append(outputError);
        }
        catch (Exception ex)
        {
            _ = output.AppendLine(
                CultureInfo.InvariantCulture,
                $"Error executing command: {ex.Message}"
            );
            result = ExecutionResult.ErrorExecuting;

            return output;
        }

        return output;
    }

    private static void CheckOSPlatform()
    {
        Platform = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? OperatingSystem.Windows
            : RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                ? OperatingSystem.Linux
                : RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                    ? OperatingSystem.OSX
                    : OperatingSystem.Unknown;
    }

    private static void CheckDefaultTerminal()
    {
        DefaultTerminal = Platform switch
        {
            OperatingSystem.Windows => "cmd.exe",
            OperatingSystem.Linux => "/bin/bash",
            OperatingSystem.OSX => "/bin/bash",
            OperatingSystem.Unknown => throw new NotImplementedException(),
            _ => string.Empty
        };
    }
}
