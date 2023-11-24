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

		private static string[] GenerateBubble(string message)
		{
			var lines = message.Split('\n');
			var longestLine = lines.Max(line => line.Length);

			var bubbleWidth = longestLine + 4;
			var bubble = new string[lines.Length + 2];

			bubble[0] = $" {new string('_', bubbleWidth)}";
			bubble[1] = $"/ {message.PadRight(longestLine)} \\";
			bubble[2] = $"\\ {new string('-', bubbleWidth)} /";

			return bubble;
		}

		private void PrintCow(string message)
		{
			var bubble = GenerateBubble(message);
			var padding = string.Empty.PadRight(bubble[0].Length >> 1);
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
