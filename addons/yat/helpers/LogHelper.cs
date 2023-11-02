using Godot;

namespace YAT.Helpers
{
	public static class LogHelper
	{
		public static Terminal Terminal
		{
			get => Terminal;
			set
			{
				Terminal = value;
				_terminalValid = GodotObject.IsInstanceValid(value);
			}
		}
		private static bool _terminalValid = false;
	}
}
