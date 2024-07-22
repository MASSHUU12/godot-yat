using Godot;

namespace YAT.Update;

[Tool]
public partial class UpdaterWindow : Window
{
#nullable disable
    private TextEdit _output;
    private Button _update, _cancel;
#nullable restore

    public override void _Ready()
    {
        _cancel = GetNode<Button>("%Cancel");
        _update = GetNode<Button>("%Update");
        _output = GetNode<TextEdit>("%Output");

        _cancel.Pressed += QueueFree;
        CloseRequested += QueueFree;
    }
}
