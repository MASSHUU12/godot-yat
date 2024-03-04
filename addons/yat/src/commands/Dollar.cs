using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("$", "Executes a method on the selected node.")]
[Argument("method", "string", "The method to execute.")]
public sealed class Dollar : ICommand
{
	public CommandResult Execute(CommandData data)
	{
		return ICommand.Success(string.Join(" ", data.RawData[1..]));
	}
}
