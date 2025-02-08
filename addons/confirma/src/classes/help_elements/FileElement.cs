using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Confirma.Deserialization.Json;
using Confirma.Enums;
using Confirma.Helpers;

namespace Confirma.Classes.HelpElements;

[JsonConverter(typeof(FileElementConverter))]
public abstract class FileElement
{
    public string Text { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string BgColor { get; set; } = string.Empty;
    public List<string> FormatOverride { get; set; } = [];

    /// <summary>
    /// I didn't have any idea how to name this method not to be misleading
    /// </summary>
    public virtual string Format(string text)
    {
        if (FormatOverride is null)
        {
            return text;
        }

        foreach (string? a in FormatOverride)
        {
            if (a is null or "fill" or "f" or "center" or "c")
            {
                continue;
            }

            text = TextFormatHelper.FormatText(text, a, false);
        }
        return text;
    }

    public virtual string GetText()
    {
        return Log.IsHeadless
        ? GetTextToTerminal()
        : GetTextToGodot();
    }

    public virtual string GetTextToGodot()
    {
        string? color, bgColor = null;
        string text = Format(Text);

        if (FormatOverride.Any(static (string a) => a is "center" or "c"))
        {
            text = TextFormatHelper.FormatText(text, EFormatType.Center);
        }

        if (FormatOverride.Any(static (string a) => a is "fill" or "f"))
        {
            text = TextFormatHelper.FormatText(text, EFormatType.Fill);
        }

        color = Color?.Length == 0 ? null : Color;
        bgColor = BgColor?.Length == 0 ? null : BgColor;

        return Colors.Color(text, color, bgColor);
    }

    public virtual string GetTextToTerminal()
    {
        string? color, bgColor = null;
        string text = Text;

        if (FormatOverride.Any(static (string a) => a is "center" or "c"))
        {
            text = TextFormatHelper.FormatText(text, EFormatType.Center);
        }

        if (FormatOverride.Any(static (string a) => a is "fill" or "f"))
        {
            text = TextFormatHelper.FormatText(text, EFormatType.Fill);
        }

        color = Color?.Length == 0 ? null : Color;
        bgColor = BgColor?.Length == 0 ? null : BgColor;

        text = Colors.Color(text, color, bgColor, false);

        return Format(text) + Colors.TerminalReset;
    }
}
