using System.Text.Json;
using TheDashboard.Proxy.Services;
using TheDashboard.SharedEntities;
using Yarp.ReverseProxy.Forwarder;
using Yarp.ReverseProxy.Transforms;

namespace TheDashboard.Proxy.Transformers;

public class ServiceTransformer : HttpTransformer
{

    private readonly ICommandServiceRepository _commandServiceRepository;

    public ServiceTransformer(ICommandServiceRepository commandServiceRepository)
    {
        _commandServiceRepository = commandServiceRepository;
    }

    public override async ValueTask TransformRequestAsync(HttpContext httpContext,
      HttpRequestMessage proxyRequest, string destinationPrefix, CancellationToken cancellationToken)
    {
        // Copy all request headers
        await base.TransformRequestAsync(httpContext, proxyRequest, destinationPrefix, cancellationToken);

        // check that it's a POST request and contains a header specific for commands
        if (httpContext.Request.Method == "POST" && httpContext.Request.Headers.ContainsKey("X-Command"))
        {
            // get the command from the body
            var command = await JsonSerializer.DeserializeAsync<Command>(httpContext.Request.Body, cancellationToken: cancellationToken);
            // store the command in event store and publish it to the bus
            if (command != null)
            {
                await _commandServiceRepository.StoreAndPublish(command, cancellationToken);
            }
        }

    }
}