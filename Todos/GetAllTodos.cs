using MediatR;
using MyTodos.Database;

namespace MyTodos;

public sealed record GetAllTodosQuery() : IRequest<IEnumerable<Todo>>;

public sealed class GetAllTodosHandler : IRequestHandler<GetAllTodosQuery, IEnumerable<Todo>>
{
    private readonly ITodoRepository _todoRepository;

    public GetAllTodosHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }
    public async Task<IEnumerable<Todo>> Handle(GetAllTodosQuery request, CancellationToken cancellationToken)
    {
        return await _todoRepository.GetAllAsync();
    }
}