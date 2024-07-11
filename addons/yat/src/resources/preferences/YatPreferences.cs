using Godot;
using YAT.Attributes;
using YAT.Enums;

namespace YAT.Resources;

public partial class YatPreferences : Resource
{
    #region Terminal
    [ExportGroup("Terminal")]
    [Export] public string Prompt { get; set; } = ">";
    [Export] public bool ShowPrompt { get; set; } = true;
    [Export(PropertyHint.Range, "1,255,1")]
    public uint HistoryLimit { get; set; } = 15;
    // [Export] public bool AutoFocus { get; set; } = true;
    [Export] public bool AutoScroll { get; set; } = true;
    // [Export] public bool AutoComplete { get; set; } = true;
    // [Export] public bool AutoCompleteOnTab { get; set; } = true;
    // [Export] public bool AutoCompleteOnEnter { get; set; } = true;
    [Export(PropertyHint.Range, "1, 64, 0")] public ushort FontSize { get; set; } = 16;
    [Export] public int DefaultWidth { get; set; } = 728;
    [Export] public int DefaultHeight { get; set; } = 384;
    #endregion Terminal

    #region Window
    [ExportGroup("Window")]
    [Export(PropertyHint.Range, "1, 64, 0")]
    public ushort BaseFontSize { get; set; } = 16;
    [Ignore]
    [Export]
    public Font BaseFont { get; set; } = ThemeDB.FallbackFont;
    #endregion Window

    #region Colors
    [ExportGroup("Colors")]
    [Export] public Color InputColor { get; set; } = new("dfdfdf99"); // #dfdfdf99
    [Ignore]
    [Export] public Color BackgroundColor { get; set; } = new("1d2229"); // #192229
    [Export] public Color OutputColor { get; set; } = new("dfdfdf80"); // #dfdfdf80
    [Export] public Color ErrorColor { get; set; } = new("ff786b"); // #ff7866
    [Export] public Color WarningColor { get; set; } = new("ffdd65"); // #ffdd65
    [Export] public Color SuccessColor { get; set; } = new("a5ff8a"); // #a5ff8a
    #endregion Colors
}
