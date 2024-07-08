using Godot;

namespace Confirma.Helpers;

public static class Colors
{
    public static readonly string Success = "#8eef97";
    public static readonly string Warning = "#ffdd65";
    public static readonly string Error = "#ff786b";

    // Note: GD.PrintRich does not support hex color codes
    // this is why we have to use different methods for terminal and Godot
    public static string ColorText(string text, string color)
    {
        return Log.IsHeadless
            ? ToTerminal(text, new Color(color))
            : ToGodot(text, new Color(color));
    }

    public static string ColorText(string text, Color color)
    {
        return Log.IsHeadless ? ToTerminal(text, color) : ToGodot(text, color);
    }

    public static string ToTerminal(string text, Color color)
    {
        return $"\x1b[38;2;{color.R * 0xFF};{color.G * 0xFF};{color.B * 0xFF}m{text}{TerminalReset()}";
    }

    public static string TerminalReset()
    {
        return "\x1b[0m";
    }

    public static string ToGodot(string text, Color color)
    {
        return $"[color=#{color.ToHtml()}]{text}[/color]";
    }
}
