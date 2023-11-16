using Godot;
using YAT.Overlay.Components.Terminal;

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
			if (_terminalValid) Terminal.Print(message, printType);
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
		/// Logs an error message indicating that an invalid argument was passed to a command.
		/// </summary>
		/// <param name="command">The name of the command.</param>
		/// <param name="argument">The name of the invalid argument.</param>
		/// <param name="expected">The expected type of the argument.</param>
		public static void InvalidArgument(string command, string argument, string expected)
		{
			var message = string.Format(
				"{0} expected [i]{1}[/i] to be an [i]{2}[/i].",
				command,
				argument,
				expected
			);

			PrintMessage(message, Terminal.PrintType.Error);
		}

		/// <summary>
		/// Prints an error message indicating that one or more arguments are missing for the specified command.
		/// </summary>
		/// <param name="command">The name of the command that is missing arguments.</param>
		/// <param name="args">The names of the missing arguments.</param>
		public static void MissingArguments(string command, params string[] args)
		{
			var message = string.Format(
				"{0} expected [i]{1}[/i] to be provided.",
				command,
				string.Join(", ", args)
			);

			PrintMessage(message, Terminal.PrintType.Error);
		}

		/// <summary>
		/// Prints an error message indicating that an invalid argument type was passed to a command.
		/// </summary>
		/// <param name="command">The name of the command.</param>
		/// <param name="argument">The name of the argument.</param>
		/// <param name="expected">The expected type of the argument.</param>
		public static void InvalidArgumentType(string command, string argument, string expected)
		{
			var message = string.Format(
				"{0} expected [i]{1}[/i] to be an [i]{2}[/i].",
				command,
				argument,
				expected
			);

			PrintMessage(message, Terminal.PrintType.Error);
		}

		/// <summary>
		/// Prints an error message indicating that a required value is missing.
		/// </summary>
		/// <param name="command">The name of the command that is missing the value.</param>
		/// <param name="option">The name of the option that is missing the value.</param>
		public static void MissingValue(string command, string option)
		{
			var message = string.Format(
				"{0} expected [i]{1}[/i] to be provided.",
				command,
				option
			);

			PrintMessage(message, Terminal.PrintType.Error);
		}

		public static void Error(string message) => PrintMessage(message, Terminal.PrintType.Error);
	}
}
