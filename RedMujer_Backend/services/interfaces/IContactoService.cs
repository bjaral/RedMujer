using RedMujer_Backend.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public interface IContactoService
    {
        Task<IEnumerable<ContactoDto>> GetAllAsync();
        Task<ContactoDto?> GetByIdAsync(int id);
        Task CrearAsync(ContactoDto dto);
        Task ActualizarAsync(int id, ContactoDto dto);
        Task EliminarAsync(int id);
    }
}
