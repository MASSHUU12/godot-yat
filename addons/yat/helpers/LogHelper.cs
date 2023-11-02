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
	}
}
