using RedMujer_Backend.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public interface IPlataformaRepository
    {
        Task<IEnumerable<Plataforma>> GetAllAsync();
        Task<Plataforma?> GetByIdAsync(int id);
        Task<int> InsertAsync(Plataforma plataforma);
        Task<int> UpdateAsync(Plataforma plataforma);
        Task DeleteAsync(int id); // Soft-delete (vigencia)
    }
}
