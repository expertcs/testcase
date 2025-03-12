using System.Collections;
using System.Collections.Concurrent;

namespace TaskManagerProvider.Services;

internal class ItemsList<T> : IEnumerable<T>
{
    private int _lastId = 0;
    private readonly ConcurrentDictionary<int, T> _items = new();
    private readonly Func<T, int> _getId;
    private readonly Action<T, int> _setId;
    private readonly ILogger _logger;

    public ItemsList(Func<T, int> getId, Action<T, int> setId, ILogger logger)
    {
        _getId = getId;
        _setId = setId;
        _logger = logger;
    }

    public T? GetById(int id)
    {
        _items.TryGetValue(id, out var item);
        return item;
    }

    public T CreateItem(T item)
    {
        _setId(item, Interlocked.Increment(ref _lastId));
        _logger.LogInformation("{name}({item})", nameof(CreateItem), item);
        if (_items.TryAdd(_getId(item), item))
            return item;
        throw new InvalidOperationException($"Failed to create {typeof(T).Name.ToLower()}");
    }

    public T? UpdateItem(T item)
    {
        _logger.LogInformation("{name}({item})", nameof(UpdateItem), item);
        var id = _getId(item);
        if (_items.ContainsKey(id))
        {
            _items[id] = item;
            return item;
        }
        return default;
    }

    public void DeleteItem(int id)
    {
        _logger.LogInformation("{name}({id})", nameof(DeleteItem), id);
        if (!_items.Remove(id, out var _))
            throw new InvalidOperationException($"Failed to delete {typeof(T).Name.ToLower()}");
    }

    public void DeleteItems(Func<T, bool> removing)
    {
        this.Where(removing).Select(_getId).ToList().ForEach(DeleteItem);
    }

    public IEnumerator<T> GetEnumerator() => _items.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
