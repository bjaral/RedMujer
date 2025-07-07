using Microsoft.AspNetCore.Mvc;
using RedMujer_Backend.DTOs;
using RedMujer_Backend.services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using RedMujer_Backend.models;

namespace RedMujer_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactosController : ControllerBase
    {
        private readonly IContactoService _service;
        public ContactosController(IContactoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.GetAllAsync());

        [HttpGet("tipos_contacto")]
        public ActionResult<IEnumerable<string>> GetTiposContacto()
        {
            var tipos = Enum.GetNames(typeof(Contacto.TipoContacto));
            return Ok(tipos);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var contacto = await _service.GetByIdAsync(id);
            if (contacto == null) return NotFound();
            return Ok(contacto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ContactoCreateDto dto)
        {
            await _service.CrearAsync(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ContactoCreateDto dto)
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
    }
}
