using Grpc.Core;

using TaskManagerProvider.Protos;

namespace TaskManagerProvider.Services;

internal class TaskItemService : Protos.TaskItemService.TaskItemServiceBase
{
    private readonly IDataStore _store;

    public TaskItemService(IDataStore store) : base()
        => _store = store;

    private void SetUser(TaskItem? task)
    {
        if (task != null)
            task.User = _store.Users.GetById(task.UserId);
    }

    public override Task GetUserTasks(GetUserTasksRequest request, IServerStreamWriter<TaskItem> responseStream, ServerCallContext context)
    {
        if (_store.Users.GetById(request.UserId) == null)
            return Task.CompletedTask;
        return Task.WhenAll(_store.Tasks
            .Where(t => t.UserId == request.UserId)
            .Select(t =>
            {
                SetUser(t);
                return responseStream.WriteAsync(t, context.CancellationToken);
            }));
    }

    public override Task GetTasks(GetTasksRequest request, IServerStreamWriter<TaskItem> responseStream, ServerCallContext context)
    {
        return Task.WhenAll(_store.Tasks
            .Select(t =>
            {
                SetUser(t);
                return responseStream.WriteAsync(t, context.CancellationToken);
            }));
    }

    public override Task<GetTaskByIdResponse> GetTaskById(GetTaskByIdRequest request, ServerCallContext context)
    {
        context.CancellationToken.ThrowIfCancellationRequested();
        var t = _store.Tasks.GetById(request.Id);
        SetUser(t);
        return Task.FromResult(new GetTaskByIdResponse { Task = t });
    }

    public override Task<CreateTaskResponse> CreateTask(CreateTaskRequest request, ServerCallContext context)
    {
        context.CancellationToken.ThrowIfCancellationRequested();
        var t = _store.Tasks.CreateItem(request.Task);
        SetUser(t);
        return Task.FromResult(new CreateTaskResponse { Task = t });
    }

    public override Task<UpdateTaskResponse> UpdateTask(UpdateTaskRequest request, ServerCallContext context)
    {
        context.CancellationToken.ThrowIfCancellationRequested();
        var t = _store.Tasks.UpdateItem(request.Task);
        SetUser(t);
        return Task.FromResult(new UpdateTaskResponse { Task = t });
    }

    public override Task<DeleteTaskResponse> DeleteTask(DeleteTaskRequest request, ServerCallContext context)
    {
        context.CancellationToken.ThrowIfCancellationRequested();
        _store.Tasks.DeleteItem(request.Id);
        return Task.FromResult(new DeleteTaskResponse());
    }
}
