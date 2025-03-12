using Grpc.Core;

using TaskManagerProvider.Protos;

namespace TaskManagerProvider.Services;

internal class UserService : Protos.UserService.UserServiceBase
{
    private readonly IDataStore _store;

    public UserService(IDataStore store) : base()
        => _store = store;

    public override Task GetUsers(GetUsersRequest request, IServerStreamWriter<User> responseStream, ServerCallContext context)
    {
        return Task.WhenAll(_store.Users.Select(u => responseStream.WriteAsync(u, context.CancellationToken)));
    }

    public override Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request, ServerCallContext context)
    {
        context.CancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(new GetUserByIdResponse { User = _store.Users.GetById(request.Id) });
    }

    public override Task<CreateUserResponse> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        context.CancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(new CreateUserResponse { User = _store.Users.CreateItem(request.User) });
    }

    public override Task<UpdateUserResponse> UpdateUser(UpdateUserRequest request, ServerCallContext context)
    {
        context.CancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(new UpdateUserResponse { User = _store.Users.UpdateItem(request.User) });
    }

    public override Task<DeleteUserResponse> DeleteUser(DeleteUserRequest request, ServerCallContext context)
    {
        context.CancellationToken.ThrowIfCancellationRequested();
        _store.Tasks.DeleteItems(t => t.UserId == request.Id);
        _store.Users.DeleteItem(request.Id);
        return Task.FromResult(new DeleteUserResponse());
    }
}
