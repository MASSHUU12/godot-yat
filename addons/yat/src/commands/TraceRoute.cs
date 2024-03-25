using System.Text;
using YAT.Attributes;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command("traceroute", aliases: "trace")]
[Usage("traceroute [i]hostname[/i]")]
[Description("Displays the route that packets take to reach the specified host.")]
[Argument("hostname", "string", "The host to trace the route to.")]
[Option("-t", "int(1:32767)", "The maximum time to wait for each reply, in milliseconds.", 10000)]
[Option("-ttl", "int(1:255)", "The maximum number of hops to search for the target.", 30)]
[Option("-b", "int(1:32767)", "The size of the buffer to send with the request.", 32)]
[Option("-f", "bool", "Specifies that the packet can be fragmented.")]
[Threaded]
public sealed class TraceRoute : ICommand
{
	public CommandResult Execute(CommandData data)
	{
		var hostname = (string)data.Arguments["hostname"];
		var options = new NetworkingOptions
		{
			Timeout = (ushort)(int)data.Options["-t"],
			TTL = (ushort)(int)data.Options["-ttl"],
			BufferSize = (ushort)(int)data.Options["-b"],
			DontFragment = !(bool)data.Options["-f"]
		};

		data.Terminal.Output.Print($"Tracing route to {hostname}...");

		var addresses = Networking.GetTraceRoute(hostname, options, data.CancellationToken);
		var result = new StringBuilder();

		foreach (var address in addresses) result.AppendLine(address?.ToString());

		return ICommand.Ok(result.ToString());
	}
}
