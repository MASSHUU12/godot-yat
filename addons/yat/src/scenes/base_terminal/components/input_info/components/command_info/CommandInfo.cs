using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Godot;
using YAT.Attributes;
using YAT.Helpers;

namespace YAT.Scenes;

public partial class CommandInfo : Node
{
#nullable disable
    [Export] public Input Input { get; set; }
    [Export] public InputInfo InputInfo { get; set; }
    [Export] public BaseTerminal Terminal { get; set; }

    private YAT _yat;
#nullable restore

    public override void _Ready()
    {
        _yat = GetNode<YAT>("/root/YAT");

        Input.TextChanged += UpdateCommandInfo;
    }

    public void UpdateCommandInfo(string text)
    {
        string[] tokens = Text.SanitizeText(text);

        if (!AreTokensValid(tokens))
        {
            return;
        }

        InputInfo.DisplayCommandInfo(GenerateCommandInfo(tokens));
    }

    private string GenerateCommandInfo(string[] tokens)
    {
        Type command = RegisteredCommands.Registered[tokens[0]];
        CommandAttribute? commandAttribute = command.GetCustomAttribute<CommandAttribute>();
        IEnumerable<ArgumentAttribute>? commandArguments = command.GetCustomAttributes<ArgumentAttribute>();

        StringBuilder commandInfo = new();
        _ = commandInfo.Append(commandAttribute?.Name);

        if (commandArguments is null)
        {
            return commandInfo.ToString();
        }

        uint i = 0;
        uint count = (uint)commandArguments.Count();
        foreach (ArgumentAttribute arg in commandArguments)
        {
            bool current = tokens.Length - 1 == i;
            bool valid = Terminal.CommandValidator.ValidateCommandArgument(
                arg,
                new() { { arg.Name, arg.Types } },
                (tokens.Length - 1 >= i + 1) ? tokens[i + 1] : string.Empty,
                false
            );

            string argument = string.Format(
                " {0}{1}<{2}:{3}>{4}{5}",
                valid ? string.Empty : $"[color={_yat.PreferencesManager.Preferences.ErrorColor.ToHtml()}]",
                current ? "[b]" : string.Empty,
                arg.Name,
                string.Join(" | ", arg.Types.Select(t => t.TypeDefinition)),
                current ? "[/b]" : string.Empty,
                valid ? string.Empty : "[/color]"
            );

            _ = commandInfo.Append(argument);

            if (i < count - 1)
            {
                _ = commandInfo.Append(' ');
            }

            i++;
        }

        return commandInfo.ToString();
    }

    private bool AreTokensValid(string[] tokens)
    {
        if (tokens.Length == 0 || !RegisteredCommands.Registered.ContainsKey(tokens[0]))
        {
            InputInfo.Visible = false;
            return false;
        }
        return true;
    }
}
