using System.Net.NetworkInformation;
using System.Threading;
using YAT.Attributes;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Types;
using static YAT.Helpers.Networking;

namespace YAT.Commands;

[Command(
    "ping",
    "Sends ICMP (Internet Control Message Protocol) echo request to the server."
)]
[Threaded]
[Argument("hostname", "string", "The host to trace the route to.")]
[Option("-t", "int(1:32767)", "The maximum time to wait for each reply, in milliseconds.", 10000)]
[Option("-ttl", "int(1:255)", "The maximum number of hops to search for the target.", 30)]
[Option("-b", "int(1:32767)", "The size of the buffer to send with the request.", 32)]
[Option("-f", "bool", "Specifies that the packet can be fragmented.")]
[Option("-delay", "int(1:10)", "The delay between pings in seconds.", 1)]
[Option("-limit", "int(1:255)", "The maximum number of pings to send.", 0)]
public sealed class Ping : ICommand
{
    public CommandResult Execute(CommandData data)
    {
        var hostname = (string)data.Arguments["hostname"];
        var maxPings = (int)data.Options["-limit"];
        var options = new NetworkingOptions
        {
            Timeout = (ushort)(int)data.Options["-t"],
            TTL = (ushort)(int)data.Options["-ttl"],
            BufferSize = (ushort)(int)data.Options["-b"],
            DontFragment = !(bool)data.Options["-f"],
            Delay = (ushort)((int)data.Options["-delay"] * 1000),
        };

        uint pings = 0;

        data.Terminal.Output.Print($"Pinging {hostname}...");

        while ((maxPings == 0 || pings < maxPings) && !data.CancellationToken.IsCancellationRequested)
        {
            EPingStatus status = Networking.Ping(hostname, out var reply, options);

            if (reply is null)
            {
                if (status == Networking.EPingStatus.Unsupported)
                {
                    data.Terminal.Output.Error("The current platform does not support ICMP or access is denied.");
                    break;
                }

                data.Terminal.Output.Error("Failed to ping the host.");
                break;
            }

            if (reply.Status == IPStatus.Success)
            {
                data.Terminal.Print(string.Format(
                    "Reply from {0}: bytes={1} time={2}ms TTL={3}",
                    reply.Address,
                    reply.Buffer.Length,
                    reply.RoundtripTime,
                    reply.Options?.Ttl
                ));
            }
            else
            {
                data.Terminal.Output.Error("Request timed out.");
            }

            if (maxPings != 0)
            {
                pings++;
            }

            Thread.Sleep(options.Delay);
        }

        return ICommand.Success();
    }
}
