using BookStoreApi.Models;
using BookStoreApi.Services;
using MongoDB.Bson;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt; // Add this using directive
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Linq; // Add this using directive for Enumerable
using System.Collections.Generic; // Add this using directive for List

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    var key = builder.Configuration["Jwt:Key"];
    if (!string.IsNullOrEmpty(key))
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true
        };
    }
});
builder.Services.AddAuthorization();
// Add configuration from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

services.Configure<BookStoreDatabaseSettings>(
    builder.Configuration.GetSection("BookStoreDatabase"));

services.AddSingleton<BooksService>();
services.AddSingleton<AuthorsService>(); // Add AuthorsService

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// Generate JWT token
string? key = builder.Configuration["Jwt:Key"];
if (key == null)
{
    // Handle the case where the key is null
    throw new InvalidOperationException("JWT key is not configured.");
}

var tokenDescriptor = new SecurityTokenDescriptor
{
    Subject = new ClaimsIdentity(new Claim[]
    {
        new Claim(ClaimTypes.Name, "username"), // Replace "username" with actual user information
        // Add more claims as needed
    }),
    Expires = DateTime.UtcNow.AddHours(1), // Token expiration time
    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature)
};

var tokenHandler = new JwtSecurityTokenHandler();
var token = tokenHandler.CreateToken(tokenDescriptor);
var tokenString = tokenHandler.WriteToken(token);

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

// Define the route handler for GET /weatherforecast
app.MapGet("/weatherforecast", () =>
{
    var rng = new Random();
    var summaries = new List<string> { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
    var forecasts = Enumerable.Range(1, 5).Select(index => new
    {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = rng.Next(-20, 55),
        Summary = summaries[rng.Next(summaries.Count)]
    })
    .ToArray();
    return forecasts;
})
.WithName("GetWeatherForecast")
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

// Define the route handler for GET /books/{id}
app.MapGet("/books/{id}", async (HttpContext context, string id, BooksService booksService) =>
{
    var book = await booksService.GetAsync(id);
    if (book == null)
    {
        context.Response.StatusCode = 404; // Not Found
        return;
    }
    await context.Response.WriteAsJsonAsync(book);
})
.WithName("GetBookById")
.WithOpenApi();

// Define the route handler for POST /books
app.MapPost("/books", async (HttpContext context, BooksService booksService) =>
{
    var newBook = await context.Request.ReadFromJsonAsync<Book>();
    await booksService.CreateAsync(newBook);
    context.Response.StatusCode = 201; // Created
    context.Response.Headers["Location"] = $"/books/{newBook.Id}";
    await context.Response.WriteAsJsonAsync(newBook);
})
.WithName("CreateBook")
.WithOpenApi();

// Define the route handler for PUT /books/{id}
app.MapPut("/books/{id}", async (string id, Book updatedBook, BooksService booksService) =>
{
    var book = await booksService.GetAsync(id);
    if (book == null)
    {
        return Results.NotFound();
    }

    // Ensure the ID is a valid ObjectId format
    if (!ObjectId.TryParse(id, out _))
    {
        // Handle invalid ObjectId format here
        return Results.BadRequest("Invalid ID format");
    }

    // Update the book's ID to match the path parameter
    updatedBook.Id = id;

    // Update the book
    await booksService.UpdateAsync(id, updatedBook);
    return Results.NoContent();
})
.WithName("UpdateBook")
.WithOpenApi();



// Define the route handler for DELETE /books/{id}
app.MapDelete("/books/{id}", async (HttpContext context, string id, BooksService booksService) =>
{
    var book = await booksService.GetAsync(id);
    if (book == null)
    {
        context.Response.StatusCode = 404; // Not Found
        return;
    }

    await booksService.RemoveAsync(id);
    context.Response.StatusCode = 204; // No Content
    
})
.WithName("DeleteBook")
.WithOpenApi();


app.Run();
