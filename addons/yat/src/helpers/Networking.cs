using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using YAT.Types;

namespace YAT.Helpers;

public static class Networking
{
	public static PingReply? Ping(string hostname, NetworkingOptions? options = null)
	{
		if (string.IsNullOrEmpty(hostname)) return null;

		options ??= new();

		byte[] buffer = CreateBuffer(options.BufferSize);

		using var ping = new Ping();

		try
		{
			PingReply reply = ping.Send(hostname, options.Timeout, buffer, new()
			{
				Ttl = options.TTL,
				DontFragment = options.DontFragment
			});
			return reply;
		}
		catch (Exception ex) when (ex
			is PingException
			or InvalidOperationException
			or ArgumentException
		)
		{
			return null;
		}
		finally
		{
			ping.Dispose();
		}
	}

	/// <summary>
	/// Returns a list of IP addresses that a packet would take to reach the specified host. <br />
	/// Inspired by https://stackoverflow.com/questions/142614/traceroute-and-ping-in-c-sharp/45565253#45565253
	/// </summary>
	public static IEnumerable<IPAddress?> GetTraceRoute(
		string hostname,
		NetworkingOptions? options = null,
		CancellationToken ct = default
	)
	{
		if (string.IsNullOrEmpty(hostname)) yield return null;

		options ??= new();

		byte[] buffer = CreateBuffer(options.BufferSize);

		using var ping = new Ping();

		for (ushort ttl = 1; ttl <= options.TTL && !ct.IsCancellationRequested; ttl++)
		{
			PingOptions pOptions = new(ttl, true);
			PingReply reply = ping.Send(hostname, options.Timeout, buffer, pOptions);

			// Route has been found
			if (reply.Status == IPStatus.Success || reply.Status == IPStatus.TtlExpired)
				yield return reply.Address;

			// Route has not been found or the host is unreachable
			if (reply.Status != IPStatus.TtlExpired && reply.Status != IPStatus.TimedOut)
				break;
		}
	}

	public static byte[] CreateBuffer(uint size)
	{
		byte[] buffer = new byte[size];
		new Random().NextBytes(buffer);
		return buffer;
	}
}
