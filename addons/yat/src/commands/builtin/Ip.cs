using System.Text;
using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Scenes.BaseTerminal;

namespace YAT.Commands
{
	[Command("ip", "Displays your private IP addresses.", "[b]Usage[/b]: ip")]
	[Argument("action", "[addr]", "The action to perform.")]
	public sealed class Ip : ICommand
	{
		private BaseTerminal _terminal;

		public CommandResult Execute(CommandArguments args)
		{
			string action = args.ConvertedArgs["action"] as string;

			_terminal = args.Terminal;

			if (action == "addr") PrintLocalInterfaces();

			return CommandResult.Success;
		}

		private void PrintLocalInterfaces()
		{
			StringBuilder sb = new();
			var interfaces = IP.GetLocalInterfaces();

			foreach (var iface in interfaces)
			{
				sb.AppendLine($"[b]{iface["index"]}[/b]: {iface["friendly"]} ({iface["name"]})");
				sb.AppendLine("Addresses:");
				foreach (var addr in iface["addresses"].AsStringArray())
				{
					sb.AppendLine($"\t{addr}");
				}

				sb.AppendLine();
			}

			_terminal.Print(sb.ToString());
		}
	}
}
