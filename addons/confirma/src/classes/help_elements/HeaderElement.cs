namespace Confirma.Classes.HelpElements;

public class HeaderElement : FileElement
{
    public int Level { get; set; }

    public override string GetText()
    {
        SetFormat(Level);

        return $"{base.GetText()}";
        //return $"{new string(' ',(Level-1)*2)}{base.GetText()}\n";
    }

    public void SetFormat(int level)
    {
        switch (level)
        {
            case 1:
                FormatOverride = ["b", "i", "f"];
                BgColor = "#edede9";
                Color = "#000";
                break;
            case 2:
                FormatOverride = ["i", "f"];
                Color = "#000";
                BgColor = "#a8a8a3";
                break;
            case 3:
                FormatOverride = ["b", "u"];
                Color = "#fff";
                break;
            case 4:
                FormatOverride = ["i"];
                Color = "#fff";
                break;
        }
    }
}
