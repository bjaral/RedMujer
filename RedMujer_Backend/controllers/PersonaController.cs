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
        private readonly IEmprendimientoService _emprendimientoService;
        public PersonasController(IPersonaService service, IEmprendimientoService emprendimientoService)
        {
            _service = service;
            _emprendimientoService = emprendimientoService;
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
        public async Task<IActionResult> Post([FromBody] PersonaCreateDto dto)
        {
            var id = await _service.CrearAsync(dto);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PersonaCreateDto dto)
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
        [HttpGet("{idPersona}/emprendimientos")]
        public async Task<IActionResult> GetEmprendimientosPorPersona(int idPersona)
        {
            var emprendimientos = await _emprendimientoService.ObtenerPorPersonaAsync(idPersona);
            return Ok(emprendimientos);
        }
        [HttpGet("usuario/{idUsuario:int}")]
        public async Task<IActionResult> GetByUsuarioId(int idUsuario)
        {
            var persona = await _service.GetByUsuarioIdAsync(idUsuario);
            if (persona == null) return NotFound();
            return Ok(persona);
        }

    }
}
