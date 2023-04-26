using MediatR;
using MyTodos.Database;

namespace MyTodos;

public sealed record GetTodoQuery(Guid Id) : IRequest<Todo?>;

public sealed class GetTodoHandler : IRequestHandler<GetTodoQuery, Todo?>
{
    private readonly ITodoRepository _todoRepository;

    public GetTodoHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }
    
    public async Task<Todo?> Handle(
        GetTodoQuery request, CancellationToken cancellationToken)
    {
        return await _todoRepository.GetByIdAsync(request.Id);
    }
}