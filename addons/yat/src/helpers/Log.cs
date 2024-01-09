using System;
using System.Diagnostics;
using System.Text;
using Godot;
using YAT.Scenes.BaseTerminal;
using static YAT.Scenes.BaseTerminal.BaseTerminal;

namespace YAT.Helpers
{
	public sealed class Log
	{
		public static BaseTerminal Terminal { get; set; }

		[Flags]
		public enum LogOutput
		{
			None = 0b000,
			Terminal = 0b001,
			Editor = 0b010,
			EditorRich = 0b100
		}

		private static LogOutput _enabledOutputs = LogOutput.Terminal;

		public Log(LogOutput enabledOutputs)
		{
			_enabledOutputs = enabledOutputs;
		}

		public static void Print(string message)
		{
			if ((_enabledOutputs & LogOutput.Terminal) == LogOutput.Terminal)
				Terminal.Print(message);

			if ((_enabledOutputs & LogOutput.Editor) == LogOutput.Editor)
				GD.Print(message);

			if ((_enabledOutputs & LogOutput.EditorRich) == LogOutput.EditorRich)
				GD.PrintRich(message);
		}

		public static void Error(string message)
		{
			if ((_enabledOutputs & LogOutput.Terminal) == LogOutput.Terminal)
				Terminal.Print(message, PrintType.Error);

			if ((_enabledOutputs & LogOutput.Editor) == LogOutput.Editor)
				GD.PushError(message);

			if ((_enabledOutputs & LogOutput.EditorRich) == LogOutput.EditorRich)
				GD.PushError(message);
		}

		public static void Warning(string message)
		{
			if ((_enabledOutputs & LogOutput.Terminal) == LogOutput.Terminal)
				Terminal.Print(message, PrintType.Warning);

			if ((_enabledOutputs & LogOutput.Editor) == LogOutput.Editor)
				GD.PushWarning(message);

			if ((_enabledOutputs & LogOutput.EditorRich) == LogOutput.EditorRich)
				GD.PushWarning(message);
		}

		public static void Info(string message)
		{
			if ((_enabledOutputs & LogOutput.Terminal) == LogOutput.Terminal)
				Terminal.Print(message, PrintType.Normal);

			if ((_enabledOutputs & LogOutput.Editor) == LogOutput.Editor)
				GD.Print(message);

			if ((_enabledOutputs & LogOutput.EditorRich) == LogOutput.EditorRich)
				GD.PrintRich(message);
		}

		public static void Success(string message)
		{
			if ((_enabledOutputs & LogOutput.Terminal) == LogOutput.Terminal)
				Terminal.Print(message, PrintType.Success);

			if ((_enabledOutputs & LogOutput.Editor) == LogOutput.Editor)
				GD.Print(message);

			if ((_enabledOutputs & LogOutput.EditorRich) == LogOutput.EditorRich)
				GD.PrintRich(message);
		}

		public static void Debug(string message)
		{
#if DEBUG
			message = $"[DEBUG] {message}";

			if ((_enabledOutputs & LogOutput.Terminal) == LogOutput.Terminal)
				Terminal.Print(message);

			if ((_enabledOutputs & LogOutput.Editor) == LogOutput.Editor)
				GD.Print(message);

			if ((_enabledOutputs & LogOutput.EditorRich) == LogOutput.EditorRich)
				GD.PrintRich(message);
#endif
		}

		public static void Trace(string message, bool detailed = false)
		{
#if DEBUG
			if (!detailed) message = $"[TRACE] {message}: {System.Environment.StackTrace}";
			else
			{
				StackTrace st = new();
				StringBuilder sb = new();

				sb.Append($"[TRACE] {message}");

				for (int i = 0; i < st.FrameCount; i++)
				{
					StackFrame sf = st.GetFrame(i);

					sb.Append(string.Format(
						"\n{0}({1},{2}): {3}.{4}",
						sf.GetFileName(),
						sf.GetFileLineNumber(),
						sf.GetFileColumnNumber(),
						sf.GetMethod().DeclaringType,
						sf.GetMethod())
					);
				}

				message = sb.ToString();
			}

			if ((_enabledOutputs & LogOutput.Terminal) == LogOutput.Terminal)
				Terminal.Print(message);

			if ((_enabledOutputs & LogOutput.Editor) == LogOutput.Editor)
				GD.Print(message);

			if ((_enabledOutputs & LogOutput.EditorRich) == LogOutput.EditorRich)
				GD.PrintRich(message);
#endif
		}
	}
}
