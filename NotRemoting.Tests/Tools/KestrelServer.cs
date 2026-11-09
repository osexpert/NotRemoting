#if NET8_0_OR_GREATER
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.AspNetCore.Server;
using Grpc.AspNetCore.Server.Model;
using Grpc.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using NotRemoting;
using NotRemoting.RpcMessaging;

namespace NotRemoting.Tests.Tools;

internal class KestrelRemotingService { }

internal class KestrelRemotingMethodProvider : IServiceMethodProvider<KestrelRemotingService>
{
    private readonly RemotingServer _server;

    public KestrelRemotingMethodProvider(RemotingServer server) => _server = server;

    public void OnServiceMethodDiscovery(ServiceMethodProviderContext<KestrelRemotingService> context)
    {
        context.AddDuplexStreamingMethod(_server.DuplexCallDescriptor, new List<object>(), DuplexCall);
    }

    private Task DuplexCall(KestrelRemotingService svc, IAsyncStreamReader<RequestMessage> input,
        IServerStreamWriter<ResponseMessage> output, ServerCallContext context)
        => _server.DuplexCall(input, output, context);
}



public class KestrelServer : TestServer
{
    private readonly int _port;
    private WebApplication? _app;

    public KestrelServer(int port, ServerConfig config) : base(config)
    {
        _port = port;
    }

    public void Start()
    {
		Environment.SetEnvironmentVariable("DOTNET_USE_POLLING_FILE_WATCHER", "true");

		var builder = WebApplication.CreateBuilder();
        builder.Services.AddGrpc(o => o.IgnoreUnknownServices = true);
        builder.Services.Configure<RouteOptions>(c => c.SuppressCheckForUnhandledSecurityMetadata = true);
        builder.Services.AddSingleton<KestrelRemotingService>();
        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton(typeof(IServiceMethodProvider<KestrelRemotingService>),
                new KestrelRemotingMethodProvider(this)));
        builder.WebHost.ConfigureKestrel(k => k.ListenAnyIP(_port, lo => lo.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2));
        builder.Logging.ClearProviders();

        _app = builder.Build();
        _app.MapGrpcService<KestrelRemotingService>();
        _app.MapGet("/", () => "gRPC endpoint required");

        _ = _app.RunAsync();
    }

    public override ValueTask DisposeAsync()
    {
        return default;
    }
}
#endif