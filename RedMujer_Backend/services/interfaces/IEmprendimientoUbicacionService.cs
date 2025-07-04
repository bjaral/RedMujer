using RedMujer_Backend.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public interface IEmprendimientoUbicacionService
    {
        Task<IEnumerable<EmprendimientoUbicacionDto>> GetAllAsync();
        Task<EmprendimientoUbicacionDto?> GetByIdsAsync(int idEmprendimiento, int idUbicacion);
        Task CrearAsync(EmprendimientoUbicacionDto dto);
        Task EliminarAsync(int idEmprendimiento, int idUbicacion);
    }
}
