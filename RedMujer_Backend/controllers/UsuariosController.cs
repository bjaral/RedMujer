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
                Contrasenna = BCrypt.Net.BCrypt.HashPassword(dto.Contrasenna),
                Vigencia = dto.Vigencia,
                Tipo_Usuario = Enum.Parse<TipoUsuario>(dto.Tipo_Usuario, true),
                Correo = dto.Correo
            };

<<<<<<< HEAD
            var usuarioGuardado = await _service.CrearAsync(usuario);
=======
            var id = await _service.CrearAsync(usuario);
>>>>>>> 848ffadbbde22cf0434e7a5441d434c236e670d5

            usuarioGuardado.Contrasenna = "";

<<<<<<< HEAD
            // Puedes mapear de regreso a un DTO si quieres esconder info sensible
            return Ok(usuarioGuardado); // Aquí sí sale el Id_Usuario
=======
            return Ok(new { id });
>>>>>>> 848ffadbbde22cf0434e7a5441d434c236e670d5
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
                    : BCrypt.Net.BCrypt.HashPassword(dto.Contrasenna),
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
