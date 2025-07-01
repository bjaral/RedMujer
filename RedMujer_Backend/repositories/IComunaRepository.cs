using RedMujer_Backend.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public interface IComunaRepository
    {
        Task<IEnumerable<Comuna>> GetAllAsync();
        Task<Comuna?> GetByIdsAsync(int id_region, int id_comuna);

        Task InsertAsync(Comuna comuna);
        Task UpdateAsync(Comuna comuna);
        Task DeleteAsync(int id); // Soft-delete (vigencia)
    }
}
    