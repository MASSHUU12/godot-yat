using System.Globalization;
using System.Net.NetworkInformation;
using System.Threading;
using YAT.Attributes;
using YAT.Helpers;
using YAT.Interfaces;
using YAT.Net;
using YAT.Types;
using static YAT.Net.Networking;

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
        string hostname = (string)data.Arguments["hostname"];
        int maxPings = (int)data.Options["-limit"];
        NetworkingOptions options = new()
        {
            Timeout = (ushort)(int)data.Options["-t"],
            TTL = (ushort)(int)data.Options["-ttl"],
            BufferSize = (ushort)(int)data.Options["-b"],
            DontFragment = !(bool)data.Options["-f"],
            Delay = (ushort)((int)data.Options["-delay"] * 1000),
        };

        uint pings = 0;

        data.Terminal.Output.Print($"Pinging {hostname}...");

        while (
            (maxPings == 0 || pings < maxPings)
            && !data.CancellationToken.IsCancellationRequested
        )
        {
            EConnectionStatus status = Ping(
                hostname,
                out PingReply? reply,
                options
            );

            if (reply is null)
            {
                return ICommand.Failure(
                    status == EConnectionStatus.Unsupported
                        ? "The current platform does not support ICMP or access is denied."
                        : "Failed to ping the host."
                );
            }

            if (reply.Status == IPStatus.Success)
            {
                data.Terminal.Print(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Reply from {0}: bytes={1} time={2}ms TTL={3}",
                        reply.Address,
                        reply.Buffer.Length,
                        reply.RoundtripTime,
                        reply.Options?.Ttl
                    )
                );
            }
            else
            {
                return ICommand.Failure("Request timed out.");
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
