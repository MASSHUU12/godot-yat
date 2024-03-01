using System;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("forcegc", aliases: new[] { "fgc" })]
[Description("Forces the garbage collector to run.")]
[Usage("forcegc")]
public sealed class Forcegc : ICommand
{
	public CommandResult Execute(CommandData data)
	{
		GC.Collect();
		return ICommand.Success();
	}
}
