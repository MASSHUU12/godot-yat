using Godot;

namespace YAT.Helpers
{
	public static class LogHelper
	{
		public static Terminal Terminal
		{
			get => _terminal;
			set
			{
				_terminal = value;
				_terminalValid = GodotObject.IsInstanceValid(_terminal);
			}
		}

		private static Terminal _terminal;
		private static bool _terminalValid = false;

		/// <summary>
		/// Prints a message to the console or Godot terminal.
		/// </summary>
		/// <param name="message">The message to print.</param>
		/// <param name="printType">The type of message to print.</param>
		/// <param name="useGDTerminal">Whether to print also to the Godot terminal.</param>
		private static void PrintMessage(string message, Terminal.PrintType printType, bool useGDTerminal = false)
		{
			if (useGDTerminal) GD.PushError(message);
			if (_terminalValid) Terminal.Println(message, printType);
		}

		/// <summary>
		/// Prints an error message indicating that an unknown command was entered.
		/// </summary>
		/// <param name="command">The unknown command that was entered.</param>
		public static void UnknownCommand(string command)
		{
			var message = $"Unknown command: {command}";
			PrintMessage(message, Terminal.PrintType.Error);
		}
	}
}
