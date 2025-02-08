using Confirma.Deserialization.Json;
using Confirma.Helpers;
using Confirma.Classes.HelpElements;
using System.Threading.Tasks;

namespace Confirma.Classes;

public static class Help
{
    public static async Task<bool> ShowHelpPage(string pageName)
    {
        var file = await Json.LoadFromFile<HelpFile>($"{Plugin.GetPluginLocation()}docs/help_pages/{pageName}.json");

        if (file is null)
        {
            Log.PrintError($"Page: `{pageName}`, not found or failed to load.\n");
            return false;
        }

        foreach(FileElement element in file.Data)
        {
            switch(element)
            {
                case TextElement ele:
                    Log.Print(ele.GetText());
                    break;
                case HeaderElement ele:
                    Log.PrintLine($"\n{ele.GetText()}");
                    break;
                case CodeElement ele:
                    Log.Print(ele.GetText());
                    break;
                case LinkElement ele:
                    Log.Print(ele.GetText());
                    break;
            }
        }
        return true;
    }
}
