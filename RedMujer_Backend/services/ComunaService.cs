using RedMujer_Backend.DTOs;
using RedMujer_Backend.models;
using RedMujer_Backend.repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public class ComunaService : IComunaService
    {
        private readonly IComunaRepository _repo;

        public ComunaService(IComunaRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ComunaDto>> GetAllAsync()
        {
            var comunas = await _repo.GetAllAsync();
            return comunas.Select(c => new ComunaDto
            {
                Id_Comuna = c.Id_Comuna,
                Id_Region = c.Id_Region,
                Nombre = c.Nombre,
                Vigencia = c.Vigencia
            });
        }

        public async Task<ComunaDto?> GetByIdsAsync(int idRegion, int idComuna)
        {
            var comuna = await _repo.GetByIdsAsync(idRegion, idComuna);
            if (comuna == null) return null;
            return new ComunaDto
            {
                Id_Comuna = comuna.Id_Comuna,
                Id_Region = comuna.Id_Region,
                Nombre = comuna.Nombre,
                Vigencia = comuna.Vigencia
            };
        }

        public async Task<ComunaDto?> GetByIdAsync(int idComuna)
        {
            var comuna = await _repo.GetByIdAsync(idComuna);
            if (comuna == null) return null;
            return new ComunaDto
            {
                Id_Comuna = comuna.Id_Comuna,
                Id_Region = comuna.Id_Region,
                Nombre = comuna.Nombre,
                Vigencia = comuna.Vigencia
            };
        }

        public async Task<IEnumerable<ComunaDto>> ObtenerComunasPorRegionAsync(int idRegion)
        {
            var comunas = await _repo.ObtenerComunasPorRegionAsync(idRegion);
            return comunas.Select(c => new ComunaDto
            {
                Id_Comuna = c.Id_Comuna,
                Id_Region = c.Id_Region,
                Nombre = c.Nombre,
                Vigencia = c.Vigencia
            });
        }

        public async Task CrearAsync(ComunaCreateDto dto)
        {
            var comuna = new Comuna
            {
                Id_Region = dto.Id_Region,
                Nombre = dto.Nombre,
                Vigencia = dto.Vigencia
            };
            await _repo.CrearAsync(comuna);
        }

        public async Task ActualizarAsync(int idComuna, ComunaCreateDto dto)
        {
            var comuna = new Comuna
            {
                Id_Comuna = idComuna,
                Id_Region = dto.Id_Region,
                Nombre = dto.Nombre,
                Vigencia = dto.Vigencia
            };
            await _repo.ActualizarAsync(comuna);
        }

        public async Task EliminarAsync(int idComuna)
        {
            await _repo.EliminarAsync(idComuna);
        }
    }
}
