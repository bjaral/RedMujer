using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RedMujer_Backend.repositories;
using RedMujer_Backend.DTOs;
using RedMujer_Backend.models;

namespace RedMujer_Backend.services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        // GET todos
        public async Task<IEnumerable<CategoriaDto>> GetAllAsync()
        {
            var categorias = await _categoriaRepository.GetAllAsync();
            return categorias.Select(c => new CategoriaDto
            {
                Id_Categoria = c.Id_Categoria,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
                Vigencia = c.Vigencia,
                Grupo_Categoria = c.Grupo_Categoria
            });
        }

        // GET por id
        public async Task<CategoriaDto?> GetByIdAsync(int id)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            if (categoria == null)
                return null;

            return new CategoriaDto
            {
                Id_Categoria = categoria.Id_Categoria,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Vigencia = categoria.Vigencia,
                Grupo_Categoria = categoria.Grupo_Categoria
            };
        }

        // Crear
        public async Task CrearAsync(CategoriaCreateDto dto)
        {
            var categoria = new Categoria
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Vigencia = dto.Vigencia,
                Grupo_Categoria = dto.Grupo_Categoria
            };
            await _categoriaRepository.CrearAsync(categoria);
        }


        // Actualizar
        public async Task ActualizarAsync(int id, CategoriaCreateDto dto)
        {
            var categoria = new Categoria
            {
                Id_Categoria = id,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Vigencia = dto.Vigencia,
                Grupo_Categoria = dto.Grupo_Categoria
            };
            await _categoriaRepository.ActualizarAsync(categoria);
        }

        // Eliminar
        public async Task EliminarAsync(int id)
        {
            await _categoriaRepository.EliminarAsync(id);
        }

        // Obtener categorías por emprendimiento
        public async Task<IEnumerable<CategoriaDto>> ObtenerCategoriasPorEmprendimientoAsync(int idEmprendimiento)
        {
            var categorias = await _categoriaRepository.GetCategoriasPorEmprendimientoAsync(idEmprendimiento);
            return categorias.Select(c => new CategoriaDto
            {
                Id_Categoria = c.Id_Categoria,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
                Vigencia = c.Vigencia,
                Grupo_Categoria = c.Grupo_Categoria
            });
        }
    }
}
