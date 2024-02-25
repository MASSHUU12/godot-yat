using System.Linq;
using System.Text;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Scenes;
using YAT.Types;

namespace YAT.Commands;

[Command("ds", "Displays items in the debug screen.", "[b]Usage[/b]: ds")]
[Option("-h", "bool", "Displays this help message.")]
[Option("-i", "string...", "Items to display.", new string[] { })]
public sealed class Ds : ICommand
{
	public CommandResult Execute(CommandData data)
	{
		var h = (bool)data.Options["-h"];
		var i = ((object[])data.Options["-i"]).Cast<string>().ToArray();

		if (h)
		{
			Help(data.Terminal);
			return ICommand.Success();
		}

		if (i.Contains("all")) data.Yat.DebugScreen.RunAll();
		else data.Yat.DebugScreen.RunSelected(i);

		return ICommand.Success();
	}

	private static void Help(BaseTerminal terminal)
	{
		var message = new StringBuilder();

		message.AppendLine("Registered items:");

		foreach (IDebugScreenItem item in DebugScreen.registeredItems.SelectMany(x => x))
		{
			message.AppendLine($"- {item.Title} ({item.GetType().Name})");
		}

		terminal.Print(message);
	}
}
