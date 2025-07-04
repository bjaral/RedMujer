using RedMujer_Backend.DTOs;
using RedMujer_Backend.models;
using RedMujer_Backend.repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repo;

        public CategoriaService(ICategoriaRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<CategoriaDto>> GetAllAsync()
        {
            var categorias = await _repo.GetAllAsync();
            return categorias.Select(c => new CategoriaDto
            {
                Id_Categoria = c.Id_Categoria,
                Descripcion = c.Descripcion,
                Vigencia = c.Vigencia,
                Grupo_Categoria = c.Grupo_Categoria
            });
        }

        public async Task<CategoriaDto?> GetByIdAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return null;
            return new CategoriaDto
            {
                Id_Categoria = c.Id_Categoria,
                Descripcion = c.Descripcion,
                Vigencia = c.Vigencia,
                Grupo_Categoria = c.Grupo_Categoria
            };
        }

        public async Task CrearAsync(CategoriaDto dto)
        {
            var categoria = new Categoria
            {
                Descripcion = dto.Descripcion,
                Vigencia = dto.Vigencia,
                Grupo_Categoria = dto.Grupo_Categoria
            };
            await _repo.CrearAsync(categoria);
        }

        public async Task ActualizarAsync(int id, CategoriaDto dto)
        {
            var categoria = await _repo.GetByIdAsync(id);
            if (categoria != null)
            {
                categoria.Descripcion = dto.Descripcion;
                categoria.Vigencia = dto.Vigencia;
                categoria.Grupo_Categoria = dto.Grupo_Categoria;
                await _repo.ActualizarAsync(categoria);
            }
        }

        public async Task EliminarAsync(int id)
        {
            await _repo.EliminarAsync(id);
        }
    }
}
