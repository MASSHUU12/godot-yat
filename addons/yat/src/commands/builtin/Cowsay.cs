using System.Linq;
using System.Text;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command("cowsay", "Make a cow say something.", "[b]Usage[/b]: cowsay [i]message[/i]")]
	[Arguments("message:string")]
	public sealed class Cowsay : ICommand
	{
		public YAT Yat { get; set; }

		public Cowsay(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(params string[] args)
		{
			PrintCow(args[1]);

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

		private void PrintCow(string message)
		{
			var bubble = GenerateSpeechBubble(message);
			var padding = string.Empty.PadRight(bubble.Length >> 2);
			var cow = new[]
			{
				$"{padding} \\   ^__^",
				$"{padding}  \\  (oo)\\_______",
				$"{padding}     (__)\\       )\\/\\",
				$"{padding}         ||----w |",
				$"{padding}         ||     ||"
			};

			StringBuilder sb = new();
			sb.AppendLine(string.Join('\n', bubble));
			sb.AppendLine(string.Join('\n', cow));
			Yat.Terminal.Print(sb.ToString());
		}
	}
}
