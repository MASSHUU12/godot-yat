using System.Text;
using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;

namespace YAT.Commands;

[Command("cat", "Prints content of a file.")]
[Argument("file", "string", "The file to print.")]
[Option("-l", "int(1:99)", "Limits the number of lines to print.", -1)]
[Option(
    "-e", "bool",
    "Embeds the file content into the terminal instead of printing it to the FullWindowDisplay."
)]
[Option(
    "-s", "bool", "Silently output file content to the command output without printing."
)]
public sealed class Cat : ICommand
{
#nullable disable
    private YAT _yat;
    private BaseTerminal _terminal;
    private RichTextLabel _display;
#nullable restore

    public CommandResult Execute(CommandData data)
    {
        string fileName = (string)data.Arguments["file"];
        int lineLimit = (int)data.Options["-l"];
        bool embed = (bool)data.Options["-e"];
        bool silent = (bool)data.Options["-s"];

        if (!FileAccess.FileExists(fileName))
        {
            return ICommand.InvalidArguments($"File '{fileName}' does not exist.");
        }

        _yat = data.Yat;
        _terminal = data.Terminal;
        _display = data.Terminal.FullWindowDisplay.MainDisplay;

        using FileAccess file = FileAccess.Open(fileName, FileAccess.ModeFlags.Read);

        (StringBuilder output, int lineCount) = GenerateContent(file, lineLimit);
        if (!silent)
        {
            DisplayContent(
                output,
                embed,
                lineLimit > 0 && lineCount > lineLimit,
                lineLimit
            );
        }

        return ICommand.Success([output.ToString(), $"{lineCount}"]);
    }

    private static (StringBuilder, int) GenerateContent(
        FileAccess file,
        int lineLimit
    )
    {
        int lineCount;
        StringBuilder output = new();

        for (
            lineCount = 1;
            !file.EofReached() && (lineLimit <= 0 || lineCount <= lineLimit);
            ++lineCount
        )
        {
            _ = output.AppendLine(file.GetLine());
        }

        return (output, lineCount);
    }

    private void DisplayContent(
        StringBuilder content,
        bool embed,
        bool limitReached,
        int limit
    )
    {
        if (embed)
        {
            _terminal.Print(content);

            if (!limitReached)
            {
                return;
            }

            _terminal.Print($"Line limit of {limit} reached.", EPrintType.Warning);
            return;
        }

        _terminal.FullWindowDisplay.Open(string.Empty);
        _display.AppendText(content.ToString());

        if (!limitReached)
        {
            return;
        }

        _display.PushColor(_yat.PreferencesManager.Preferences.WarningColor);
        _display.AppendText($"Line limit of {limit} reached.");
        _display.PopAll();
    }
}
