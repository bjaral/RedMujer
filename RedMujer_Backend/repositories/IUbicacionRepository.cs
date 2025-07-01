using RedMujer_Backend.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public interface IUbicacionRepository
    {
        Task<IEnumerable<Ubicacion>> GetAllAsync();
        Task<Ubicacion?> GetByIdAsync(int id);
        Task InsertAsync(Ubicacion ubicacion);
        Task UpdateAsync(Ubicacion ubicacion);
        Task DeleteAsync(int id); // Soft-delete (vigencia)
    }
}
