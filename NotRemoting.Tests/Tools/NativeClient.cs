using System;
using System.Threading.Tasks;
using Grpc.Core;
using Channel = Grpc.Core.Channel;

namespace NotRemoting.Tests.Tools;


public abstract class TestClient : RemotingClient, IAsyncDisposable
{
	protected TestClient(CallInvoker callInvoker, ClientConfig config) : base(callInvoker, config)
	{
	}

	public abstract ValueTask DisposeAsync();
}

public class NativeClient : TestClient
{
	Channel _channel;

	public NativeClient(int port, ClientConfig config) : base(GetInvoker(port, out var channel), config)
	{
		_channel = channel;
	}

	private static CallInvoker GetInvoker(int port, out Channel channel)
	{
		channel = new Channel("localhost", port, ChannelCredentials.Insecure);
		return channel.CreateCallInvoker();
	}

	public override ValueTask DisposeAsync()
	{
		if (_channel != null)
			return new ValueTask(_channel.ShutdownAsync());
		else
			return default;
	}


}

