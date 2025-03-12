using Grpc.Net.Client;

namespace TaskManager.Services
{
    public interface IGrpcChannelBuilder
    {
        GrpcChannel Channel { get; }
    }
}