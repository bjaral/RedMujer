using Microsoft.AspNetCore.Mvc;
using RedMujer_Backend.DTOs;
using RedMujer_Backend.services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


namespace RedMujer_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonaEmprendimientoController : ControllerBase
    {
        private readonly IPersonaEmprendimientoService _service;

        public PersonaEmprendimientoController(IPersonaEmprendimientoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.GetAllAsync());

        [HttpGet("{idPersona}/{idEmprendimiento}")]
        public async Task<IActionResult> Get(int idPersona, int idEmprendimiento)
        {
            var item = await _service.GetByIdsAsync(idPersona, idEmprendimiento);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PersonaEmprendimientoDto dto)
        {
            await _service.CrearAsync(dto);
            return Ok();
        }
        
        [HttpDelete("{idPersona}/{idEmprendimiento}")]
        public async Task<IActionResult> Delete(int idPersona, int idEmprendimiento)
        {
            await _service.EliminarAsync(idPersona, idEmprendimiento);
            return NoContent();
        }
    }
}
