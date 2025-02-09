using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using YAT.Helpers;

namespace YAT.Net;

public static class Networking
{
    /// <summary>
    /// Attempts to send an ICMP echo message (ping) to the specified host
    /// and returns the status of the connection.
    /// </summary>
    /// <remarks>
    /// This method returns EConnectionStatus.AccessDenied if the application
    /// is not running as an administrator on a platform that requires it to
    /// send ICMP echo message.
    /// </remarks>
    public static EConnectionStatus Ping(
        string hostname,
        out PingReply? reply,
        NetworkingOptions? options = null
    )
    {
        reply = null;

        if (string.IsNullOrEmpty(hostname))
        {
            return EConnectionStatus.Unknown;
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
            return EConnectionStatus.Success;
        }
        catch (PlatformNotSupportedException)
        {
            return !OS.IsRunningAsAdmin()
                ? EConnectionStatus.AccessDenied
                : EConnectionStatus.Unsupported;
        }
        catch (Exception ex) when (ex
            is PingException
            or InvalidOperationException
            or ArgumentException
        )
        {
            return EConnectionStatus.Unknown;
        }
        finally
        {
            ping.Dispose();
        }
    }

    /// <summary>
    /// Inspired by https://stackoverflow.com/questions/142614/traceroute-and-ping-in-c-sharp/45565253#45565253
    /// </summary>
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
            EConnectionStatus status = Ping(
                hostname,
                out PingReply? reply,
                new()
                {
                    Timeout = options.Timeout,
                    TTL = ttl,
                    BufferSize = options.BufferSize,
                    DontFragment = options.DontFragment
                }
            );

            if (status != EConnectionStatus.Success)
            {
                break;
            }

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
        if (size == 0)
        {
            return [];
        }

        byte[] buffer = new byte[size];
        new Random().NextBytes(buffer);
        return buffer;
    }
}
