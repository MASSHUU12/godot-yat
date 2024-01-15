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

		public static void RunCommand(string command, string program, string args = "")
		{
			if (Platform == _unknown) return;

			if (string.IsNullOrEmpty(program))
			{
				if (string.IsNullOrEmpty(DefaultTerminal)) return;

				program = DefaultTerminal;
			}

			if (Platform == OSPlatform.Windows) RunWindowsCommand(command, program, args);
			else if (Platform == OSPlatform.Linux) RunLinuxCommand(command, program, args);
			else if (Platform == OSPlatform.OSX) RunMacOSCommand(command, program, args);
		}

		private static void RunWindowsCommand(string command, string program, string args)
		{
			Log.Debug($"Running command '{command}' with args '{args}' with program: {program}");
		}

		private static void RunLinuxCommand(string command, string program, string args)
		{
		}

		private static void RunMacOSCommand(string command, string program, string args)
		{
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
