using TaskManagerProvider.Protos;

namespace TaskManagerProvider.Services
{
    internal class DataStore : IDataStore
    {
        public ItemsList<User> Users { get; }

        public ItemsList<TaskItem> Tasks { get; }

        public DataStore(ILogger<DataStore> logger)
        {
            Users = new(u => u.Id, (u, id) => u.Id = id, logger);
            Tasks = new(u => u.Id, (u, id) => u.Id = id, logger);

            LoadMockData();
        }

        private void LoadMockData()
        {
            var users = CreateMockData(Users, GenMockUsers());
            CreateMockData(Tasks, GenMockTasks(users));
        }

        private static Protos.User[] GenMockUsers()
        {
            return
            [
                new Protos.User { Name = "John Doe" },
                new Protos.User { Name = "Jane Smith" }
            ];
        }

        private static Protos.TaskItem[] GenMockTasks(Protos.User[] users)
        {
            var user1 = users[0];
            var user2 = users[1];
            return
            [
                new Protos.TaskItem { Name = "Complete project", UserId = user1.Id, State = Protos.TaskState.New },
                new Protos.TaskItem { Name = "Review code", UserId = user1.Id, State = Protos.TaskState.InProgress },
                new Protos.TaskItem { Name = "Write documentation", UserId = user2.Id, State = Protos.TaskState.New }
            ];
        }

        private static T[] CreateMockData<T>(ItemsList<T> items, params T[] data)
        {
            return data.Select(items.CreateItem).ToArray();
        }


    }
}
