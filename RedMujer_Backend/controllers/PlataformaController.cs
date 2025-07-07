using Microsoft.AspNetCore.Mvc;
using RedMujer_Backend.services;
using RedMujer_Backend.models;
using RedMujer_Backend.DTOs;
using Microsoft.AspNetCore.Authorization;


namespace RedMujer_Backend.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlataformaController : ControllerBase
    {
        private readonly IPlataformaService _service;

        public PlataformaController(IPlataformaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var plataformas = await _service.GetAllAsync();
            return Ok(plataformas);
        }

        [HttpGet("tipo")]
        public ActionResult<IEnumerable<string>> GetTipoPlataforma()
        {
            var tipos = Enum.GetNames(typeof(Plataforma.TipoPlataforma));
            return Ok(tipos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var plataforma = await _service.GetByIdAsync(id);
            if (plataforma == null) return NotFound();
            return Ok(plataforma);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlataformaCreateDto dto)
        {
            var id = await _service.CrearAsync(dto);
            return CreatedAtAction(nameof(Get), new { id }, new { id });
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PlataformaCreateDto dto)
        {
            await _service.ActualizarAsync(id, dto);
            return NoContent();
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}

