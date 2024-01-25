using Event_bridge.Data;
using Event_bridge.Interfaces;
using Event_bridge.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Configuração com banco de dados
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseMySql(builder.Configuration.GetConnectionString("Default"),
        new MySqlServerVersion(new Version()));
});

// Dependencia hash.
builder.Services.AddScoped<IPasswordServices, PasswordServices>();

// Configuração de Tokens.
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddScoped<ITokenServices, TokenServices>();

var app = builder.Build();


// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
