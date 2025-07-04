using RedMujer_Backend.DTOs;
using RedMujer_Backend.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public interface IUbicacionService
    {
        Task<IEnumerable<Ubicacion>> GetAllAsync();
        Task<Ubicacion?> GetByIdAsync(int id);
        Task<int> CrearAsync(UbicacionDto dto);
        Task ActualizarAsync(int id, UbicacionDto dto);
        Task EliminarAsync(int id);
    }
}
