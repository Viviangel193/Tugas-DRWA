// Program.cs
using BookStoreApi.Models;
using BookStoreApi.Services;
using MongoDB.Bson;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

services.Configure<BookStoreDatabaseSettings>(
    builder.Configuration.GetSection("BookStoreDatabase"));

services.AddSingleton<BooksService>();
services.AddSingleton<AuthorsService>(); // Add AuthorsService

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Define the route handler for GET /books
app.MapGet("/books", async (BooksService booksService) =>
{
    var books = await booksService.GetAsync();
    return books;
})
.WithName("GetBooks")
.WithOpenApi();

// Define the route handler for GET /authors
app.MapGet("/authors", async (AuthorsService authorsService) =>
{
    var authors = await authorsService.GetAllAsync();
    return authors;
})
.WithName("GetAuthors")
.WithOpenApi();

// Define the route handler for POST /authors
app.MapPost("/authors", async (Author newAuthor, AuthorsService authorsService) =>
{
    await authorsService.CreateAsync(newAuthor);
    return Results.Created($"/authors/{newAuthor.Id}", newAuthor);
})
.WithName("CreateAuthor")
.WithOpenApi();

// Define the route handler for PUT /authors/{id}
app.MapPut("/authors/{id}", async (string id, Author updatedAuthor, AuthorsService authorsService) =>
{
    var author = await authorsService.GetAsync(id);
    if (author == null)
    {
        return Results.NotFound();
    }

    // Convert string ID to ObjectId
    ObjectId objectId;
    if (!ObjectId.TryParse(id, out objectId))
    {
        // Handle invalid ObjectId format here
        return Results.BadRequest("Invalid ID format");
    }

    // Update the author's ID
    updatedAuthor.Id = objectId;

    // Update the author
    await authorsService.UpdateAsync(id, updatedAuthor);
    return Results.NoContent();
})
.WithName("UpdateAuthor")
.WithOpenApi();


// Define the route handler for DELETE /authors/{id}
app.MapDelete("/authors/{id}", async (string id, AuthorsService authorsService) =>
{
    var author = await authorsService.GetAsync(id);
    if (author == null)
    {
        return Results.NotFound();
    }

    await authorsService.RemoveAsync(id);
    return Results.NoContent();
})
.WithName("DeleteAuthor")
.WithOpenApi();

app.Run();
