using System.Collections.Generic;
using System.Threading;
using Godot;
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
	[Options(
		"-timeout=int(120:1200)",
		"-delay=int(1:10)"
	)]
	public sealed class Ping : ICommand
	{
		public YAT Yat { get; set; }

		public Ping(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(Dictionary<string, object> cArgs, CancellationToken ct, params string[] args)
		{
			string target = cArgs["target"] as string;
			int timeout = cArgs["-timeout"] as int? ?? 120;
			int delay = cArgs["-delay"] as int? ?? 1;
			delay *= 1000;

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

				Thread.Sleep(delay);
			}

			return CommandResult.Success;
		}
	}
}
