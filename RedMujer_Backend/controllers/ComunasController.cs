using Microsoft.AspNetCore.Mvc;
using RedMujer_Backend.DTOs;
using RedMujer_Backend.services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

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

        // GET: api/comunas
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.GetAllAsync());

        // GET: api/comunas/{id_region}/{id_comuna}
        [HttpGet("{id_region:int}/{id_comuna:int}")]
        public async Task<IActionResult> Get(int id_region, int id_comuna)
        {
            var comuna = await _service.GetByIdsAsync(id_region, id_comuna);
            if (comuna == null) return NotFound();
            return Ok(comuna);
        }

        // GET: api/comunas/region/{id_region}
        [HttpGet("region/{id_region:int}")]
        public async Task<IActionResult> GetByRegion(int id_region)
        {
            var comunas = await _service.ObtenerComunasPorRegionAsync(id_region);
            return Ok(comunas);
        }

        [HttpGet("{id_comuna:int}")]
        public async Task<IActionResult> GetById(int id_comuna)
        {
            var comuna = await _service.GetByIdAsync(id_comuna);
            if (comuna == null) return NotFound();
            return Ok(comuna);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ComunaDto dto)
        {
            await _service.CrearAsync(dto);
            return Ok();
        }

        [HttpPut("{id_comuna:int}")]
        public async Task<IActionResult> Put(int id_comuna, [FromBody] ComunaDto dto)
        {
            await _service.ActualizarAsync(id_comuna, dto);
            return Ok();
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("{id_comuna:int}")]
        public async Task<IActionResult> Delete(int id_comuna)
        {
            await _service.EliminarAsync(id_comuna);
            return NoContent();
        }
    }
}
