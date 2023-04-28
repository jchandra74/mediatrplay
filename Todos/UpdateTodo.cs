using FluentValidation;
using FluentValidation.Results;
using MediatR;
using MyTodos.Database;
using MyTodos.Validation;

namespace MyTodos;

public sealed record UpdateTodoCommand(Guid Id, string Title, bool Completed)
    : IValidatableRequest<Result<Todo?, ValidationFailed>>;

public sealed class UpdateTodoHandler : IRequestHandler<UpdateTodoCommand, Result<Todo?, ValidationFailed>>
{
    private readonly ITodoRepository _todoRepository;

    public UpdateTodoHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }
    public async Task<Result<Todo?, ValidationFailed>> Handle(
        UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        var todo = new Todo(request.Id, request.Title, request.Completed);
        return await _todoRepository.UpdateAsync(todo);
    }
}

public sealed class UpdateTodoValidator : AbstractValidator<UpdateTodoCommand>
{
    private readonly ITodoRepository _todoRepository;

    public UpdateTodoValidator(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;

        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .MustAsync(HaveTodoWithSameId)
            .WithMessage("This todo cannot be found in the system");

        RuleFor(x => x.Title).NotEmpty();
    }

    private async Task<bool> HaveTodoWithSameId(
        Guid id,
        CancellationToken cancellationToken)
    {
        var matchingTodo = await _todoRepository.GetByIdAsync(id, cancellationToken);
        return matchingTodo is not null;
    }
}