using Microsoft.AspNetCore.Mvc;
using RedMujer_Backend.DTOs;
using RedMujer_Backend.services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


namespace RedMujer_Backend.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegionesController : ControllerBase
    {
        private readonly IRegionService _service;
        public RegionesController(IRegionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var region = await _service.GetByIdAsync(id);
            if (region == null) return NotFound();
            return Ok(region);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegionCreateDto dto)
        {
            await _service.CrearAsync(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] RegionCreateDto dto)
        {
            await _service.ActualizarAsync(id, dto);
            return Ok();
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.EliminarAsync(id);
            return NoContent();
        }
    }
}
