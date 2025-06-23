using Microsoft.AspNetCore.Mvc;
using RedMujer_Backend.DTOs;
using RedMujer_Backend.services;
using System.Threading.Tasks;

namespace RedMujer_Backend.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmprendimientosController : ControllerBase
    {
        private readonly IEmprendimientoService _service;

        public EmprendimientosController(IEmprendimientoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var lista = await _service.GetAllAsync();
            return Ok(lista);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] EmprendimientoDto dto)
        {
            await _service.ActualizarAsync(id, dto);
            return NoContent();
        }

        [HttpGet("random/{cantidad}")]
        public async Task<IActionResult> GetRandom(int cantidad)
        {
            var lista = await _service.GetRandomAsync(cantidad);
            return Ok(lista);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _service.EliminarAsync(id);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] EmprendimientoDto dto)
        {
            var nuevo = await _service.CrearAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { id = nuevo.Id_Emprendimiento }, nuevo);
        }
    }
}
