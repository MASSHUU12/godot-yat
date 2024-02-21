using System.Net.NetworkInformation;
using System.Threading;
using YAT.Attributes;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Types;

namespace YAT.Commands;

[Command(
	"ping",
	"Sends ICMP (Internet Control Message Protocol) echo request to the server.",
	"[b]Usage[/b]: ping [i]target[/i] [i]options[/i]"
)]
[Threaded]
[Argument("hostname", "string", "The host to trace the route to.")]
[Option("-t", "int(1:32767)", "The maximum time to wait for each reply, in milliseconds.", 10000f)]
[Option("-ttl", "int(1:255)", "The maximum number of hops to search for the target.", 30f)]
[Option("-b", "int(1:32767)", "The size of the buffer to send with the request.", 32f)]
[Option("-f", "bool", "Specifies that the packet can be fragmented.", false)]
[Option("-delay", "int(1:10)", "The delay between pings in seconds.", 1f)]
[Option("-limit", "int(1:255)", "The maximum number of pings to send.", 0f)]
public sealed class Ping : ICommand
{
	public CommandResult Execute(CommandData data)
	{
		string hostname = (string)data.Arguments["hostname"];
		var maxPings = (int)(float)data.Options["-limit"];
		var options = new NetworkingOptions
		{
			Timeout = (ushort)(float)data.Options["-t"],
			TTL = (ushort)(float)data.Options["-ttl"],
			BufferSize = (ushort)(float)data.Options["-b"],
			DontFragment = !(bool)data.Options["-f"],
			Delay = (ushort)((float)data.Options["-delay"] * 1000),
		};

		uint pings = 0;

		data.Terminal.Output.Print($"Pinging {hostname}...");

		while ((maxPings == 0 || pings < maxPings) && !data.CancellationToken.IsCancellationRequested)
		{
			var reply = Networking.Ping(hostname, options);

			if (reply is null)
			{
				data.Terminal.Output.Print("Failed to ping the host.");
				break;
			}

			if (reply.Status == IPStatus.Success) data.Terminal.Print(string.Format(
				"Reply from {0}: bytes={1} time={2}ms TTL={3}",
				reply.Address,
				reply.Buffer.Length,
				reply.RoundtripTime,
				reply.Options.Ttl
			));
			else data.Terminal.Output.Print($"Request timed out.");

			if (maxPings != 0) pings++;

			Thread.Sleep(options.Delay);
		}

		return ICommand.Success();
	}
}
