using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RedMujer_Backend.DTOs;
using RedMujer_Backend.services;
using RedMujer_Backend.models;

namespace RedMujer_Backend.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriasController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        // GET: /api/categorias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetAll()
        {
            var categorias = await _categoriaService.GetAllAsync();
            return Ok(categorias);
        }

        // GET: /api/categorias/grupos
        [HttpGet("grupos")]
        public ActionResult<IEnumerable<string>> GetGruposCategoria()
        {
            var grupos = Enum.GetNames(typeof(GrupoCategoria));
            return Ok(grupos);
        }

        // GET: /api/categorias/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDto>> GetById(int id)
        {
            var categoria = await _categoriaService.GetByIdAsync(id);
            if (categoria == null)
                return NotFound();
            return Ok(categoria);
        }

        // POST: /api/categorias
        [HttpPost]
        public async Task<IActionResult> Crear(CategoriaCreateDto dto)
        {
            await _categoriaService.CrearAsync(dto);
            return Ok();
        }

        // PUT: /api/categorias/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Actualizar(int id, [FromBody] CategoriaCreateDto dto)
        {
            await _categoriaService.ActualizarAsync(id, dto);
            return NoContent();
        }

        // DELETE: /api/categorias/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Eliminar(int id)
        {
            await _categoriaService.EliminarAsync(id);
            return NoContent();
        }
    }
}