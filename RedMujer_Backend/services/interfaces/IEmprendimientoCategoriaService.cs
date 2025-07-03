using RedMujer_Backend.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public interface IEmprendimientoCategoriaService
    {
        Task<IEnumerable<EmprendimientoCategoriaDto>> GetAllAsync();
        Task<EmprendimientoCategoriaDto?> GetByIdsAsync(int idEmprendimiento, int idCategoria);
        Task CrearAsync(EmprendimientoCategoriaDto dto);
        Task EliminarAsync(int idEmprendimiento, int idCategoria);
    }
}
