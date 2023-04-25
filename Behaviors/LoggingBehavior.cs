using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MyTodos.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest,TResponse>> logger)
    {
        _logger = logger;    
    }
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        var start = Stopwatch.GetTimestamp();

        TResponse response;

        try 
        {
            response = await next();
        }
        finally
        {
            var requestName = typeof(TRequest).Name;
            var delta = Stopwatch.GetElapsedTime(start);
            _logger.LogInformation("{RequestName} completed in {EllapseMS}ms",
                requestName,
                delta.Milliseconds);
        }

        return response;
    }
}