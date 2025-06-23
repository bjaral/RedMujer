using RedMujer_Backend.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public interface IEmprendimientoRepository
    {
        Task<IEnumerable<Emprendimiento>> GetAllAsync();
        Task<IEnumerable<Emprendimiento>> GetRandomAsync(int cantidad);
        Task InsertarEmprendimientoAsync(Emprendimiento e);
    }
}
