using Godot;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command(
	"timescale",
	"Sets the timescale.",
	"[b]Usage[/b]: timescale"
)]
[Option("-set", "float", "Sets the timescale to the specified number.", 1.0f)]
public sealed class Timescale : ICommand
{
	public ECommandResult Execute(CommandData data)
	{
		var scale = (float)data.Options["-set"];

		Engine.TimeScale = scale;

		return ECommandResult.Success;
	}
}
