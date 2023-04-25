using FluentValidation.Results;

namespace MyTodos.Validation;

public record ValidationFailed(IEnumerable<ValidationFailure> Errors)
{
    public ValidationFailed(ValidationFailure errors) : this(new[] { errors })
    {        
    }
}
