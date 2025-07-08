using RedMujer_Backend.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public interface IPersonaEmprendimientoRepository
    {
        Task<IEnumerable<PersonaEmprendimiento>> GetAllAsync();
        Task<PersonaEmprendimiento?> GetByIdsAsync(int idPersona, int idEmprendimiento);
        Task InsertAsync(PersonaEmprendimiento pe);
        Task DeleteAsync(int idPersona, int idEmprendimiento);
    }
}
