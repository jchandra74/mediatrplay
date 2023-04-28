namespace MyTodos.Database;

public interface ITodoRepository
{
    Task<Todo> CreateAsync(Todo todo, CancellationToken cancellationToken = default);

    Task<Todo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IEnumerable<Todo>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Todo?> UpdateAsync(Todo todo, CancellationToken cancellationToken = default);
}