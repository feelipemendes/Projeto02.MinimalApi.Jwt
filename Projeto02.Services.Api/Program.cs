using Microsoft.EntityFrameworkCore;
using Projeto02.Services.Api.Contexts;
using Projeto02.Services.Api.Security;
using Projeto02.Services.Api.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connStr = builder.Configuration.GetConnectionString("BDProjeto02");

builder.Services.AddDbContext<SqlServerContext>
    (options => options.UseSqlServer(connStr));

JwtConfiguration.AddJwtBearerConfiguration(builder);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/api/register", (SqlServerContext context, RegisterViewModel model) =>
{
    if (context.Usuarios.FirstOrDefault(u => u.Email.Equals(model.Email)) != null)
    {
        return Results.BadRequest(new { message = "O email informado já está cadastrado, tente novamente." });
    }

    var usuario = model.MapTo();

    if (!model.IsValid)
        return Results.BadRequest(model.Notifications);

    context.Usuarios.Add(usuario);
    context.SaveChanges();

    return Results.Ok(new { usuario.Id, usuario.Name, usuario.Email, usuario.CreatedAt });
});

app.MapPost("/api/login", (SqlServerContext context, JwtTokenService service, LoginViewModel model) =>
{
    var login = model.MapTo();
    if (!model.IsValid)
    {
        return Results.BadRequest(model.Notifications);
    }

    var usuario = context.Usuarios
        .FirstOrDefault(u => u.Email.Equals(login.Email)
                            && u.Password.Equals(login.Password));

    if (usuario == null)
    {
        return Results.BadRequest(new { message = "Acesso Negado. Usuário inválido." });
    }

    return Results.Ok(
        new
        {
            usuario.Id,
            usuario.Name,
            usuario.Email,
            usuario.CreatedAt,
            accessToken = service.Get(usuario.Email)
        });

    return Results.Ok();
});

app.MapPost("/api/password", (SqlServerContext context, PasswordRecoverViewModel model) =>
{
    return Results.Ok();
});


app.Run();
