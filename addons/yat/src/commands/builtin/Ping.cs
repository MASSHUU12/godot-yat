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
		"[b]Usage[/b]: ping [i]target[/i] [i]options[/i]"
	)]
	[Threaded]
	[Argument("target", "string", "The target to ping.")]
	[Options(
		"-timeout=int(120:1200)",
		"-delay=int(1:10)",
		"-bytes=int(16:65500)",
		"-ttl=int(1:255)",
		"-fragment"
	)]
	public sealed class Ping : ICommand
	{
		public YAT Yat { get; set; }

		public Ping(YAT Yat) => this.Yat = Yat;

		public CommandResult Execute(Dictionary<string, object> cArgs, CancellationToken ct, params string[] args)
		{
			string target = cArgs["target"] as string;
			bool fragment = (bool)cArgs["-fragment"];
			int timeout = cArgs["-timeout"] as int? ?? 120;
			int bytes = cArgs["-bytes"] as int? ?? 32;
			int ttl = cArgs["-ttl"] as int? ?? 128;
			int delay = cArgs["-delay"] as int? ?? 1;
			delay *= 1000;

			byte[] buffer = System.Text.Encoding.ASCII.GetBytes(GenerateData(bytes));

			while (!ct.IsCancellationRequested)
			{
				System.Net.NetworkInformation.Ping ping = new();
				System.Net.NetworkInformation.PingOptions options = new()
				{
					Ttl = ttl,
					DontFragment = !fragment
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

		/// <summary>
		/// Generates a string of specified length with repeated character 'a'.
		/// </summary>
		/// <param name="bytes">The number of bytes in the generated string.</param>
		/// <returns>A string of specified length with repeated character 'a'.</returns>
		private static string GenerateData(int bytes)
		{
			string data = string.Empty;
			for (int i = 0; i < bytes; i++)
			{
				data += 'a';
			}
			return data;
		}
	}
}
