using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace YAT.Helpers;

public static partial class OS
{
#if GODOT_WINDOWS
    public const string DefaultTerminal = "cmd.exe";
#else
    public const string DefaultTerminal = "/bin/bash";
    public static readonly bool UsingWayland = true;

    static OS()
    {
        UsingWayland = RunCommand(
            "echo $XDG_SESSION_TYPE",
            out EExecutionResult _
        ).ToString().Trim() == "wayland";
    }
#endif

    public enum EExecutionResult
    {
        Success = 0,
        CannotExecute = 1,
        ErrorExecuting = 2,
        UnknownPlatform = 3,
        Timeout = 4
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
