using YAT.Attributes;
using YAT.Enums;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Scenes.Overlay.Components.Terminal;

namespace YAT.Commands
{
	[Command("quickcommands", "Manages Quick Commands.", "[b]Usage[/b]: quickcommands [i]action[/i] [i]name[/i] [i]command[/i]", "qc")]
	[Arguments("action:[add, remove, list]", "name:string", "command:string")]
	public sealed class QuickCommands : ICommand
	{
		public YAT Yat { get; set; }

		public QuickCommands(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(System.Collections.Generic.Dictionary<string, object> cArgs, params string[] args)
		{
			string action = (string)cArgs["action"];
			string name = (string)cArgs["name"];
			string command = (string)cArgs["command"];

			bool status = true;
			string message;

			switch (action)
			{
				case "add":
					status = Yat.Terminal.Context.QuickCommands.AddQuickCommand(name, command);
					if (status) message = $"[b]Added quick command '{name}'[/b]";
					else message = $"[b]Failed to add quick command '{name}'[/b]";

					Yat.Terminal.Print(message, status ? Terminal.PrintType.Success : Terminal.PrintType.Error);
					break;
				case "remove":
					status = Yat.Terminal.Context.QuickCommands.RemoveQuickCommand(name);
					if (status) message = $"[b]Removed quick command '{name}'[/b]";
					else message = $"[b]Failed to remove quick command '{name}'[/b]";

					Yat.Terminal.Print(message, status ? Terminal.PrintType.Success : Terminal.PrintType.Error);
					break;
				default:
					foreach (var qc in Yat.Terminal.Context.QuickCommands.QuickCommands.Commands)
					{
						Yat.Terminal.Print($"[b]{qc.Key}[/b] - {TextHelper.EscapeBBCode(qc.Value)}");
					}
					break;
			}

			return status ? CommandResult.Success : CommandResult.Failure;
		}
	}
}
