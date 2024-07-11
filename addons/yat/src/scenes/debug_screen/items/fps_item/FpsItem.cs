using Godot;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Resources;

namespace YAT.Scenes;

[Title("FPS")]
public partial class FpsItem : PanelContainer, IDebugScreenItem
{
#nullable disable
    private YAT _yat;
    private RichTextLabel
        _label,
        _times;
#nullable restore

    public override void _Ready()
    {
        _yat = GetNode<YAT>("/root/YAT");
        _label = GetNode<RichTextLabel>("./VBoxContainer/RichTextLabel");
        _times = GetNode<RichTextLabel>("./VBoxContainer/Times");
    }

    public void Update()
    {
        double fps = Performance.GetMonitor(Performance.Monitor.TimeFps);
        double process = Performance.GetMonitor(Performance.Monitor.TimeProcess) * 1000;
        double physics = Performance.GetMonitor(Performance.Monitor.TimePhysicsProcess) * 1000;
        YatPreferences pref = _yat.PreferencesManager.Preferences;

        _label.Clear();
        _label.PushColor(fps < 30 ? pref.ErrorColor : pref.SuccessColor);
        _label.AppendText($"{fps} FPS");
        _label.Pop();

        _times.Clear();
        _times.AppendText("Process: ");
        if (fps < 30)
        {
            _times.PushColor(pref.ErrorColor);
        }

        _times.AppendText($"{process:0.00} ms\n");
        if (fps < 30)
        {
            _times.Pop();
        }

        _times.AppendText("Physics: ");
        if (fps < 30)
        {
            _times.PushColor(pref.ErrorColor);
        }

        _times.AppendText($"{physics:0.000} ms");
        if (fps < 30)
        {
            _times.Pop();
        }
    }
}
