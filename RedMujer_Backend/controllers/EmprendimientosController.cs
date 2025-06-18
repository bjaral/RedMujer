using Microsoft.AspNetCore.Mvc;
using RedMujer_Backend.services;

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
            var emprendimientos = await _service.GetAllAsync();
            return Ok(emprendimientos);
        }

        [HttpGet("random/{cantidad}")]
        public async Task<IActionResult> GetRandom(int cantidad)
        {
            var emprendimientos = await _service.GetRandomAsync(cantidad);
            return Ok(emprendimientos);
        }
    }
}
