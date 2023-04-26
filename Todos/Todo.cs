namespace MyTodos;

public sealed record Todo(Guid Id, string Title, bool Completed = false);
