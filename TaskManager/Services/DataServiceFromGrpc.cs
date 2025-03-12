using Grpc.Core;
using Mapster;
using TaskManager.Models;
using Proto = TaskManagerProvider.Protos;

namespace TaskManager.Services;

public class DataServiceFromGrpc : IDataService
{
    private readonly Proto.UserService.UserServiceClient _userServiceClient;
    private readonly Proto.TaskItemService.TaskItemServiceClient _taskItemServiceClient;

    public DataServiceFromGrpc(IGrpcChannelBuilder grpcChannelBuilder)
    {
        var channel = grpcChannelBuilder.Channel;

        _userServiceClient = new Proto.UserService.UserServiceClient(channel);
        _taskItemServiceClient = new Proto.TaskItemService.TaskItemServiceClient(channel);
    }

    public async Task<List<User>> GetUsersAsync()
    {
        var data = await _userServiceClient
            .GetUsers(new Proto.GetUsersRequest())
            .ResponseStream
            .ReadAllAsync()
            .ToListAsync();
        return data.Adapt<List<User>>();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        var data = await _userServiceClient
            .GetUserByIdAsync(new Proto.GetUserByIdRequest { Id = id })
            .ResponseAsync;
        return data.User.Adapt<User>();
    }

    public async Task<User> CreateUserAsync(User user)
    {
        var u = user.Adapt<Proto.User>();
        var data = await _userServiceClient
            .CreateUserAsync(new Proto.CreateUserRequest { User = u })
            .ResponseAsync;
        return data.User.Adapt<User>();
    }

    public async Task<User?> UpdateUserAsync(User user)
    {
        var u = user.Adapt<Proto.User>();
        var data = await _userServiceClient
            .UpdateUserAsync(new Proto.UpdateUserRequest { User = u })
            .ResponseAsync;
        return data.User.Adapt<User>();
    }

    public Task DeleteUserAsync(int id)
    {
        return _userServiceClient
            .DeleteUserAsync(new Proto.DeleteUserRequest { Id = id })
            .ResponseAsync;
    }

    public async Task<List<TaskItem>> GetTasksAsync()
    {
        var data = await _taskItemServiceClient
            .GetTasks(new Proto.GetTasksRequest())
            .ResponseStream
            .ReadAllAsync()
            .ToListAsync();

        return data.Adapt<List<TaskItem>>();
    }

    public async Task<List<TaskItem>> GetUserTasksAsync(int userId)
    {
        var data = await _taskItemServiceClient
            .GetUserTasks(new Proto.GetUserTasksRequest { UserId = userId })
            .ResponseStream
            .ReadAllAsync()
            .ToListAsync();

        return data.Adapt<List<TaskItem>>();
    }

    public async Task<TaskItem?> GetTaskByIdAsync(int id)
    {
        var data = await _taskItemServiceClient
            .GetTaskByIdAsync(new Proto.GetTaskByIdRequest { Id = id })
            .ResponseAsync;
        return data.Task.Adapt<TaskItem>();
    }

    public async Task<TaskItem> CreateTaskAsync(TaskItem task)
    {
        var t = task.Adapt<Proto.TaskItem>();
        var data = await _taskItemServiceClient
            .CreateTaskAsync(new Proto.CreateTaskRequest { Task = t })
            .ResponseAsync;
        return data.Task.Adapt<TaskItem>();
    }

    public async Task<TaskItem?> UpdateTaskAsync(TaskItem task)
    {
        var t = task.Adapt<Proto.TaskItem>();
        var data = await _taskItemServiceClient
            .UpdateTaskAsync(new Proto.UpdateTaskRequest { Task = t })
            .ResponseAsync;
        return data.Task.Adapt<TaskItem>();
    }

    public Task DeleteTaskAsync(int id)
    {
        return _taskItemServiceClient
            .DeleteTaskAsync(new Proto.DeleteTaskRequest { Id = id })
            .ResponseAsync;
    }
}