using Microsoft.AspNetCore.Mvc;
using RedMujer_Backend.DTOs;
using RedMujer_Backend.services;
using System.Threading.Tasks;

namespace RedMujer_Backend.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComunasController : ControllerBase
    {
        private readonly IComunaService _service;
        public ComunasController(IComunaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.GetAllAsync());
        [HttpGet("{id_region}/{id_comuna}")]
        public async Task<IActionResult> Get(int id_region, int id_comuna)
        {
            var comuna = await _service.GetByIdsAsync(id_region, id_comuna);
            if (comuna == null) return NotFound();
            return Ok(comuna);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ComunaDto dto)
        {
            await _service.CrearAsync(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ComunaDto dto)
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
