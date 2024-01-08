using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using static YAT.Scenes.BaseTerminal.BaseTerminal;

namespace YAT.Commands
{
	[Command("cat", "Prints content of a file.", "[b]Usage[/b]: cat [i]file[/i]")]
	[Argument("file", "string", "The file to print.")]
	[Option("-l", "int(1:99)", "Limits the number of lines to print.", -1)]
	public sealed class Cat : ICommand
	{
		public CommandResult Execute(CommandArguments args)
		{
			string fileName = (string)args.ConvertedArgs["file"];
			int lineLimit = (int)args.ConvertedArgs["-l"];

			if (!FileAccess.FileExists(fileName))
			{
				args.Terminal.Print($"File '{fileName}' does not exist.", PrintType.Error);
				return CommandResult.InvalidArguments;
			}

			using FileAccess file = FileAccess.Open(fileName, FileAccess.ModeFlags.Read);
			int lineCount = 0;

			while (!file.EofReached())
			{
				string line = file.GetLine();
				if (lineLimit > 0 && ++lineCount > lineLimit)
				{
					args.Terminal.Print(
						$"Line limit of {lineLimit} reached.",
						PrintType.Warning
					);
					break;
				}

				args.Terminal.Print(line);
			}

			return CommandResult.Success;
		}
	}
}
