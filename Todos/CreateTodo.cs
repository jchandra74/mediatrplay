using MediatR;
using FluentValidation;

using MyTodos.Database;
using MyTodos.Validation;

namespace MyTodos;

public sealed record CreateTodoCommand(string Title, Guid Id = default) 
    : IValidatableRequest<Result<Todo, ValidationFailed>>;

public sealed class CreateTodoHandler : IRequestHandler<CreateTodoCommand, Result<Todo, ValidationFailed>>
{
    private readonly ITodoRepository _todoRepository;

    public CreateTodoHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<Result<Todo, ValidationFailed>> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var id = request.Id == default ? Guid.NewGuid() : request.Id;
        var todo = new Todo(id, request.Title);

        await _todoRepository.CreateAsync(todo);
        return todo;
    }
}

public sealed class CreateTodoValidator : AbstractValidator<CreateTodoCommand>
{
    private readonly ITodoRepository _todoRepository;

    public CreateTodoValidator(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;

        RuleFor(x => x.Id)
            .MustAsync(NotHaveTodoWithSameId)
            .WithName("Id")
            .WithMessage("This todo already exists in the system");

        RuleFor(x => x.Title).NotEmpty();
    }

    private async Task<bool> NotHaveTodoWithSameId(
        Guid id,
        CancellationToken cancellationToken)
    {
        var matchingTodo = await _todoRepository.GetByIdAsync(id, cancellationToken);
        return matchingTodo is null;
    }
}