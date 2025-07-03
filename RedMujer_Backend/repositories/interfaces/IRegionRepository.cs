using RedMujer_Backend.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync();
        Task<Region?> GetByIdAsync(int id);
        Task InsertAsync(Region region);
        Task UpdateAsync(Region region);
        Task DeleteAsync(int id); // Eliminado lógico (vigencia)
    }
}
