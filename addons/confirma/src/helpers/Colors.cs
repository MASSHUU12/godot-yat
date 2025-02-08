using System;
using System.Globalization;
using Godot;

namespace Confirma.Helpers;

public static class Colors
{
    public static readonly string Success = "#8eef97";
    public static readonly string Warning = "#ffdd65";
    public static readonly string Error = "#ff786b";
    public static readonly string Class = "#218a72";
    public static readonly string CSharp = "#9b4993";
    public static readonly string Gdscript = "#478cbf";
    public static readonly string TerminalReset = "\x1b[0m";

    public static string Color<T>(
        T text,
        string? TextColor = null,
        string? BgColor = null,
        bool addReset = true
    )
    where T : IConvertible
    {
        if (TextColor is null && BgColor is null)
        {
            return text?.ToString() ?? string.Empty;
        }

        string coloredText = Convert.ToString(text, CultureInfo.InvariantCulture) ?? string.Empty;

        if (TextColor is not null)
        {
            coloredText = ColorText(text, new Color(TextColor), false);
        }

        if (BgColor is not null)
        {
            coloredText = ColorBackground(coloredText, new Color(BgColor), false);
        }
        return addReset && Log.IsHeadless
            ? coloredText + TerminalReset
            : coloredText;
    }

    #region Text
    public static string ColorText<T>(T text, string color, bool addReset = true)
    where T : IConvertible
    {
        // Note: GD.PrintRich does not support hex color codes
        // this is why we have to use different methods for terminal and Godot
        return Log.IsHeadless
            ? TextToTerminal(text, new Color(color), addReset)
            : TextToGodot(text, new Color(color));
    }

    public static string ColorText<T>(T text, Color color, bool addReset = true)
    where T : IConvertible
    {
        return Log.IsHeadless
            ? TextToTerminal(text, color, addReset)
            : TextToGodot(text, color);
    }

    public static string TextToTerminal<T>(
        T text,
        Color color,
        bool addReset = true
    )
    where T : IConvertible
    {
        string reset = addReset ? TerminalReset : string.Empty;
        return $"\x1b[38;2;{color.R * 0xFF};{color.G * 0xFF};{color.B * 0xFF}m{text}{reset}";
    }

    public static string TextToGodot<T>(T text, Color color)
    where T : IConvertible
    {
        return $"[color=#{color.ToHtml()}]{text}[/color]";
    }
    #endregion Text

    #region Background
    public static string ColorBackground<T>(
        T text,
        string color,
        bool addReset = true
    )
    where T : IConvertible
    {
        return Log.IsHeadless
            ? BackgroundToTerminal(text, new Color(color), addReset)
            : BackgroundToGodot(text, new Color(color));
    }

    public static string ColorBackground<T>(
        T text,
        Color color,
        bool addReset = true
    )
    where T : IConvertible
    {
        return Log.IsHeadless
            ? BackgroundToTerminal(text, color, addReset)
            : BackgroundToGodot(text, color);
    }

    public static string BackgroundToTerminal<T>(
        T text,
        Color color,
        bool addReset = true
    )
    where T : IConvertible
    {
        string reset = addReset ? TerminalReset : string.Empty;
        return $"\x1b[48;2;{color.R * 0xFF};{color.G * 0xFF};{color.B * 0xFF}m{text}{reset}";
    }

    public static string BackgroundToGodot<T>(T text, Color color)
    where T : IConvertible
    {
        return $"[bgcolor=#{color.ToHtml()}]{text}[/bgcolor]";
    }
    #endregion Background
}
