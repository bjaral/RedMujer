using RedMujer_Backend.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public interface IRegistroRepository
    {
        Task<IEnumerable<Registro>> GetAllAsync();
        Task<Registro?> GetByIdAsync(int id);
        Task InsertAsync(Registro registro);
        Task UpdateAsync(Registro registro);
        Task DeleteAsync(int id);
    }
}
