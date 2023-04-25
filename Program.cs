using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MediatR;
using FluentValidation;

using MyTodos;
using MyTodos.Database;
using MyTodos.Behaviors;

var builder = Host.CreateDefaultBuilder(Environment.GetCommandLineArgs())
    .ConfigureAppConfiguration((hostingContext, config) => {
        config.AddJsonFile("appsettings.json", optional: false);
    })
    .ConfigureServices((hostContext, services) => {
        services.AddSingleton<ITodoRepository, InMemoryTodoRepository>();
        services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);
        services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<Program>()
            //.AddBehavior<IPipelineBehavior<CreateTodoCommand, Result<Todo, ValidationFailed>>, CreateTodoBehavior>());
            .AddOpenBehavior(typeof(LoggingBehavior<,>)));
    });

var host = builder.Build();

await host.StartAsync();

var mediatr = host.Services.GetRequiredService<IMediator>();

var command1 = new CreateTodoCommand("Figure out how to use Mediatr.");
var result1 = await mediatr.Send(command1);

var output1 = result1.Match<string>(
    todo => todo.Title,
    failure => failure.Errors
                .Select(f => f.ErrorMessage)
                .Aggregate((c,n) => c + n)
);

var command2 = new CreateTodoCommand("Go to bed.");
var result2 = await mediatr.Send(command2);

Guid uid2 = Guid.Empty;

var output2 = result2.Match<string>(
    todo => { 
        uid2 = todo.Id;
        return todo.Title;
    },
    failure => failure.Errors
                .Select(f => f.ErrorMessage)
                .Aggregate((c,n) => c + n)
);

var query1 = new GetTodoQuery(uid2);
var result3 = await mediatr.Send(query1);

var output3 = result2.Match<string>(
    todo => todo.Title,
    failure => failure.Errors
                .Select(f => f.ErrorMessage)
                .Aggregate((c,n) => c + n)
);
Console.WriteLine($"Retrieved Todo text is: {output3}");

await host.StopAsync();


