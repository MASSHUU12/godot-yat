using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;
using YAT.Attributes;
using YAT.Helpers;

namespace YAT.Scenes;

public partial class Autocompletion : Node
{
#nullable disable
    [Export] public CommandInfo CommandInfo { get; set; }

    private YAT _yat;
    private Input _input;

    private string cachedInput = string.Empty;
    private LinkedList<string> suggestions = new();
    private LinkedListNode<string> currentSuggestion;
#nullable restore

    public override void _Ready()
    {
        _yat = GetNode<YAT>("/root/YAT");

        _input = CommandInfo.Input;
    }

    public override void _Input(InputEvent @event)
    {
        if (!_input.HasFocus())
        {
            return;
        }

        if (@event.IsActionPressed(Keybindings.TerminalAutocompletionPrevious))
        {
            Autocomplete(false);
            _ = _input.CallDeferred("grab_focus"); // Prevent toggling the input focus
        }
        else if (@event.IsActionPressed(Keybindings.TerminalAutocompletionNext))
        {
            Autocomplete();
            _ = _input.CallDeferred("grab_focus"); // Prevent toggling the input focus
        }
    }

    private void Autocomplete(bool next = true)
    {
        if (suggestions.Count > 0 && (_input.Text == cachedInput || suggestions.Contains(_input.Text)))
        {
            if (next)
            {
                UseNextSuggestion();
            }
            else
            {
                UsePreviousSuggestion();
            }

            return;
        }

        cachedInput = _input.Text;
        suggestions = new();
        currentSuggestion = null;

        string[] tokens = Text.SanitizeText(_input.Text);

        if (tokens.Length == 1)
        {
            suggestions = GenerateCommandSuggestions(tokens[0]);

            if (suggestions.Count > 0)
            {
                UseNextSuggestion();
            }

            return;
        }
    }

    private void UseNextSuggestion()
    {
        if (suggestions.Count == 0)
        {
            return;
        }

        currentSuggestion = currentSuggestion?.Next ?? suggestions.First;
        _input.Text = currentSuggestion?.Value ?? string.Empty;

        _input.MoveCaretToEnd();

        CommandInfo.UpdateCommandInfo(_input.Text);
    }

    private void UsePreviousSuggestion()
    {
        if (suggestions.Count == 0)
        {
            return;
        }

        currentSuggestion = currentSuggestion?.Previous ?? suggestions.Last;
        _input.Text = currentSuggestion?.Value ?? string.Empty;

        _input.MoveCaretToEnd();

        CommandInfo.UpdateCommandInfo(_input.Text);
    }

    private static LinkedList<string> GenerateCommandSuggestions(string token)
    {
        List<string>? listSuggestions = RegisteredCommands.Registered
            ?.Where(x => x.Value.GetCustomAttribute<CommandAttribute>()?.Name?.StartsWith(token) == true)
            ?.Select(x => x.Value.GetCustomAttribute<CommandAttribute>()?.Name ?? string.Empty)
            ?.Where(name => !string.IsNullOrEmpty(name))
            ?.Distinct()
            ?.ToList();

        return listSuggestions is null ? new() : new(listSuggestions);
    }
}
