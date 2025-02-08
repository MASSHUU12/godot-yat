using System.Collections.Generic;
using Confirma.Helpers;

namespace Confirma.Classes.HelpElements;

public class CodeElement : FileElement
{
    public List<string> Lines { get; set; } = [];

    public override string GetText()
    {
        SetFormat();

        foreach (string line in Lines)
        {
            Text += $"{TextFormatHelper.Fill(line)}\n";
        }
        Text = Text.TrimEnd('\n');

        return base.GetText();
    }

    public void SetFormat()
    {
        FormatOverride = ["i"];
        BgColor = "#0d1117";
        Color = "#c9d1d9";
    }
}
