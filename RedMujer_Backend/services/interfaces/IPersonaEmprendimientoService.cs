using RedMujer_Backend.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public interface IPersonaEmprendimientoService
    {
        Task<IEnumerable<PersonaEmprendimientoDto>> GetAllAsync();
        Task<PersonaEmprendimientoDto?> GetByIdsAsync(int idPersona, int idEmprendimiento);
        Task CrearAsync(PersonaEmprendimientoDto dto);
        Task EliminarAsync(int idPersona, int idEmprendimiento);
    }
}
