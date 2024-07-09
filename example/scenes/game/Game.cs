using Godot;
using YAT.Classes;
using YAT.Helpers;

namespace Example;

public partial class Game : Node3D
{
#nullable disable
    private Label3D _hint;
#nullable restore

    public override void _Ready()
    {
        _hint = GetNode<Label3D>("%Hint");

        RegisterCommand();
        SetHint();
    }

    private void SetHint()
    {
        var inputMap = InputMap.GetActions();

        foreach (var action in inputMap)
        {
            if (action == Keybindings.TerminalToggle)
            {
                var inputEvent = InputMap.ActionGetEvents(action);

                _hint.Text = inputEvent.Count > 0
                            ? $"Press {inputEvent[0].AsText()} to open the YAT overlay."
                            : "Please set a key to open the YAT overlay.";
                return;
            }
        }

        _hint.Text = "Please create yat_toggle input map.";
    }

    private void RegisterCommand()
    {
        var cube = GetNode<MeshInstance3D>("Scene/Cube");

        SetCube.Cube = cube;
        Extensible.RegisterExtension("set", typeof(SetCube));
    }
}
