using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RedMujer_Backend.repositories;
using RedMujer_Backend.services;

var builder = WebApplication.CreateBuilder(args);

// JWT Authentication con parámetros desde appsettings.json
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

// Swagger + JWT Bearer (botón Authorize)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RedMujer API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header usando el esquema Bearer.  
                        Escribe 'Bearer' [espacio] y tu token en la caja de texto.  
                        Ejemplo: 'Bearer eyJhbGciOi...'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// CORS para Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Inyección de dependencias para repositorios y servicios (tu listado original)
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddScoped<IEmprendimientoRepository, EmprendimientoRepository>();
builder.Services.AddScoped<IEmprendimientoService, EmprendimientoService>();

builder.Services.AddScoped<IPlataformaRepository, PlataformaRepository>();
builder.Services.AddScoped<PlataformaService>();

builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IRegionService, RegionService>();

builder.Services.AddScoped<IComunaRepository, ComunaRepository>();
builder.Services.AddScoped<IComunaService, ComunaService>();
builder.Services.AddScoped<IMultimediaRepository, MultimediaRepository>();
builder.Services.AddScoped<IMultimediaService, MultimediaService>();
builder.Services.AddScoped<IUbicacionRepository, UbicacionRepository>();
builder.Services.AddScoped<IUbicacionService, UbicacionService>();

builder.Services.AddScoped<IPersonaRepository, PersonaRepository>();
builder.Services.AddScoped<IPersonaService, PersonaService>();

builder.Services.AddScoped<IContactoRepository, ContactoRepository>();
builder.Services.AddScoped<IContactoService, ContactoService>();

builder.Services.AddScoped<IRegistroRepository, RegistroRepository>();
builder.Services.AddScoped<IRegistroService, RegistroService>();

builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();

builder.Services.AddScoped<IEmprendimientoCategoriaRepository, EmprendimientoCategoriaRepository>();
builder.Services.AddScoped<IEmprendimientoCategoriaService, EmprendimientoCategoriaService>();

builder.Services.AddScoped<IEmprendimientoUbicacionRepository, EmprendimientoUbicacionRepository>();
builder.Services.AddScoped<IEmprendimientoUbicacionService, EmprendimientoUbicacionService>();

builder.Services.AddScoped<IPersonaEmprendimientoRepository, PersonaEmprendimientoRepository>();
builder.Services.AddScoped<IPersonaEmprendimientoService, PersonaEmprendimientoService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Servir archivos estáticos desde /media
var mediaPath = Path.Combine(Directory.GetCurrentDirectory(), "media");
if (!Directory.Exists(mediaPath))
{
    Directory.CreateDirectory(mediaPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(mediaPath),
    RequestPath = "/media"
});

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
