#if NET8_0_OR_GREATER
using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Grpc.Core;
using NotRemoting;

namespace NotRemoting.Tests.Tools;

public class KestrelClient : TestClient
{
    private readonly GrpcChannel? _channel;

    public KestrelClient(int port, ClientConfig config)
        : base(CreateInvoker(port, out var channel), config)
    {
        _channel = channel;
    }

    private static CallInvoker CreateInvoker(int port, out GrpcChannel channel)
    {
        channel = GrpcChannel.ForAddress($"http://localhost:{port}");
        return channel.CreateCallInvoker();
    }

    public override ValueTask DisposeAsync()
    {
        if (_channel != null)
            return new ValueTask(_channel.ShutdownAsync());
        return default;
    }
}
#endif