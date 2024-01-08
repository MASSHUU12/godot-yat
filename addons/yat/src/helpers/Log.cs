using Godot;
using YAT.Scenes.BaseTerminal;
using static YAT.Scenes.BaseTerminal.BaseTerminal;

namespace YAT.Helpers
{
	public sealed class Log
	{
		public enum LogOutput
		{
			Terminal,
			Editor,
			EditorRich
		}

		private static LogOutput[] _enabledOutputs = new[] { LogOutput.Terminal };

		public Log(params LogOutput[] enabledOutputs)
		{
			_enabledOutputs = enabledOutputs;
		}

		public static void Print(string message)
		{
			foreach (LogOutput output in _enabledOutputs)
			{
				switch (output)
				{
					case LogOutput.Terminal:
						GD.Print(message);
						break;
					case LogOutput.Editor:
						GD.Print(message);
						break;
					case LogOutput.EditorRich:
						GD.PrintRich(message);
						break;
				}
			}
		}

		public static void Error(string message)
		{ }

		public static void Warning(string message)
		{ }

		public static void Info(string message)
		{ }

		public static void Debug(string message)
		{ }

		public static void Trace(string message)
		{ }
	}
}
