using RedMujer_Backend.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaDto>> GetAllAsync();
        Task<CategoriaDto?> GetByIdAsync(int id);
        Task CrearAsync(CategoriaCreateDto dto);
        Task ActualizarAsync(int id, CategoriaCreateDto dto);
        Task EliminarAsync(int id);
        Task<IEnumerable<CategoriaDto>> ObtenerCategoriasPorEmprendimientoAsync(int idEmprendimiento);
    }
}
