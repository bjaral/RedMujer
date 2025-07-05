using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RedMujer_Backend.services;
using RedMujer_Backend.models;
using System.Threading.Tasks;

namespace RedMujer_Backend.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IConfiguration _configuration;

        public AuthController(IUsuarioService usuarioService, IConfiguration configuration)
        {
            _usuarioService = usuarioService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            Usuario? user = null;

            // Permitir login por correo O nombre de usuario
            if (!string.IsNullOrEmpty(request.Correo))
            {
                user = await _usuarioService.AuthenticateByCorreoAsync(request.Correo, request.Password);
            }
            else if (!string.IsNullOrEmpty(request.UsuarioNombre))
            {
                user = await _usuarioService.AuthenticateAsync(request.UsuarioNombre, request.Password);
            }

            if (user == null || !user.Vigencia)
                return Unauthorized("Credenciales inválidas o usuario inactivo.");

            var token = GenerateJwtToken(user);
            return Ok(new
            {
                token,
                usuario = new {
                    user.Id_Usuario,
                    user.UsuarioNombre,
                    user.Correo,
                    user.Tipo_Usuario
                }
            });
        }

        private string GenerateJwtToken(Usuario user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id_Usuario.ToString()),
                new Claim(ClaimTypes.Name, user.UsuarioNombre),
                new Claim(ClaimTypes.Email, user.Correo),
                new Claim(ClaimTypes.Role, user.Tipo_Usuario.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string? UsuarioNombre { get; set; }
        public string? Correo { get; set; }
        public required string Password { get; set; }
    }
}
