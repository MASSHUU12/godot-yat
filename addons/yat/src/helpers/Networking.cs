using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using YAT.Types;

namespace YAT.Helpers;

public static class Networking
{
    public enum EPingStatus
    {
        Success,
        Unsupported,
        Unknown
    }

    public static EPingStatus Ping(string hostname, out PingReply? reply, NetworkingOptions? options = null)
    {
        reply = null;

        if (string.IsNullOrEmpty(hostname))
        {
            return EPingStatus.Unknown;
        }

        options ??= new();

        byte[] buffer = CreateBuffer(options.BufferSize);

        using Ping ping = new();

        try
        {
            reply = ping.Send(hostname, options.Timeout, buffer, new()
            {
                Ttl = options.TTL,
                DontFragment = options.DontFragment
            });
            return EPingStatus.Success;
        }
        catch (PlatformNotSupportedException)
        {
            return EPingStatus.Unsupported;
        }
        catch (Exception ex) when (ex
            is PingException
            or InvalidOperationException
            or ArgumentException
        )
        {
            return EPingStatus.Unknown;
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
    /// <param name="hostname"></param>
    /// <param name="options"></param>
    /// <param name="ct"></param>
    public static IEnumerable<IPAddress?> GetTraceRoute(
        string hostname,
        NetworkingOptions? options = null,
        CancellationToken ct = default
    )
    {
        if (string.IsNullOrEmpty(hostname))
        {
            yield return null;
        }

        options ??= new();

        using Ping ping = new();

        for (ushort ttl = 1; ttl <= options.TTL && !ct.IsCancellationRequested; ttl++)
        {
            EPingStatus status = Ping(hostname, out var reply, new NetworkingOptions
            {
                Timeout = options.Timeout,
                TTL = ttl,
                BufferSize = options.BufferSize,
                DontFragment = options.DontFragment
            });

            if (status != EPingStatus.Success)
            {
                break;
            }

            // Route has been found
            if (reply?.Status is IPStatus.Success or IPStatus.TtlExpired)
            {
                yield return reply.Address;
            }

            // Route has not been found or the host is unreachable
            if (reply?.Status is not IPStatus.TtlExpired and not IPStatus.TimedOut)
            {
                break;
            }
        }
    }

    public static byte[] CreateBuffer(uint size)
    {
        byte[] buffer = new byte[size];
        new Random().NextBytes(buffer);
        return buffer;
    }
}
