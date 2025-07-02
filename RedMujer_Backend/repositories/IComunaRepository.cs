using RedMujer_Backend.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.repositories
{
    public interface IComunaRepository
    {
        Task<IEnumerable<Comuna>> GetAllAsync();
        Task<Comuna?> GetByIdsAsync(int idRegion, int idComuna);
        Task<Comuna?> GetByIdAsync(int idComuna);
        Task<IEnumerable<Comuna>> ObtenerComunasPorRegionAsync(int idRegion);
        Task CrearAsync(Comuna comuna);
        Task ActualizarAsync(Comuna comuna);
        Task EliminarAsync(int idComuna);
    }
}
