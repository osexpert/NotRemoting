using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;

namespace NotRemoting.Tests.Tools;

public abstract class TestServer : RemotingServer, IAsyncDisposable
{
	protected TestServer(ServerConfig config) : base(config)
	{
	}

	public abstract ValueTask DisposeAsync();
}

public class NativeServer : TestServer
{

	Grpc.Core.Server _server;

	public NativeServer(int port, ServerConfig config) : base(config)
	{
		var options = new List<ChannelOption>();
		options.Add(new ChannelOption(ChannelOptions.MaxReceiveMessageLength, int.MaxValue));
		options.Add(new ChannelOption(ChannelOptions.MaxSendMessageLength, int.MaxValue));

		_server = new Grpc.Core.Server(options)
		{
			Services =
			{
				ServerServiceDefinition.CreateBuilder()
					.AddMethod(DuplexCallDescriptor, this.DuplexCall)
					.Build()
			}
		};

		_server.Ports.Add("0.0.0.0", port, ServerCredentials.Insecure);
	}

	private bool DisposeKills = true;

	public override ValueTask DisposeAsync()
	{
		if (_server != null)
			return new ValueTask(DisposeKills ? _server.KillAsync() : _server.ShutdownAsync());
		else
			return default;
	}

	public void Start() => _server.Start();
}

