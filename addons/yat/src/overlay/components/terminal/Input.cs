using Godot;
using YAT.Helpers;

namespace YAT.Overlay.Components.Terminal
{
	public partial class Input : LineEdit
	{
		private YAT _yat;
		private Terminal _terminal;

		public override void _Ready()
		{
			_yat = GetNode<YAT>("/root/YAT");
			_terminal = GetNode<Terminal>("../../../../../../..");

			TextSubmitted += OnTextSubmitted;
		}

		/// <summary>
		/// Handles the submission of a command by sanitizing the input,
		/// executing the command, and clearing the input buffer.
		/// </summary>
		/// <param name="command">The command to be submitted.</param>
		private void OnTextSubmitted(string command)
		{
			var input = TextHelper.SanitizeText(command);

			if (input.Length == 0 || _terminal.Locked) return;

			_yat.HistoryNode = null;
			_yat.History.AddLast(command);
			if (_yat.History.Count > _yat.Options.HistoryLimit) _yat.History.RemoveFirst();

			_terminal.CommandManager(input);
			Clear();
		}
	}
}
