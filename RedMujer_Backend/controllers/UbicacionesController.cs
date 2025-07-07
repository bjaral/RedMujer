using Microsoft.AspNetCore.Mvc;
using RedMujer_Backend.DTOs;
using RedMujer_Backend.services;
using System.Threading.Tasks;
using RedMujer_Backend.models;
using Microsoft.AspNetCore.Authorization;


namespace RedMujer_Backend.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UbicacionesController : ControllerBase
    {
        private readonly IUbicacionService _service;
        public UbicacionesController(IUbicacionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var ubicacion = await _service.GetByIdAsync(id);
            if (ubicacion == null) return NotFound();
            return Ok(ubicacion);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UbicacionCreateDto dto)
        {
            var id = await _service.CrearAsync(dto);
            return Ok(new { id });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UbicacionCreateDto dto)
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
