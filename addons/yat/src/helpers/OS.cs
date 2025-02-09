using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace YAT.Helpers;

public static partial class OS
{
    public static EOperatingSystem Platform { get; private set; }
    public static string DefaultTerminal { get; private set; } = string.Empty;

    // TODO: Remove this
    public enum EOperatingSystem
    {
        Unknown = 0,
        Windows = 1,
        Linux = 2,
        OSX = 3
    }

    public enum EExecutionResult
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
        out EExecutionResult result,
        string program = "",
        string args = "",
        int timeoutMilis = 10000
    )
    {
        StringBuilder output = new();
        result = EExecutionResult.Success;

        if (Platform == EOperatingSystem.Unknown)
        {
            _ = output.AppendLine("Cannot run command, unknown platform.");
            result = EExecutionResult.UnknownPlatform;

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
                result = EExecutionResult.Timeout;
            }
            else
            {
                result = EExecutionResult.Success;
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
            result = EExecutionResult.ErrorExecuting;

            return output;
        }

        return output;
    }

    private static void CheckOSPlatform()
    {
        Platform = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? EOperatingSystem.Windows
            : RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                ? EOperatingSystem.Linux
                : RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                    ? EOperatingSystem.OSX
                    : EOperatingSystem.Unknown;
    }

    private static void CheckDefaultTerminal()
    {
        DefaultTerminal = Platform switch
        {
            EOperatingSystem.Windows => "cmd.exe",
            EOperatingSystem.Linux => "/bin/bash",
            EOperatingSystem.OSX => "/bin/bash",
            EOperatingSystem.Unknown => throw new NotImplementedException(),
            _ => string.Empty
        };
    }

    public static bool IsRunningAsAdmin()
    {
#if GODOT_WINDOWS
        using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
        {
            WindowsPrincipal principal = new(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
#else // Unix-like systems (Linux, macOS) check
        return GetEuid() == 0;
#endif
    }

#if !GODOT_WINDOWS
    [LibraryImport("libc", EntryPoint = "geteuid")]
    private static partial uint GetEuid();
#endif
}
