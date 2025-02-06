using Godot;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;

namespace YAT.Commands;

[Command("locale", "Manages game locale.")]
[Option("--list-countries", "bool", "Prints known country codes.")]
[Option("--list-languages", "bool", "Prints known language codes")]
[Option("--list-scripts", "bool", "Prints known script codes.")]
[Option("--list-locales", "bool", "Prints all loaded locales of the project.")]
[Option("--get-locale", "bool", "Prints the current locale of the project.")]
[Option(
    "--set-locale",
    "string",
    "Sets the locale of the project. "
    + "The locale string will be standardized to match known locales.",
    ""
)]
public sealed class Locale : ICommand
{
    public string[]? Arguments { get; set; }

    private BaseTerminal? _terminal;

    public CommandResult Execute(CommandData data)
    {
        bool listCountries = (bool)data.Options["--list-countries"];
        bool listLanguages = (bool)data.Options["--list-languages"];
        bool listScripts = (bool)data.Options["--list-scripts"];
        bool listLocales = (bool)data.Options["--list-locales"];
        bool getLocale = (bool)data.Options["--get-locale"];
        string setLocale = (string)data.Options["--set-locale"];

        _terminal = data.Terminal;

        if (listCountries)
        {
            PrintArray(TranslationServer.Singleton.GetAllCountries());
            return ICommand.Ok();
        }

        if (listLanguages)
        {
            PrintArray(TranslationServer.Singleton.GetAllLanguages());
            return ICommand.Ok();
        }

        if (listScripts)
        {
            PrintArray(TranslationServer.Singleton.GetAllScripts());
            return ICommand.Ok();
        }

        if (listLocales)
        {
            PrintArray(TranslationServer.Singleton.GetLoadedLocales());
            return ICommand.Ok();
        }

        if (getLocale)
        {
            return ICommand.Ok(TranslationServer.Singleton.GetLocale());
        }

        if (!string.IsNullOrEmpty(setLocale))
        {
            TranslationServer.Singleton.SetLocale(setLocale);
            return ICommand.Ok();
        }

        return ICommand.Ok();
    }

    private void PrintArray(string[] items)
    {
        foreach (string item in items)
        {
            _terminal?.Print(item);
        }
    }
}
