using Microsoft.Extensions.Logging;

using MediatR;

namespace MyTodos;

public sealed class TodoController
{
    private readonly IMediator _mediatr;
    private readonly ILogger<TodoController> _logger;

    public TodoController(IMediator mediatr, ILogger<TodoController> logger)
    {
        _mediatr = mediatr;
        _logger = logger;
    }
    public async Task AddTodo(string title, Guid id = default)
    {
        var command = new CreateTodoCommand(title, id);
        var result = await _mediatr.Send(command);

        result.Switch(
            todo => {  },
            failure => {
                var errors = failure.Errors.Select(f => f.ErrorMessage).Aggregate((c, n) => string.Join(' ', c, n));
                _logger.LogError("Failures: {failures}", errors);
            }
        );
    }

    public async Task ShowTodo(Guid id)
    {
        var query = new GetTodoQuery(id);
        var result = await _mediatr.Send(query);

        if (result != null) 
            _logger.LogInformation("Retrieved Todo text is: {title}", result.Title);
        else
            _logger.LogWarning("Unable to retrieve Todo with id: {id}", id);
    }
}