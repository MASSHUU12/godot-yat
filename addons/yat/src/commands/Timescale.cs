using Godot;
using YAT.Attributes;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("timescale", "Sets the timescale.")]
[Option("-set", "float", "Sets the timescale to the specified number.", 1.0f)]
public sealed class Timescale : ICommand
{
    public CommandResult Execute(CommandData data)
    {
        Engine.TimeScale = (float)data.Options["-set"];

        return ICommand.Success(
            [Engine.TimeScale.ToStringInvariant()]
        );
    }
}
