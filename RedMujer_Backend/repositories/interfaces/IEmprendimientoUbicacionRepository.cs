using RedMujer_Backend.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public interface IEmprendimientoUbicacionRepository
    {
        Task<IEnumerable<EmprendimientoUbicacion>> GetAllAsync();
        Task<EmprendimientoUbicacion?> GetByIdsAsync(int idEmprendimiento, int idUbicacion);
        Task InsertAsync(EmprendimientoUbicacion eu);
        Task DeleteAsync(int idEmprendimiento, int idUbicacion);
    }
}
