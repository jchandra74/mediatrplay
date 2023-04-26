﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        services.AddSingleton<TodoController>();
        services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);
        services.AddMediatR(c => c.RegisterServicesFromAssemblyContaining<Program>()
            //.AddBehavior<IPipelineBehavior<CreateTodoCommand, Result<Todo, ValidationFailed>>, CreateTodoBehavior>());
            .AddOpenBehavior(typeof(PerformanceLoggingBehavior<,>)));
    });

var host = builder.Build();

await host.StartAsync();

var controller = host.Services.GetRequiredService<TodoController>();
await controller.AddTodo("Figure out how to use Mediatr");
Guid id = Guid.NewGuid();
await controller.AddTodo("Figure out how he implemented the discriminated union", id);
await controller.AddTodo("Go to bed.");
await controller.ShowTodo(id);
await controller.ShowAllTodos();

await host.StopAsync();


