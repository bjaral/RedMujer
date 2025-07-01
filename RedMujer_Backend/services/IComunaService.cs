using RedMujer_Backend.DTOs;
using RedMujer_Backend.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public interface IComunaService
    {
        Task<IEnumerable<Comuna>> GetAllAsync();
        Task<Comuna?> GetByIdsAsync(int id_region, int id_comuna);

        Task CrearAsync(ComunaDto dto);
        Task ActualizarAsync(int id, ComunaDto dto);
        Task EliminarAsync(int id);
    }
}
