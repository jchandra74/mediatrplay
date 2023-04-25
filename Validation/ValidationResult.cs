using FluentValidation.Results;

namespace MyTodos.Validation;

public class ValidationResult
{
    public bool IsValid { get; }

    public IEnumerable<ValidationFailure> Errors { get; }
    
    public ValidationResult(IEnumerable<ValidationFailure> errors)
    {
        Errors = errors ?? new List<ValidationFailure>();
        IsValid = Errors.Any();        
    }
}