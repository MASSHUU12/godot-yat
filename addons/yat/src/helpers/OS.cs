using System.Diagnostics;
using System.Runtime.InteropServices;

namespace YAT.Helpers
{
	public static class OS
	{
		public static OperatingSystem Platform { get; private set; }
		public static string DefaultTerminal { get; private set; }

		public enum OperatingSystem
		{
			Unknown,
			Windows,
			Linux,
			OSX
		}

		static OS()
		{
			CheckOSPlatform();
			CheckDefaultTerminal();
		}

		/// <summary>
		/// Runs a command using the specified program and arguments.
		/// </summary>
		/// <param name="command">The command to run.</param>
		/// <param name="program">
		/// The program to use for running the command.
		/// If empty, the default terminal will be used.</param>
		/// <param name="args">The arguments to pass to the program.</param>
		public static void RunCommand(string command, string program = "", string args = "")
		{
			if (Platform == OperatingSystem.Unknown)
			{
				Log.Error("Cannot run command, unknown platform.");
				return;
			}

			if (string.IsNullOrEmpty(program))
			{
				if (string.IsNullOrEmpty(DefaultTerminal)) return;

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

			process.Start();
			process.StandardInput.WriteLine(command + ' ' + args);
			process.StandardInput.Flush();
			process.StandardInput.Close();

			string output = process.StandardOutput.ReadToEnd();
			string error = process.StandardError.ReadToEnd();

			process.WaitForExit();
			process.Close();

			if (!string.IsNullOrEmpty(output)) Log.Info(output);
			if (!string.IsNullOrEmpty(error)) Log.Error(error);
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
			if (Platform == OperatingSystem.Windows) DefaultTerminal = "cmd.exe";
			else if (Platform == OperatingSystem.Linux) DefaultTerminal = "/bin/bash";
			else if (Platform == OperatingSystem.OSX) DefaultTerminal = "/bin/bash";
			else DefaultTerminal = string.Empty;
		}
	}
}
