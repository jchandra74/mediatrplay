using FluentValidation.Results;

namespace MyTodos.Validation;

public sealed record ValidationFailed(IEnumerable<ValidationFailure> Errors)
{
    public ValidationFailed(ValidationFailure errors) : this(new[] { errors })
    {
    }
}
