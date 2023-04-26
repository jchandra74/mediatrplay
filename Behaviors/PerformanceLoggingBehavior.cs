using System.Diagnostics;

using Microsoft.Extensions.Logging;

using MediatR;

namespace MyTodos.Behaviors;

public sealed class PerformanceLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<PerformanceLoggingBehavior<TRequest, TResponse>> _logger;

    public PerformanceLoggingBehavior(ILogger<PerformanceLoggingBehavior<TRequest,TResponse>> logger)
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