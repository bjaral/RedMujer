using RedMujer_Backend.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public interface IPersonaService
    {
        Task<IEnumerable<PersonaDto>> GetAllAsync();
        Task<PersonaDto?> GetByIdAsync(int id);
        Task CrearAsync(PersonaDto dto);
        Task ActualizarAsync(int id, PersonaDto dto);
        Task EliminarAsync(int id);
    }
}
