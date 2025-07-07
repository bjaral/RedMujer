using RedMujer_Backend.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public interface IContactoRepository
    {
        Task<IEnumerable<Contacto>> GetAllAsync();
        Task<Contacto?> GetByIdAsync(int id);
        Task<int> InsertAsync(Contacto contacto);
        Task UpdateAsync(Contacto contacto);
        Task DeleteAsync(int id);
    }
}
