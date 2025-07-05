using Microsoft.AspNetCore.Mvc;
using RedMujer_Backend.DTOs;
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
        public async Task<ActionResult<IEnumerable<Usuario>>> GetAll()
        {
            var usuarios = await _service.GetAllAsync();
            foreach (var user in usuarios)
                user.Contrasenna = "";
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetById(int id)
        {
            var usuario = await _service.GetByIdAsync(id);
            if (usuario == null) return NotFound();
            usuario.Contrasenna = "";
            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult> Crear([FromBody] UsuarioDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = new Usuario
            {
                UsuarioNombre = dto.Usuario,
                Contrasenna = dto.Contrasenna, // <-- SOLO password plano
                Vigencia = dto.Vigencia,
                Tipo_Usuario = Enum.Parse<TipoUsuario>(dto.Tipo_Usuario, true),
                Correo = dto.Correo
            };

            var id = await _service.CrearAsync(usuario);

            usuario.Contrasenna = "";

            return Ok(new { id });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Actualizar(int id, [FromForm] UsuarioDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existente = await _service.GetByIdAsync(id);
            if (existente == null)
                return NotFound();

            var usuario = new Usuario
            {
                Id_Usuario = id,
                UsuarioNombre = dto.Usuario,
                Contrasenna = string.IsNullOrWhiteSpace(dto.Contrasenna)
                    ? existente.Contrasenna
                    : dto.Contrasenna, // <-- password plano, servicio lo hashea
                Vigencia = dto.Vigencia,
                Tipo_Usuario = Enum.Parse<TipoUsuario>(dto.Tipo_Usuario, true),
                Correo = dto.Correo
            };

            await _service.ActualizarAsync(id, usuario);
            return Ok(new { mensaje = "Usuario actualizado correctamente" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            await _service.EliminarAsync(id);
            return Ok(new { mensaje = "Usuario eliminado correctamente" });
        }
    }
}
