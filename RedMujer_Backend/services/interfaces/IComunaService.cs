using RedMujer_Backend.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public interface IComunaService
    {
        Task<IEnumerable<ComunaDto>> GetAllAsync();
        Task<ComunaDto?> GetByIdsAsync(int idRegion, int idComuna); // buscar por id de region + id de comuna
        Task<ComunaDto?> GetByIdAsync(int idComuna); // buscar solo por id de comuna
        Task<IEnumerable<ComunaDto>> ObtenerComunasPorRegionAsync(int idRegion); // comunas de una región
        Task CrearAsync(ComunaDto dto);
        Task ActualizarAsync(int idComuna, ComunaDto dto);
        Task EliminarAsync(int idComuna);
    }
}
