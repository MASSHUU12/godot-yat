using System.Threading;
using YAT.Attributes;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("sleep")]
[Description(
    "Suspends the current thread for the specified number of milliseconds. "
    + "Command used for testing purposes."
)]
[Argument("ms", "int(0:", "Number of miliseconds.")]
[Threaded]
public sealed class Sleep : ICommand
{
    public CommandResult Execute(CommandData data)
    {
        Thread.Sleep((int)data.Arguments["ms"]);
        return ICommand.Ok([((int)data.Arguments["ms"]).ToStringInvariant()]);
    }
}
