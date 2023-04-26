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

    public Task CreateAsync(Todo todo, CancellationToken cancellationToken)
    {
        lock(_lock)
        {
            _data[todo.Id] = todo;
            _logger.LogDebug("Created Todo: {todoId}: {todoTitle}", todo.Id, todo.Title);
        }

        return Task.CompletedTask;
    }

    public Task<Todo> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Fetching Todo with id: {todoId}", id);
        lock(_lock)
        {
            if (_data.ContainsKey(id)) return Task.FromResult(_data[id]);
        }

        throw new KeyNotFoundException(id.ToString());
    }

    public Task<IEnumerable<Todo>> GetAllAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Fetching all Todos...");
        lock(_lock)
        {
            return Task.FromResult(_data.Values.AsEnumerable());
        }
    }
}