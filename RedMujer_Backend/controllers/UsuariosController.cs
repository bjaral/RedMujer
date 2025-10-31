using Microsoft.AspNetCore.Mvc;
using RedMujer_Backend.DTOs;
using RedMujer_Backend.models;
using RedMujer_Backend.services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


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

        [HttpGet("tipos")]
        public ActionResult<IEnumerable<string>> GetTiposUsuario()
        {
            var tipos = Enum.GetNames(typeof(TipoUsuario));
            return Ok(tipos);
        }



        [HttpPost]
        public async Task<ActionResult> Crear([FromBody] UsuarioDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = new Usuario
            {
                UsuarioNombre = dto.Usuario,
                Contrasenna = dto.Contrasenna,
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
                    : dto.Contrasenna,
                Vigencia = dto.Vigencia,
                Tipo_Usuario = Enum.Parse<TipoUsuario>(dto.Tipo_Usuario, true),
                Correo = dto.Correo
            };

            await _service.ActualizarAsync(id, usuario);
            return Ok(new { mensaje = "Usuario actualizado correctamente" });
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            await _service.EliminarAsync(id);
            return Ok(new { mensaje = "Usuario eliminado correctamente" });
        }

        [HttpGet("verificar-correo")]
        public async Task<ActionResult> VerificarCorreo([FromQuery] string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
                return BadRequest(new { mensaje = "El correo es requerido" });

            var usuario = await _service.GetByCorreoAsync(correo);

            return Ok(new
            {
                existe = usuario != null,
                correo = correo.Trim()
            });
        }

        [HttpPut("{id}/cambiar-contrasena")]
        public async Task<ActionResult> CambiarContrasena(int id, [FromBody] CambiarContrasenaRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _service.CambiarContrasenaAsync(id, request.ContrasenaActual, request.ContrasenaNueva);

            if (!resultado)
                return BadRequest(new { mensaje = "La contraseña actual es incorrecta" });

            return Ok(new { mensaje = "Contraseña cambiada exitosamente" });
        }

        public class CambiarContrasenaRequest
        {
            public required string ContrasenaActual { get; set; }
            public required string ContrasenaNueva { get; set; }
        }
    }
}
