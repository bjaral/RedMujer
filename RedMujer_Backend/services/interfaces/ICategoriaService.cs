using RedMujer_Backend.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaDto>> GetAllAsync();
        Task<CategoriaDto?> GetByIdAsync(int id);
        Task CrearAsync(CategoriaDto dto);
        Task ActualizarAsync(int id, CategoriaDto dto);
        Task EliminarAsync(int id);
    }
}
