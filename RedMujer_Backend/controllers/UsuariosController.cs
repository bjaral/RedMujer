using Microsoft.AspNetCore.Mvc;
using RedMujer_Backend.models;
using RedMujer_Backend.services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuariosController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Usuario>> GetAll()
        {
            return await _service.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetById(int id)
        {
            var usuario = await _service.GetByIdAsync(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> Create([FromBody] Usuario usuario)
        {
            var nuevoUsuario = await _service.CrearAsync(usuario);
            return CreatedAtAction(nameof(GetById), new { id = nuevoUsuario.Id_Usuario }, nuevoUsuario);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Usuario usuario)
        {
            var existente = await _service.GetByIdAsync(id);
            if (existente == null) return NotFound();

            await _service.ActualizarAsync(id, usuario);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existente = await _service.GetByIdAsync(id);
            if (existente == null) return NotFound();

            await _service.EliminarAsync(id);
            return NoContent();
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<Usuario>> Authenticate([FromBody] LoginRequest login)
        {
            var usuario = await _service.AuthenticateAsync(login.UsuarioNombre, login.Contrasenna);
            if (usuario == null) return Unauthorized();

            return Ok(usuario);
        }
    }

    public class LoginRequest
    {
        public string UsuarioNombre { get; set; } = string.Empty;
        public string Contrasenna { get; set; } = string.Empty;
    }
}
