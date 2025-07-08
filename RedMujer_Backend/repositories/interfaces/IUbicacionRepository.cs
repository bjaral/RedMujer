using RedMujer_Backend.models;
using RedMujer_Backend.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public interface IUbicacionRepository
    {
        Task<IEnumerable<Ubicacion>> GetAllAsync();
        Task<Ubicacion?> GetByIdAsync(int id);
        Task<int> InsertAsync(Ubicacion ubicacion);
        Task UpdateAsync(Ubicacion ubicacion);
        Task DeleteAsync(int id); // Soft-delete (vigencia)

        Task<IEnumerable<UbicacionDto>> GetUbicacionesPorEmprendimientoAsync(int id_Emprendimiento);
    }
}
