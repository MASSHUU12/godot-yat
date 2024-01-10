using System.Text;
using Godot;
using YAT.Attributes;
using YAT.Helpers;

namespace YAT.Scenes.BaseTerminal.Components.InputInfo
{
	public partial class CommandInfo : Node
	{
		[Export] public InputInfo InputInfo { get; set; }
		[Export] public Input Input { get; set; }

		private YAT _yat;

		public override void _Ready()
		{
			_yat = GetNode<YAT>("/root/YAT");

			Input.TextChanged += UpdateCommandInfo;
		}

		public void UpdateCommandInfo(string text)
		{
			var tokens = Text.SanitizeText(text);

			if (!AreTokensValid(tokens)) return;

			InputInfo.DisplayCommandInfo(GenerateCommandInfo(tokens));
		}

		private string GenerateCommandInfo(string[] tokens)
		{
			var command = _yat.Commands[tokens[0]];
			CommandAttribute commandAttribute = command.GetAttribute<CommandAttribute>()!;
			var commandArguments = command.GetAttributes<ArgumentAttribute>();

			StringBuilder commandInfo = new();
			commandInfo.Append(commandAttribute.Name);

			if (commandArguments is null) return commandInfo.ToString();

			for (int i = 0; i < commandArguments.Length; i++)
			{
				string key = commandArguments[i].Name;
				var arg = commandArguments[i].Type;
				bool current = tokens.Length - 1 == i;
				bool valid = CommandHelper.ValidateCommandArgument(
					commandAttribute.Name,
					arg,
					new() { { key, arg } },
					(tokens.Length - 1 >= i + 1) ? tokens[i + 1] : string.Empty,
					false
				);

				string argument = string.Format(
					" {0}{1}<{2}:{3}>{4}{5}",
					valid ? string.Empty : $"[color={_yat.Options.ErrorColor.ToHtml()}]",
					current ? "[b]" : string.Empty,
					key,
					(arg is string[]) ? "options" : arg,
					current ? "[/b]" : string.Empty,
					valid ? string.Empty : "[/color]"
				);

				commandInfo.Append(argument);

				if (i < commandArguments.Length - 1) commandInfo.Append(' ');
			}

			return commandInfo.ToString();
		}

		private bool AreTokensValid(string[] tokens)
		{
			if (tokens.Length == 0 || !_yat.Commands.ContainsKey(tokens[0]))
			{
				InputInfo.Visible = false;
				return false;
			}
			return true;
		}
	}
}
