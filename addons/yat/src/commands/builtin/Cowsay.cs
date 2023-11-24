using System.Collections.Generic;
using System.Linq;
using System.Text;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command("cowsay", "Make a cow say something.", "[b]Usage[/b]: cowsay [i]message[/i]")]
	[Arguments("message:string")]
	[Options("-b", "-d", "-g", "-p", "-s", "-t", "-w", "-y")]
	public sealed class Cowsay : ICommand
	{
		public YAT Yat { get; set; }

		public Cowsay(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(Dictionary<string, object> cArgs, params string[] args)
		{
			char eye = 'o';
			char tongue = ' ';

			var eyes = new Dictionary<string, char>
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

			var tongues = new Dictionary<string, char>
			{
				{ "-d", 'U' },
				{ "-s", 'U' },
			};

			foreach (var (key, value) in eyes)
			{
				if (!(bool)cArgs[key]) continue;

				eye = value;
				if (tongues.ContainsKey(key)) tongue = tongues[key];

				break;
			}

			PrintCow(args[1], eye, tongue);

			return CommandResult.Success;
		}

		private static string GenerateSpeechBubble(string text)
		{
			string[] lines = text.Split('\n');
			int maxLineLength = lines.Max(line => line.Length);
			int bubbleWidth = maxLineLength + 4;

			string topLine = "  " + new string('_', bubbleWidth + 2);
			string bottomLine = "  " + new string('-', (int)(bubbleWidth * 1.5));

			StringBuilder middleLines = new();
			foreach (string line in lines)
			{
				int padding = maxLineLength - line.Length;
				string paddedLine = "< " + line + new string(' ', padding) + " >\n";
				middleLines.Append(paddedLine);
			}

			return topLine + '\n' + middleLines.ToString() + bottomLine;
		}

		private void PrintCow(string message, char eye, char tongue)
		{
			var eyes = $"{eye}{eye}";
			var bubble = GenerateSpeechBubble(message);
			var padding = string.Empty.PadRight(bubble.Length >> 2);
			var cow = new[]
			{
				$"{padding} \\   ^__^",
				$"{padding}  \\  ({eyes})\\_______",
				$"{padding}     (__)\\       )\\/\\",
				$"{padding}       {tongue} ||----w |",
				$"{padding}         ||     ||"
			};

			StringBuilder sb = new();
			sb.AppendLine(string.Join('\n', bubble));
			sb.AppendLine(string.Join('\n', cow));
			Yat.Terminal.Print(sb.ToString());
		}
	}
}
