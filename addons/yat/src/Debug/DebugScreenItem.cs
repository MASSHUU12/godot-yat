using Godot;

namespace YAT.Debug;

public partial class DebugScreenItem : PanelContainer
{
#nullable disable
    public YAT Yat { get; private set; }
    public VBoxContainer VContainer { get; private set; }
#nullable restore

    public static RichTextLabel CreateLabel(Color? color = null)
    {
        RichTextLabel label = new()
        {
            BbcodeEnabled = true,
            FitContent = true,
            AutowrapMode = 0,
            ScrollActive = false
        };

        if (color is not null)
        {
            label.AddThemeColorOverride("default_color", color.Value);
        }

        return label;
    }

    public static string ColorText(string text, Color color)
    {
        return $"[color={color}]{text}[/color]";
    }

    public override void _Ready()
    {
        Yat = GetNode<YAT>("/root/YAT");
        VContainer = new()
        {
            SizeFlagsHorizontal = 0,
            SizeFlagsVertical = 0
        };
        AddChild(VContainer);
    }

    public virtual void Update() { }
}
