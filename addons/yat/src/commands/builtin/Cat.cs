using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Types;
using static YAT.Scenes.BaseTerminal.BaseTerminal;

namespace YAT.Commands;

[Command("cat", "Prints content of a file.", "[b]Usage[/b]: cat [i]file[/i]")]
[Argument("file", "string", "The file to print.")]
[Option("-l", "int(1:99)", "Limits the number of lines to print.", -1)]
public sealed class Cat : ICommand
{
	public CommandResult Execute(CommandData data)
	{
		string fileName = (string)data.Arguments["file"];
		int lineLimit = (int)data.Options["-l"];

		if (!FileAccess.FileExists(fileName))
		{
			data.Terminal.Print($"File '{fileName}' does not exist.", PrintType.Error);
			return CommandResult.InvalidArguments;
		}

		using FileAccess file = FileAccess.Open(fileName, FileAccess.ModeFlags.Read);
		int lineCount = 0;

		while (!file.EofReached())
		{
			string line = file.GetLine();
			if (lineLimit > 0 && ++lineCount > lineLimit)
			{
				data.Terminal.Print(
					$"Line limit of {lineLimit} reached.",
					PrintType.Warning
				);
				break;
			}

			data.Terminal.Print(line);
		}

		return CommandResult.Success;
	}
}
