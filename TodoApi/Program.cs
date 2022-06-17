using Microsoft.EntityFrameworkCore;
using TodoApi.Middlewares;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


try
{
    builder.Services.AddDbContext<TodoDb>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    using (var db = builder.Services.BuildServiceProvider().GetService<TodoDb>())
    {
        db.Database.EnsureCreated();
    }

    Console.WriteLine("PostgreSQL database created");
}
catch
{
    // if there is an error, using the in-memory database
    var dbService = builder.Services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<TodoDb>));

    builder.Services.Remove(dbService);

    builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));

    Console.WriteLine("In-memory database created");
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapGet("/v1/todos", async (TodoDb db) =>
    await db.Todos.ToListAsync());


app.MapGet("/v1/todos/{id}", async (Guid id, TodoDb db) =>
    await db.Todos.FindAsync(id)
        is Todo todo
            ? Results.Ok(todo)
            : Results.NotFound());


app.MapPost("/v1/todos", async (Todo todo, TodoDb db) =>
{
    if (!todo.IsValid()) return Results.BadRequest(new GenericResponse("Invalid model", todo));
    db.Todos.Add(todo);
    await db.SaveChangesAsync();
    return Results.Created($"/todoitems/{todo.Id}", todo);
});


app.MapPut("/todos/{id}", async (Guid id, Todo todo, TodoDb db) =>
{
    if (!todo.IsValid()) return Results.BadRequest(new GenericResponse("Invalid model", todo));
    var recorded = await db.Todos.FindAsync(id);
    if (recorded is null) return Results.NotFound();
    recorded.Title = todo.Title;
    recorded.Done = todo.Done;
    await db.SaveChangesAsync();
    return Results.Ok();
});


app.MapDelete("/todos/{id}", async (Guid id, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);
    if (todo is null) return Results.NotFound();
    db.Todos.Remove(todo);
    await db.SaveChangesAsync();
    return Results.Ok();
});


app.Run();