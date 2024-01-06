using System.Text;
using Godot;
using YAT.Helpers;

namespace YAT.Scenes.BaseTerminal
{
	public partial class BaseTerminal : Control
	{
		public Input Input { get; private set; }
		public TerminalContext Context { get; private set; }
		public SelectedNode SelectedNode { get; private set; }

		/// <summary>
		/// The type of message to print in the YatTerminal.
		/// </summary>
		public enum PrintType
		{
			/// <summary>
			/// Represents the normal state of the YatTerminal component.
			/// </summary>
			Normal,
			/// <summary>
			/// Displays a error message in the terminal.
			/// </summary>
			Error,
			/// <summary>
			/// Displays a warning message in the terminal.
			/// </summary>
			Warning,
			/// <summary>
			/// Displays a success message in the terminal.
			/// </summary>
			Success
		}

		private YAT _yat;
		private Label _promptLabel;
		private Label _selectedNodeLabel;
		private RichTextLabel Output;
		private string _prompt = "> ";
		private CommandManager.CommandManager _commandManager;
	}
}
