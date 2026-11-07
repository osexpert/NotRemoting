using NotRemoting;
using NotRemoting.RpcMessaging;
using Grpc.AspNetCore.Server.Model;
using Grpc.Core;

namespace grpcdotnetServerNet60;

internal class RemotingMethodProvider : IServiceMethodProvider<RemotingService>
{
	RemotingServer _server;

	public RemotingMethodProvider(RemotingServer server)
	{
		_server = server;
	}

	public void OnServiceMethodDiscovery(ServiceMethodProviderContext<RemotingService> context)
	{
		context.AddDuplexStreamingMethod(_server.DuplexCallDescriptor, new List<object>(), DuplexCall);
	}

	Task DuplexCall(RemotingService service, IAsyncStreamReader<RequestMessage> input, IServerStreamWriter<ResponseMessage> output, ServerCallContext serverCallContext)
		=> _server.DuplexCall(input, output, serverCallContext);
}

