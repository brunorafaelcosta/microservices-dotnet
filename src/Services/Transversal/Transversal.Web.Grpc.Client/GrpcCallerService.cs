using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Transversal.Web.Grpc.Client
{
    public static class GrpcCallerService
    {
        public static async Task<TResponse> CallService<TResponse>(
            string grpcAddress,
            ILogger logger,
            Func<GrpcChannel, Task<TResponse>> func)
        {
            if (string.IsNullOrEmpty(grpcAddress))
                throw new ArgumentNullException(nameof(grpcAddress));
            if (logger is null)
                throw new ArgumentNullException(nameof(logger));
            if (func is null)
                throw new ArgumentNullException(nameof(func));

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

            var channel = GrpcChannel.ForAddress(grpcAddress);

            logger.LogInformation("Creating gRPC client base address = {@grpcAddress}, BaseAddress = {@BaseAddress}", grpcAddress, channel.Target);

            try
            {
                return await func(channel);
            }
            catch (RpcException e)
            {
                logger.LogError(e, "Error calling via gRPC: {Status} - {Message}", e.Status, e.Message);
                return default;
            }
            finally
            {
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", false);
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", false);
            }
        }
    }
}
