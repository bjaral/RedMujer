using RedMujer_Backend.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public interface IContactoService
    {
        Task<IEnumerable<ContactoDto>> GetAllAsync();
        Task<ContactoDto?> GetByIdAsync(int id);
        Task<int> CrearAsync(ContactoCreateDto dto);
        Task ActualizarAsync(int id, ContactoCreateDto dto);
        Task EliminarAsync(int id);
        Task<IEnumerable<ContactoDto>> GetContactosPorEmprendimientoAsync(int idEmprendimiento);

    }
}
