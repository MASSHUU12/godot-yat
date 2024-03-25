using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace YAT.Helpers;

public static class OS
{
	public static OperatingSystem Platform { get; private set; }
	public static string DefaultTerminal { get; private set; } = string.Empty;

	public enum OperatingSystem
	{
		Unknown,
		Windows,
		Linux,
		OSX
	}

	public enum ExecutionResult
	{
		Success,
		CannotExecute,
		ErrorExecuting,
		UnknownPlatform
	}

	static OS()
	{
		CheckOSPlatform();
		CheckDefaultTerminal();
	}

	public static StringBuilder RunCommand(string command, out ExecutionResult result, string program = "", string args = "")
	{
		StringBuilder output = new();
		result = ExecutionResult.Success;

		if (Platform == OperatingSystem.Unknown)
		{
			output.AppendLine("Cannot run command, unknown platform.");
			result = ExecutionResult.UnknownPlatform;

			return output;
		}

		if (string.IsNullOrEmpty(program)) program = DefaultTerminal;

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

		try
		{
			process.Start();
			process.StandardInput.WriteLine(command + ' ' + args);
			process.StandardInput.Flush();
			process.StandardInput.Close();

			string outputString = process.StandardOutput.ReadToEnd();
			string errorString = process.StandardError.ReadToEnd();

			process.WaitForExit();

			if (!string.IsNullOrEmpty(outputString))
				output.AppendLine(outputString);
			if (!string.IsNullOrEmpty(errorString))
				output.AppendLine(errorString);
		}
		catch (Exception ex)
		{
			output.AppendLine($"Error executing command: {ex.Message}");
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
			_ => string.Empty
		};
	}
}
