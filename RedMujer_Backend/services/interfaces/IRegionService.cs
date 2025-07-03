using RedMujer_Backend.DTOs;
using RedMujer_Backend.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public interface IRegionService
    {
        Task<IEnumerable<Region>> GetAllAsync();
        Task<Region?> GetByIdAsync(int id);
        Task CrearAsync(RegionDto dto);
        Task ActualizarAsync(int id, RegionDto dto);
        Task EliminarAsync(int id);
    }
}
