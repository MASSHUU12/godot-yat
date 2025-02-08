using Confirma.Helpers;

namespace Confirma.Classes.HelpElements;

public class LinkElement : FileElement
{
    public string Url { get; set; } = string.Empty;
    public override string GetText()
    {
        SetFormat();

        Text = TextFormatHelper.Link(Text, Url);
        return base.GetText();
    }

    public void SetFormat()
    {
        FormatOverride = ["u"];
        Color = "#2a7bde";
    }
}
