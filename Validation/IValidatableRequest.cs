using MediatR;

namespace MyTodos.Validation;

public interface IValidatableRequest<out TResponse>
    : IRequest<TResponse>, IValidatableRequest {}

public interface IValidatableRequest { }