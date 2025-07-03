using RedMujer_Backend.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public interface IEmprendimientoCategoriaRepository
    {
        Task<IEnumerable<EmprendimientoCategoria>> GetAllAsync();
        Task<EmprendimientoCategoria?> GetByIdsAsync(int idEmprendimiento, int idCategoria);
        Task InsertAsync(EmprendimientoCategoria ec);
        Task DeleteAsync(int idEmprendimiento, int idCategoria);
    }
}
