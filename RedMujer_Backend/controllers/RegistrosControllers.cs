using Microsoft.AspNetCore.Mvc;
using RedMujer_Backend.DTOs;
using RedMujer_Backend.services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RedMujer_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrosController : ControllerBase
    {
        private readonly IRegistroService _service;
        public RegistrosController(IRegistroService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get() 
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var registro = await _service.GetByIdAsync(id);
            if (registro == null)
                return NotFound(new { error = "Registro no encontrado" });
            return Ok(registro);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegistroDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _service.CrearAsync(dto);
                return Ok(new { mensaje = "Registro creado correctamente" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] RegistroDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _service.ActualizarAsync(id, dto);
                return Ok(new { mensaje = "Registro actualizado correctamente" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.EliminarAsync(id);
                return Ok(new { mensaje = "Registro eliminado correctamente" });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
