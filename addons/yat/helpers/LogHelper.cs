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
		private static void PrintMessage(string message, Terminal.PrintType printType = Terminal.PrintType.Error, bool useGDTerminal = false)
		{
			if (useGDTerminal) GD.PushError(message);
			if (_terminalValid) Terminal.Println(message, printType);
		}

		/// <summary>
		/// Prints an error message indicating that an unknown command was entered.
		/// </summary>
		/// <param name="command">The unknown command that was entered.</param>
		public static void UnknownCommand(string command) => PrintMessage($"Unknown command: {command}");

		/// <summary>
		/// Logs a message indicating that a required attribute is missing.
		/// </summary>
		/// <param name="attribute">The name of the missing attribute.</param>
		/// <param name="name">The name of the object that is missing the attribute.</param>
		/// <param name="useGDTerminal">Whether to print the message to the Godot terminal.</param>
		public static void MissingAttribute(string attribute, string name, bool useGDTerminal = true)
		{
			var message = string.Format(
							"Missing attribute: {0} on {1}",
							attribute,
							name
						);

			PrintMessage(message, Terminal.PrintType.Error, useGDTerminal);
		}

		/// <summary>
		/// Prints an error message indicating that the number of arguments provided for a command is invalid.
		/// </summary>
		/// <param name="command">The name of the command.</param>
		/// <param name="expected">The expected number of arguments.</param>
		/// <param name="actual">The actual number of arguments.</param>
		public static void InvalidArguments(string command, uint expected, uint actual)
		{
			var message = string.Format(
							"Invalid number of arguments for: {0}. Expected {1}, got {2}.",
							command,
							expected,
							actual
						);

			PrintMessage(message, Terminal.PrintType.Error);
		}
	}
}
