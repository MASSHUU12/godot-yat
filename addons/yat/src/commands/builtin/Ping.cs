using System.Collections.Generic;
using System.Threading;
using YAT.Attributes;
using YAT.Enums;
using YAT.Interfaces;

namespace YAT.Commands
{
	[Command(
		"ping",
		"Sends ICMP (Internet Control Message Protocol) echo request to the server.",
		"[b]Usage[/b]: ping"
	)]
	[Threaded]
	[Arguments("target:string")]
	[Options("-timeout=int(120:1200)")]
	public sealed class Ping : ICommand
	{
		public YAT Yat { get; set; }

		public Ping(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(Dictionary<string, object> cArgs, CancellationToken ct, params string[] args)
		{
			string target = cArgs["target"] as string;
			int timeout = cArgs.TryGetValue("timeout", out object timeoutObj) ? (int)timeoutObj : 120;

			string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
			byte[] buffer = System.Text.Encoding.ASCII.GetBytes(data);

			while (!ct.IsCancellationRequested)
			{
				System.Net.NetworkInformation.Ping ping = new();
				System.Net.NetworkInformation.PingOptions options = new()
				{
					DontFragment = true
				};
				System.Net.NetworkInformation.PingReply reply = ping.Send(target, timeout, buffer, options);

				if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
				{
					Yat.Terminal.Print($"Reply from {reply.Address}: bytes={reply.Buffer.Length} time={reply.RoundtripTime}ms TTL={reply.Options.Ttl}");
				}
				else Yat.Terminal.Print($"Request timed out.");

				Thread.Sleep(1000);
			}

			return CommandResult.Success;
		}
	}
}
