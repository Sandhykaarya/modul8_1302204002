using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("Movie"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/Movies", async (TodoDb db) =>
    await db.Todos.ToListAsync());


app.MapPost("/api/Movies", async (Movie todo, TodoDb db) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/api/", todo);
});

/*
app.MapGet("/api/Movies/{id}", async (int id, Movie inputTodo, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Title = inputTodo.Title;
    todo.Director = inputTodo.Director;
    todo.Stars = inputTodo.Stars;
    todo.Desription = inputTodo.Desription;

    await db.SaveChangesAsync();

    return Results.NoContent();
}); */

app.MapGet("/api/Movies/{id}", () => "");

app.MapDelete("/api/Movies/{id}", async (int id, TodoDb db) =>
{
    if (await db.Todos.FindAsync(id) is Movie todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok(todo);
    }

    return Results.NotFound();
});

app.Run();

class Movie
{
    public string Title { get; set; }
    public string Director { get; set; }
    public List<string> Stars { get; set; }
    public string Desription { get; set; }

    public Movie(string Title, string Director, List<string> stars, string Description)
    {
        this.Title = Title;
        this.Director = Director;
        this.Stars = stars;
        this.Desription = Description;
    }
}

class TodoDb : DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options)
        : base(options) { }

    public DbSet<Movie> Todos => Set<Movie>();
}

/*app.MapGet("/api/Movies/{id}", async (int id, Movie inputTodo, TodoDb db) =>
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return Results.NotFound();

    todo.Title = inputTodo.Title;
    todo.Director = inputTodo.Director;
    todo.Stars = inputTodo.Stars;
    todo.Desription = inputTodo.Desription;

    await db.SaveChangesAsync();

    return Results.NoContent();
});*/