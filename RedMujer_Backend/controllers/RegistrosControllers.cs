using Microsoft.AspNetCore.Mvc;
using RedMujer_Backend.DTOs;
using RedMujer_Backend.services;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Get() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var registro = await _service.GetByIdAsync(id);
            if (registro == null) return NotFound();
            return Ok(registro);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegistroDto dto)
        {
            await _service.CrearAsync(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] RegistroDto dto)
        {
            await _service.ActualizarAsync(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.EliminarAsync(id);
            return NoContent();
        }
    }
}
