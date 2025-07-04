using RedMujer_Backend.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public interface IRegistroService
    {
        Task<IEnumerable<RegistroDto>> GetAllAsync();
        Task<RegistroDto?> GetByIdAsync(int id);
        Task CrearAsync(RegistroDto dto);
        Task ActualizarAsync(int id, RegistroDto dto);
        Task EliminarAsync(int id);
    }
}
