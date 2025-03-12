using TaskManagerProvider.Services;

namespace TaskManagerProvider
{
    internal static class Program
    {
        private static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services
                .AddSingleton<IDataStore, DataStore>()
                .AddGrpc()
                ;

            var app = builder.Build();
            app.MapGrpcService<UserService>();
            app.MapGrpcService<TaskItemService>();

            return app.RunAsync();
        }
    }
}
