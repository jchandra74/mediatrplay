namespace MyTodos.Database;

public interface ITodoRepository
{
    Task CreateAsync(Todo todo, CancellationToken cancellationToken = default);

    Task<Todo> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}