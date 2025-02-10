using Godot;
using YAT.Attributes;
using YAT.Resources;

using static Godot.Performance;

namespace YAT.Debug;

[Title("FPS")]
public partial class FpsItem : DebugScreenItem
{
#nullable disable
    private RichTextLabel _label, _times;
#nullable restore

    public override void _Ready()
    {
        base._Ready();

        _label = CreateLabel();
        VContainer.AddChild(_label);
        _times = CreateLabel();
        VContainer.AddChild(_times);
    }

    public override void Update()
    {
        float fps = (float)GetMonitor(Monitor.TimeFps);
        float process = (float)GetMonitor(Monitor.TimeProcess) * 1000;
        float physics = (float)GetMonitor(Monitor.TimePhysicsProcess) * 1000;
        YatPreferences pref = Yat.PreferencesManager.Preferences;

        _label.Clear();
        _label.PushFontSize(24);
        _label.PushColor(fps < 30 ? pref.ErrorColor : pref.SuccessColor);
        _label.AppendText($"{fps} FPS");
        _label.PopAll();

        _times.Clear();
        _times.PushFontSize(12);
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
        _times.Pop();
    }
}
