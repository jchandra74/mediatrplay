using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using FluentValidation;

using MyTodos;
using MyTodos.Database;
using MyTodos.Behaviors;
using MediatR;
using MyTodos.Validation;

var builder = Host.CreateDefaultBuilder(Environment.GetCommandLineArgs())
    .ConfigureAppConfiguration((hostingContext, config) => {
        config.AddJsonFile("appsettings.json", optional: false);
    })
    .ConfigureServices((hostContext, services) => {
        services.AddSingleton<ITodoRepository, InMemoryTodoRepository>();
        services.AddSingleton<TodoController>();
        services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);
        services.AddMediatR(c => 
            c.RegisterServicesFromAssemblyContaining<Program>()
                .AddOpenBehavior(typeof(PerformanceLoggingBehavior<,>))
                .AddValidation<CreateTodoCommand, Todo>()
                .AddValidation<UpdateTodoCommand, Todo?>()
        );
    });

var host = builder.Build();

await host.StartAsync();

var controller = host.Services.GetRequiredService<TodoController>();
await controller.AddTodo("Figure out how to use Mediatr");
Guid id = Guid.NewGuid();
await controller.AddTodo("Figure out how he implemented the discriminated union", id);
await controller.AddTodo("Go to bed.");
await controller.ShowTodo(id);
await controller.ShowTodo(Guid.NewGuid());                      // This should fail
await controller.ShowAllTodos();
await controller.UpdateTodo(Guid.NewGuid(), "Whatever", true);  //This should fail
await controller.UpdateTodo(id, "Figure out how he implemented the discriminated union", true);

await host.StopAsync();


