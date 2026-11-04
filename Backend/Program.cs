using BackEnd.Data;
using BackEnd.Modules.Auth;
using BackEnd.Modules.Authors;
using BackEnd.Modules.Books;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Base de datos (SQLite: se guarda en el archivo bookcase.db)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddControllers();

// Módulos
builder.Services.AddAuthModule(builder.Configuration);
builder.Services.AddAuthorsModule();
builder.Services.AddBooksModule();

// CORS: el frontend (Vue) necesita enviar la cookie, por eso AllowCredentials
var origins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>()
              ?? new[] { "http://localhost:5173" };

builder.Services.AddCors(options =>
    options.AddPolicy("Frontend", policy => policy
        .WithOrigins(origins)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()));

var app = builder.Build();

// En desarrollo, aplica las migraciones pendientes al arrancar
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
}

app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
