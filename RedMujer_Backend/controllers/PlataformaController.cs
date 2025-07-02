using Microsoft.AspNetCore.Mvc;
using RedMujer_Backend.services;
using RedMujer_Backend.DTOs;

namespace RedMujer_Backend.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlataformaController : ControllerBase
    {
        private readonly PlataformaService _service;

        public PlataformaController(PlataformaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var plataformas = await _service.GetAllAsync();
            return Ok(plataformas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var plataforma = await _service.GetByIdAsync(id);
            if (plataforma == null) return NotFound();
            return Ok(plataforma);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlataformaDto dto)
        {
            await _service.AddAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = dto.Id_Plataforma }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PlataformaDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}

