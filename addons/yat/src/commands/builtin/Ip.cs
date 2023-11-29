using System.Collections.Generic;
using System.Text;
using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command("ip", "Displays your private IP addresses.", "[b]Usage[/b]: ip")]
	[Arguments("action:[addr]")]
	public sealed class Ip : ICommand
	{
		public YAT Yat { get; set; }

		public Ip(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(Dictionary<string, object> cArgs, params string[] args)
		{
			string action = cArgs["action"] as string;

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

			Yat.Terminal.Print(sb.ToString());
		}
	}
}
