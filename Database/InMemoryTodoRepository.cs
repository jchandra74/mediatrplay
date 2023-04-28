using Microsoft.Extensions.Logging;

namespace MyTodos.Database;

public sealed class InMemoryTodoRepository : ITodoRepository
{
    private static readonly object _lock = new Object();
    
    private readonly Dictionary<Guid, Todo> _data = new Dictionary<Guid, Todo>();
    private readonly ILogger<InMemoryTodoRepository> _logger;

    public InMemoryTodoRepository(ILogger<InMemoryTodoRepository> logger)
    {
        _logger = logger;
    }

    public Task<Todo> CreateAsync(Todo todo, CancellationToken cancellationToken)
    {
        lock(_lock)
        {
            _data[todo.Id] = todo;
            _logger.LogDebug("Created Todo: {todoId}: {todoTitle}", todo.Id, todo.Title);
        }

        return Task.FromResult(todo);
    }

    public Task<Todo?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        lock(_lock)
        {
            if (_data.ContainsKey(id)) return Task.FromResult((Todo?)_data[id]);
        }
        return Task.FromResult(default(Todo));
    }

    public Task<IEnumerable<Todo>> GetAllAsync(CancellationToken cancellationToken)
    {
        lock(_lock)
        {
            return Task.FromResult(_data.Values.AsEnumerable());
        }
    }

    public Task<Todo?> UpdateAsync(Todo todo, CancellationToken cancellationToken = default)
    {
        lock(_lock)
        {
            if (!_data.ContainsKey(todo.Id)) return Task.FromResult(default(Todo));
            
            _data[todo.Id] = todo;
            return Task.FromResult((Todo?)todo);
        }
    }
}