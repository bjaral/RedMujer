using Microsoft.AspNetCore.Mvc;
using RedMujer_Backend.DTOs;
using RedMujer_Backend.services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RedMujer_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonasController : ControllerBase
    {
        private readonly IPersonaService _service;
        public PersonasController(IPersonaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var persona = await _service.GetByIdAsync(id);
            if (persona == null) return NotFound();
            return Ok(persona);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PersonaDto dto)
        {
            await _service.CrearAsync(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PersonaDto dto)
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
