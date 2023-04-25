using FluentValidation;

namespace MyTodos.Validation;

public class TodoCommandValidator : AbstractValidator<Todo>
{
    public TodoCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
    }
}