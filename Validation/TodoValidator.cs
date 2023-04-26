using FluentValidation;

namespace MyTodos.Validation;

public sealed class TodoCommandValidator : AbstractValidator<Todo>
{
    public TodoCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
    }
}