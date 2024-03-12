using System.Text;
using Godot;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("cat", "Prints content of a file.", "[b]Usage[/b]: cat [i]file[/i]")]
[Argument("file", "string", "The file to print.")]
[Option("-l", "int(1:99)", "Limits the number of lines to print.", -1)]
public sealed class Cat : ICommand
{
	public CommandResult Execute(CommandData data)
	{
		var fileName = (string)data.Arguments["file"];
		int lineLimit = (int)data.Options["-l"];
		var display = data.Terminal.FullWindowDisplay.MainDisplay;

		if (!FileAccess.FileExists(fileName))
		{
			return ICommand.InvalidArguments($"File '{fileName}' does not exist.");
		}

		using FileAccess file = FileAccess.Open(fileName, FileAccess.ModeFlags.Read);

		int lineCount;
		StringBuilder output = new();

		for (lineCount = 1; !file.EofReached() && (lineLimit <= 0 || lineCount <= lineLimit); ++lineCount)
		{
			output.AppendLine(file.GetLine());
		}

		data.Terminal.FullWindowDisplay.Open(string.Empty);
		display.AppendText(output.ToString());

		if (lineLimit > 0 && lineCount > lineLimit)
		{
			var color = data.Yat.PreferencesManager.Preferences.WarningColor;
			display.PushColor(color);
			display.AppendText($"Line limit of {lineLimit} reached.");
			display.PopAll();
		}

		return ICommand.Success();
	}
}
