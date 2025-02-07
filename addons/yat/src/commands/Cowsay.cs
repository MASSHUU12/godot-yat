using System.Collections.Generic;
using System.Linq;
using System.Text;
using Godot;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;

namespace YAT.Commands;

[Command("cowsay", "Make a cow say something.")]
[Argument("message", "string", "The message to make the cow say.")]
[Option("-b", "bool", "Borg")]
[Option("-d", "bool", "Dead")]
[Option("-g", "bool", "Greedy")]
[Option("-p", "bool", "Paranoid")]
[Option("-s", "bool", "Stoned")]
[Option("-t", "bool", "Tired")]
[Option("-w", "bool", "Wired")]
[Option("-y", "bool", "Youthful")]
public sealed class Cowsay : ICommand
{
#nullable disable
    private BaseTerminal _terminal;
#nullable restore

    public CommandResult Execute(CommandData data)
    {
        _terminal = data.Terminal;

        (char eye, char tongue) = GetCowFace(data.Options);
        PrintCow(data.RawData[1], eye, tongue);

        return ICommand.Success();
    }

    private static (char eye, char tongue) GetCowFace(
        Dictionary<StringName, object> options
    )
    {
        const char DefaultEye = 'o';
        const char DefaultTongue = ' ';
        Dictionary<string, char> eyes = new()
        {
            { "-b", '=' }, // Borg
            { "-d", 'x' }, // Dead
            { "-g", '$' }, // Greedy
            { "-p", '@' }, // Paranoid
            { "-s", '*' }, // Stoned
            { "-t", '-' }, // Tired
            { "-w", 'O' }, // Wired
            { "-y", '.' }  // Youthful
        };

        Dictionary<string, char> tongues = new()
        {
            { "-d", 'U' },
            { "-s", 'U' },
        };

        foreach ((string key, char eye) in eyes)
        {
            if (options.TryGetValue(key, out object? optionValue) && (bool)optionValue)
            {
                char tongue = tongues.TryGetValue(key, out char t)
                    ? t
                    : DefaultTongue;
                return (eye, tongue);
            }
        }

        return (DefaultEye, DefaultTongue);
    }

    private static string GenerateSpeechBubble(string text)
    {
        string[] lines = text.Split('\n');
        int maxLineLength = lines.Max(static line => line.Length);
        int bubbleWidth = maxLineLength + 2;

        string topLine = "  " + new string('_', bubbleWidth);
        string bottomLine = "  " + new string('-', bubbleWidth);

        StringBuilder middleLines = new();
        foreach (string line in lines)
        {
            int padding = maxLineLength - line.Length;
            string paddedLine = "| " + line + new string(' ', padding) + " |\n";
            _ = middleLines.Append(paddedLine);
        }

        return topLine + '\n' + middleLines + bottomLine;
    }

    private void PrintCow(string message, char eye, char tongue)
    {
        string eyes = $"{eye}{eye}";
        string bubble = GenerateSpeechBubble(message);
        string padding = string.Empty.PadRight(bubble.Length >> 2);
        string[] cow =
        [
            $"{padding} \\   ^__^",
            $"{padding}  \\  ({eyes})\\_______",
            $"{padding}     (__)\\       )\\/\\",
            $"{padding}       {tongue} ||----w |",
            $"{padding}         ||     ||"
        ];

        StringBuilder sb = new();
        _ = sb.AppendLine(bubble);
        _ = sb.AppendJoin('\n', cow);
        _terminal.Print(sb.ToString());
    }
}
