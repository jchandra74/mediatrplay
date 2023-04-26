using System.Diagnostics;

using Microsoft.Extensions.Logging;

using MediatR;

using MyTodos.Validation;

namespace MyTodos.Behaviors;

public sealed class CreateTodoBehavior : IPipelineBehavior<CreateTodoCommand, Result<Todo, ValidationFailed>>
{
    private readonly ILogger<CreateTodoBehavior> _logger;

    public CreateTodoBehavior(ILogger<CreateTodoBehavior> logger)
    {
        _logger = logger;
    }
    public async Task<Result<Todo, ValidationFailed>> Handle(
        CreateTodoCommand request, 
        RequestHandlerDelegate<Result<Todo, ValidationFailed>> next, 
        CancellationToken cancellationToken)
    {
        var start = Stopwatch.GetTimestamp();

        var result = await next();

        var delta = Stopwatch.GetElapsedTime(start);
        _logger.LogInformation("CreateMovieCommand {Status} in {EllapseMS}ms",
            result.IsSuccess ? "Succeeded" : "Failed", delta.Milliseconds);

        return result;
    }
}