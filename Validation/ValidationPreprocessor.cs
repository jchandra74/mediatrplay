using FluentValidation;
using MediatR.Pipeline;

namespace MyTodos.Validation;

public sealed class ValidationPreprocessor<TRequest> : IRequestPreProcessor<TRequest>
    where TRequest : IValidatableRequest
{
    private readonly IValidator<TRequest> _validator;

    public ValidationPreprocessor(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
    }
}
