using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MyTodos.Validation;

public static class ValidationExtensions
{
    public static MediatRServiceConfiguration AddValidation<TRequest, TResponse>(
        this MediatRServiceConfiguration config) where TRequest : notnull
    {
        return config.AddBehavior<
            IPipelineBehavior<TRequest, Result<TResponse, ValidationFailed>>,
            ValidationBehavior<TRequest, TResponse>>();
    }
}