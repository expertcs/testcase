using TaskManagerProvider.Protos;

namespace TaskManagerProvider.Services
{
    internal interface IDataStore
    {
        ItemsList<TaskItem> Tasks { get; }
        ItemsList<User> Users { get; }
    }
}