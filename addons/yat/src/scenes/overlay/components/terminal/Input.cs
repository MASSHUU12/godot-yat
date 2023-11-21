using System;
using System.Linq;
using Godot;
using YAT.Attributes;
using YAT.Helpers;

namespace YAT.Scenes.Overlay.Components.Terminal
{
	public partial class Input : LineEdit
	{
		private YAT _yat;
		private CommandManager _commandManager;
		private Terminal _terminal;
		private string cachedInput = string.Empty;
		private string[] suggestions = Array.Empty<string>();
		private uint suggestionIndex = 0;

		public override void _Ready()
		{
			_yat = GetNode<YAT>("/root/YAT");
			_commandManager = _yat.GetNode<CommandManager>("CommandManager");
			_terminal = GetNode<Terminal>("../../../..");

			TextSubmitted += OnTextSubmitted;
		}

		public override void _GuiInput(InputEvent @event)
		{
			if (@event.IsActionPressed("yat_terminal_autocompletion") && HasFocus())
			{
				Autocompletion();
				CallDeferred("grab_focus"); // Prevent toggling the input focus
			}
		}

		/// <summary>
		/// Provides suggestions for autocompletion of user input in the terminal.
		/// </summary>
		private void Autocompletion()
		{
			// Command structure:
			// command_name args
			// command_name subcommand_name args

			if (suggestions.Length > 0 && (
				Text == cachedInput || suggestions.Contains(Text)
			))
			{
				UseNextSuggestion();
				return;
			}

			cachedInput = Text;
			suggestions = Array.Empty<string>();
			suggestionIndex = 0;

			var tokens = TextHelper.SanitizeText(Text);

			if (tokens.Length == 1)
			{
				suggestions = GenerateCommandSuggestions(tokens[0]);

				if (suggestions.Length > 0) UseNextSuggestion();

				return;
			}
		}

		/// <summary>
		/// Selects the next suggestion from the list of suggestions
		/// and updates the text input with it.
		/// </summary>
		private void UseNextSuggestion()
		{
			if (suggestions.Length == 0) return;

			suggestionIndex = (uint)((suggestionIndex + 1) % suggestions.Length);
			Text = suggestions[suggestionIndex];

			MoveCaretToEnd();
		}

		/// <summary>
		/// Generates an array of command suggestions based on the input state.
		/// </summary>
		/// <param name="token">The current input state.</param>
		/// <returns>An array of command suggestions.</returns>
		private string[] GenerateCommandSuggestions(string token)
		{
			return _yat.Commands
				.Where(x => x.Value.GetAttribute<CommandAttribute>()?.Name?.StartsWith(token) == true)
				.Select(x => x.Value.GetAttribute<CommandAttribute>()?.Name)
				.Where(name => !string.IsNullOrEmpty(name))
				.Distinct()
				.ToArray();
		}

		/// <summary>
		/// Handles the submission of a command by sanitizing the input,
		/// executing the command, and clearing the input buffer.
		/// </summary>
		/// <param name="command">The command to be submitted.</param>
		private void OnTextSubmitted(string command)
		{
			var input = TextHelper.SanitizeText(command);

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
