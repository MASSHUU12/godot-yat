using System.Collections.Generic;
using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Scenes.Terminal;

namespace YAT.Commands
{
	[Command("cat", "Prints content of a file.", "[b]Usage[/b]: cat [i]file[/i]")]
	[Argument("file", "string", "The file to print.")]
	[Option("-l", "int(1:99)", "Limits the number of lines to print.", -1)]
	public sealed class Cat : ICommand
	{
		public YAT Yat { get; set; }

		public Cat(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(Dictionary<string, object> cArgs, params string[] args)
		{
			string fileName = (string)cArgs["file"];
			int lineLimit = (int)cArgs["-l"];

			if (!FileAccess.FileExists(fileName))
			{
				Yat.Terminal.Print($"File '{fileName}' does not exist.", Terminal.PrintType.Error);
				return CommandResult.InvalidArguments;
			}

			using FileAccess file = FileAccess.Open(fileName, FileAccess.ModeFlags.Read);
			int lineCount = 0;

			while (!file.EofReached())
			{
				string line = file.GetLine();
				if (lineLimit > 0 && ++lineCount > lineLimit)
				{
					Yat.Terminal.Print(
						$"Line limit of {lineLimit} reached.",
						Terminal.PrintType.Warning
					);
					break;
				}

				Yat.Terminal.Print(line);
			}

			return CommandResult.Success;
		}
	}
}
