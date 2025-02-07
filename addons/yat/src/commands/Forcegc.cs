using System;
using YAT.Attributes;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("forcegc", aliases: new[] { "fgc" })]
[Description("Forces the garbage collector to run.")]
[Option("-g", "int(0:)", "The number of the oldest generation to be garbage collected.", 0)]
[Option(
    "-m",
    "int(0:3)",
    "A value that specifies whether the GC is forced - 1 (default - 0), optimized - 2 or aggressive - 3.",
    0
)]
[Option("-b", "bool", "A value that specifies whether the GC is blocking.")]
[Option("-c", "bool", "A value that specifies whether the GC is forced to compact the heap.")]
public sealed class Forcegc : ICommand
{
    public CommandResult Execute(CommandData data)
    {
        var gen = (int)data.Options["-g"];
        var mode = (GCCollectionMode)data.Options["-m"];
        var block = (bool)data.Options["-b"];
        var compact = (bool)data.Options["-c"];

        if (gen > GC.MaxGeneration)
        {
            return ICommand.Failure(
                $"The generation number must be between 0 and {GC.MaxGeneration}."
            );
        }

        GC.Collect(
            mode == GCCollectionMode.Aggressive
                ? GC.MaxGeneration
                : gen,
            mode,
            mode == GCCollectionMode.Aggressive || block,
            mode == GCCollectionMode.Aggressive || compact
        );
        return ICommand.Success();
    }
}
