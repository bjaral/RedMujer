using Microsoft.AspNetCore.Mvc;
using RedMujer_Backend.DTOs;
using RedMujer_Backend.services;
using System.Threading.Tasks;

namespace RedMujer_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmprendimientoUbicacionController : ControllerBase
    {
        private readonly IEmprendimientoUbicacionService _service;

        public EmprendimientoUbicacionController(IEmprendimientoUbicacionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.GetAllAsync());

        [HttpGet("{idEmprendimiento}/{idUbicacion}")]
        public async Task<IActionResult> Get(int idEmprendimiento, int idUbicacion)
        {
            var item = await _service.GetByIdsAsync(idEmprendimiento, idUbicacion);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmprendimientoUbicacionDto dto)
        {
            await _service.CrearAsync(dto);
            return Ok();
        }

        [HttpDelete("{idEmprendimiento}/{idUbicacion}")]
        public async Task<IActionResult> Delete(int idEmprendimiento, int idUbicacion)
        {
            await _service.EliminarAsync(idEmprendimiento, idUbicacion);
            return NoContent();
        }
    }
}
