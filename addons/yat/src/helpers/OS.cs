using System.Diagnostics;
using System.Runtime.InteropServices;

namespace YAT.Helpers
{
	public static class OS
	{
		public static OSPlatform Platform { get; private set; }
		public static string DefaultTerminal { get; private set; }

		private static readonly OSPlatform _unknown = OSPlatform.Create("Unknown");

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
			if (Platform == _unknown) return;

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
				? OSPlatform.Windows
				: RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
				? OSPlatform.Linux
				: RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
				? OSPlatform.OSX
				: _unknown;
		}

		private static void CheckDefaultTerminal()
		{
			if (Platform == OSPlatform.Windows) DefaultTerminal = "cmd.exe";
			else if (Platform == OSPlatform.Linux) DefaultTerminal = "/bin/bash";
			else if (Platform == OSPlatform.OSX) DefaultTerminal = "/bin/bash";
			else DefaultTerminal = string.Empty;
		}
	}
}
