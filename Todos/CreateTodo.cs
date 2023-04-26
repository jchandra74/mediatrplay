using MediatR;
using FluentValidation;

using MyTodos.Database;
using MyTodos.Validation;

namespace MyTodos;

public sealed record CreateTodoCommand(string Title, Guid Id = default) 
    : IRequest<Result<Todo, ValidationFailed>>;

public sealed class CreateTodoHandler : IRequestHandler<CreateTodoCommand, Result<Todo, ValidationFailed>>
{
    private readonly ITodoRepository _todoRepository;
    private readonly IValidator<Todo> _todoValidator;

    public CreateTodoHandler(ITodoRepository todoRepository, IValidator<Todo> todoValidator)
    {
        _todoRepository = todoRepository;
        _todoValidator = todoValidator;
    }

    public async Task<Result<Todo, ValidationFailed>> Handle(
        CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var id = request.Id == default ? Guid.NewGuid() : request.Id;
        var todo = new Todo(id, request.Title);

        var validationResult = await _todoValidator.ValidateAsync(todo);

        if (!validationResult.IsValid)
        {
            return new ValidationFailed(validationResult.Errors);
        }

        await _todoRepository.CreateAsync(todo);
        return todo;
    }
}