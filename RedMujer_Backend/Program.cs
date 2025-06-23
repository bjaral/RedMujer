using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RedMujer_Backend.repositories;
using RedMujer_Backend.services;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios

builder.Services.AddControllers();

// Agregar Swagger (OpenAPI) para documentación
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyección de dependencias para repositorios y servicios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddScoped<IEmprendimientoRepository, EmprendimientoRepository>();
builder.Services.AddScoped<IEmprendimientoService, EmprendimientoService>();

var app = builder.Build();

// Middleware

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
