using System.Collections.Generic;
using System.Threading.Tasks;
using NotRemoting;
using NotRemoting.RpcMessaging;
using Grpc.AspNetCore.Server.Model;
using Grpc.Core;

namespace grpcdotnetServerNet50
{
	internal class NotRemotingMethodProvider : IServiceMethodProvider<NotRemotingService>
	{
		RemotingServer _server;

		public NotRemotingMethodProvider(RemotingServer server)
		{
			_server = server;
		}

		public void OnServiceMethodDiscovery(ServiceMethodProviderContext<NotRemotingService> context)
		{
			context.AddDuplexStreamingMethod(_server.DuplexCallDescriptor, new List<object>(), RpcCallBinaryFormatter);
		}

		Task RpcCallBinaryFormatter(NotRemotingService service, IAsyncStreamReader<GoreRequestMessage> input, IServerStreamWriter<GoreResponseMessage> output, ServerCallContext serverCallContext)
			=> _server.DuplexCall(input, output, serverCallContext);

	}
}

