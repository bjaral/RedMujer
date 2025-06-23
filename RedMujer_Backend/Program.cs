using RedMujer_Backend.repositories;
using RedMujer_Backend.services;

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependencias (Inyección de dependencias)
builder.Services.AddScoped<IEmprendimientoRepository, EmprendimientoRepository>();
builder.Services.AddScoped<IEmprendimientoService, EmprendimientoService>();

// CORS para Angular frontend en localhost:4200
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
