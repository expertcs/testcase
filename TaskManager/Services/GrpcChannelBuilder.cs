using Grpc.Net.Client;

namespace TaskManager.Services
{
    public class GrpcChannelBuilder : IGrpcChannelBuilder
    {
        public GrpcChannel Channel { get; }

        public GrpcChannelBuilder(IConfiguration configuration)
        {
            var adr = configuration.GetValue<string>("GrpcChannel")!;
            Channel = GrpcChannel.ForAddress(adr);
        }
    }
}
