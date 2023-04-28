using FluentValidation;
using MediatR;

namespace MyTodos.Validation;

public sealed class ValidationBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, Result<TResult, ValidationFailed>>
    where TRequest : notnull
{
    private readonly IValidator<TRequest> _validator;

    public ValidationBehavior(IValidator<TRequest> validator)
    {
        _validator = validator;
    }
    public async Task<Result<TResult, ValidationFailed>> Handle(
        TRequest request, 
        RequestHandlerDelegate<Result<TResult, ValidationFailed>> next, 
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return new ValidationFailed(validationResult.Errors);

        return await next();
    }
}