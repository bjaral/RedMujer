using RedMujer_Backend.DTOs;
using RedMujer_Backend.models;
using RedMujer_Backend.repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public class RegionService : IRegionService
    {
        private readonly IRegionRepository _repo;
        public RegionService(IRegionRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Region>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<Region?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task CrearAsync(RegionDto dto)
        {
            var region = new Region
            {
                Nombre = dto.Nombre,
                Vigencia = dto.Vigencia
            };
            await _repo.InsertAsync(region);
        }

        public async Task ActualizarAsync(int id, RegionDto dto)
        {
            var region = new Region
            {
                Id_Region = id,
                Nombre = dto.Nombre,
                Vigencia = dto.Vigencia
            };
            await _repo.UpdateAsync(region);
        }

        public async Task EliminarAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
