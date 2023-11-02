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
		/// Prints an error message indicating that an unknown command was entered.
		/// </summary>
		/// <param name="command">The unknown command that was entered.</param>
		public static void UnknownCommand(string command)
		{
			if (!_terminalValid) return;
			Terminal.Println($"Unknown command: {command}", Terminal.PrintType.Error);
		}
	}
}
