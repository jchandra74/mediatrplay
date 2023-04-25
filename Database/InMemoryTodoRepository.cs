namespace MyTodos.Database;

public class InMemoryTodoRepository : ITodoRepository
{
    private readonly Dictionary<Guid, Todo> _data;
    private static readonly object _lock = new Object();

    public InMemoryTodoRepository()
    {
        _data = new Dictionary<Guid, Todo>();
    }

    public Task CreateAsync(Todo todo, CancellationToken cancellationToken)
    {
        lock(_lock)
        {
            _data[todo.Id] = todo;
            Console.WriteLine($"Created Todo: {todo.Id}: {todo.Title}");
        }

        return Task.CompletedTask;
    }

    public Task<Todo> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Fetching Todo with id {id}...");
        lock(_lock)
        {
            if (_data.ContainsKey(id)) return Task.FromResult(_data[id]);
        }

        throw new KeyNotFoundException(id.ToString());
    }
}