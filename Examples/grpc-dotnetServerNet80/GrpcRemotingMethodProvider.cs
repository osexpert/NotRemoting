using NotRemoting;
using NotRemoting.RpcMessaging;
using Grpc.AspNetCore.Server.Model;
using Grpc.Core;

namespace grpcdotnetServerNet60;

internal class NotRemotingMethodProvider : IServiceMethodProvider<NotRemotingService>
{
	RemotingServer _server;

	public NotRemotingMethodProvider(RemotingServer server)
	{
		_server = server;
	}

	public void OnServiceMethodDiscovery(ServiceMethodProviderContext<NotRemotingService> context)
	{
		context.AddDuplexStreamingMethod(_server.DuplexCallDescriptor, new List<object>(), DuplexCall);
	}

	Task DuplexCall(NotRemotingService service, IAsyncStreamReader<GoreRequestMessage> input, IServerStreamWriter<GoreResponseMessage> output, ServerCallContext serverCallContext)
		=> _server.DuplexCall(input, output, serverCallContext);
}

