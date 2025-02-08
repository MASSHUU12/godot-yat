using System;
using Confirma.Enums;
using Confirma.Types;
using Godot;
using static System.AttributeTargets;

namespace Confirma.Attributes;

[AttributeUsage(Class | Method, AllowMultiple = false)]
public class IgnoreAttribute(
    EIgnoreMode mode = EIgnoreMode.Always,
    string? reason = null,
    bool hideFromResults = false,
    string category = ""
    ) : Attribute
{
    public EIgnoreMode Mode { get; init; } = mode;
    public string? Reason { get; init; } = string.IsNullOrEmpty(reason) ? null : reason;
    public bool HideFromResults { get; init; } = hideFromResults;
    public string Category { get; init; } = category;

    public bool IsIgnored(in RunTarget target)
    {
        return Mode switch
        {
            EIgnoreMode.Always => true,
            EIgnoreMode.InEditor => Engine.IsEditorHint(),
            EIgnoreMode.WhenNotRunningCategory => (
                (target.Target == ERunTargetType.Category
                && target.Name != Category)
                || string.IsNullOrEmpty(target.Name)
            ),
            EIgnoreMode.InHeadless => DisplayServer.GetName() == "headless",
            _ => false
        };
    }
}
