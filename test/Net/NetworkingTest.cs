using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using Confirma.Attributes;
using Confirma.Extensions;
using YAT.Net;
using static YAT.Net.Networking;

namespace YAT.Test;

[TestClass]
[Parallelizable]
public class NetworkingTest
{
    private readonly NetworkingOptions _opts = new() { BufferSize = 0 };

    #region Ping
    [TestCase]
    public void Ping_WithEmptyHostname_ReturnsUnknown()
    {
        EConnectionStatus result = Ping(string.Empty, out PingReply? reply, _opts);

        _ = result.ConfirmEqual(EConnectionStatus.Unknown);
        _ = reply.ConfirmNull();
    }

    [TestCase]
    public void Ping_WithValidHostname_ReturnsSuccess()
    {
        EConnectionStatus result = Ping("localhost", out PingReply? reply, _opts);

        _ = result.ConfirmEqual(EConnectionStatus.Success);
        _ = reply.ConfirmNotNull();
        _ = reply!.Status.ConfirmEqual(IPStatus.Success);
    }

    [TestCase]
    public void Ping_WithInvalidHostname_ReturnsUnknown()
    {
        EConnectionStatus result = Ping("invalid.host", out PingReply? reply, _opts);

        _ = result.ConfirmEqual(EConnectionStatus.Unknown);
        _ = reply.ConfirmNull();
    }
    #endregion Ping

    #region GetTraceRoute
    [TestCase]
    public void GetTraceRoute_WithValidHostname_ReturnsRoute()
    {
        IEnumerable<IPAddress?> traceRoute = GetTraceRoute("localhost", _opts, new());

        _ = traceRoute.ConfirmNotNull();
        _ = traceRoute.ConfirmCountGreaterThan(0);
    }

    [TestCase]
    public void GetTraceRoute_WithInvalidHostname_ReturnsEmpty()
    {
        IEnumerable<IPAddress?> traceRoute = GetTraceRoute("invalid.host", _opts, new());

        _ = traceRoute.ConfirmNotNull();
        _ = traceRoute.ConfirmCount(0);
    }
    #endregion GetTraceRoute

    #region CreateBuffer
    [TestCase]
    public void CreateBuffer_WithValidSize_ReturnsBuffer()
    {
        _ = CreateBuffer(256).ConfirmCount(256);
    }
    #endregion CreateBuffer
}
