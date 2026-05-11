using System.Text.RegularExpressions;
using Copilot_teste.Models;
using Copilot_teste.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseMiddleware<AuthenticationMiddleware>();

app.UseMiddleware<LoggingMiddleware>();

var users = new List<User>
{
    new User
    {
        Id = 1,
        FirstName = "John",
        LastName = "Doe",
        Email = "john.doe@techhive.com",
        Department = "IT"
    },
    new User
    {
        Id = 2,
        FirstName = "Jane",
        LastName = "Smith",
        Email = "jane.smith@techhive.com",
        Department = "HR"
    }
};

app.MapGet("/api/users", () =>
{
    var orderedUsers = users.OrderBy(u => u.Id);

    return Results.Ok(orderedUsers);
});

app.MapGet("/api/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);

    return user is null
        ? Results.NotFound($"Usuário com ID {id} não encontrado.")
        : Results.Ok(user);
});

app.MapPost("/api/users", (User user) =>
{
    var validationError = ValidateUser(user);

    if (validationError is not null)
    {
        return Results.BadRequest(validationError);
    }

    var emailAlreadyExists = users.Any(u =>
        u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase));

    if (emailAlreadyExists)
    {
        return Results.BadRequest("Já existe um usuário cadastrado com este e-mail.");
    }

    user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;

    users.Add(user);

    return Results.Created($"/api/users/{user.Id}", user);
});

app.MapPut("/api/users/{id}", (int id, User updatedUser) =>
{
    var validationError = ValidateUser(updatedUser);

    if (validationError is not null)
    {
        return Results.BadRequest(validationError);
    }

    var user = users.FirstOrDefault(u => u.Id == id);

    if (user is null)
    {
        return Results.NotFound($"Usuário com ID {id} não encontrado.");
    }

    var emailAlreadyExists = users.Any(u =>
        u.Id != id &&
        u.Email.Equals(updatedUser.Email, StringComparison.OrdinalIgnoreCase));

    if (emailAlreadyExists)
    {
        return Results.BadRequest("Já existe outro usuário cadastrado com este e-mail.");
    }

    user.FirstName = updatedUser.FirstName;
    user.LastName = updatedUser.LastName;
    user.Email = updatedUser.Email;
    user.Department = updatedUser.Department;

    return Results.Ok(user);
});

app.MapDelete("/api/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);

    if (user is null)
    {
        return Results.NotFound($"Usuário com ID {id} não encontrado.");
    }

    users.Remove(user);

    return Results.NoContent();
});

app.MapGet("/api/test-error", () =>
{
    throw new Exception("Erro de teste.");
});

static string? ValidateUser(User user)
{
    if (string.IsNullOrWhiteSpace(user.FirstName))
    {
        return "O primeiro nome é obrigatório.";
    }

    if (string.IsNullOrWhiteSpace(user.LastName))
    {
        return "O sobrenome é obrigatório.";
    }

    if (string.IsNullOrWhiteSpace(user.Email))
    {
        return "O e-mail é obrigatório.";
    }

    if (!Regex.IsMatch(user.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
    {
        return "O e-mail informado é inválido.";
    }

    if (string.IsNullOrWhiteSpace(user.Department))
    {
        return "O departamento é obrigatório.";
    }

    return null;
}

app.Run();