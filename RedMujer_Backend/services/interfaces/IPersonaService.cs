using RedMujer_Backend.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public interface IPersonaService
    {
        Task<IEnumerable<PersonaDto>> GetAllAsync();
        Task<PersonaDto?> GetByIdAsync(int id);
        Task<int> CrearAsync(PersonaCreateDto dto);
        Task ActualizarAsync(int id, PersonaCreateDto dto);
        Task EliminarAsync(int id);
    }
}
