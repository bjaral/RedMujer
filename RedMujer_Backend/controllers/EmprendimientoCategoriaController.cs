using Microsoft.AspNetCore.Mvc;
using RedMujer_Backend.DTOs;
using RedMujer_Backend.services;
using System.Threading.Tasks;


namespace RedMujer_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmprendimientoCategoriaController : ControllerBase
    {
        private readonly IEmprendimientoCategoriaService _service;

        public EmprendimientoCategoriaController(IEmprendimientoCategoriaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.GetAllAsync());

        [HttpGet("{idEmprendimiento}/{idCategoria}")]
        public async Task<IActionResult> Get(int idEmprendimiento, int idCategoria)
        {
            var item = await _service.GetByIdsAsync(idEmprendimiento, idCategoria);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmprendimientoCategoriaDto dto)
        {
            await _service.CrearAsync(dto);
            return Ok();
        }

        [HttpDelete("{idEmprendimiento}/{idCategoria}")]
        public async Task<IActionResult> Delete(int idEmprendimiento, int idCategoria)
        {
            await _service.EliminarAsync(idEmprendimiento, idCategoria);
            return NoContent();
        }
    }
}
