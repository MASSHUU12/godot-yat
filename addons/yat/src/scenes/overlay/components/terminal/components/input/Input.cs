using Godot;
using YAT.Helpers;

namespace YAT.Scenes.Overlay.Components.Terminal
{
	public partial class Input : LineEdit
	{
		private YAT _yat;
		private CommandManager _commandManager;

		public override void _Ready()
		{
			_yat = GetNode<YAT>("/root/YAT");
			_commandManager = _yat.GetNode<CommandManager>("CommandManager");

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
			input = TextHelper.ConcatenateSentence(input);

			if (input.Length == 0 || _commandManager.Locked) return;

			_yat.HistoryNode = null;
			_yat.History.AddLast(command);
			if (_yat.History.Count > _yat.Options.HistoryLimit) _yat.History.RemoveFirst();

			_commandManager.Run(input);
			Clear();
		}

		/// <summary>
		/// Moves the caret to the end of the input text.
		/// </summary>
		public void MoveCaretToEnd()
		{
			CaretColumn = Text.Length;
		}
	}
}
