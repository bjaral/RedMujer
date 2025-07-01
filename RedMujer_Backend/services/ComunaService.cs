using RedMujer_Backend.DTOs;
using RedMujer_Backend.models;
using RedMujer_Backend.repositories;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Comuna>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<Comuna?> GetByIdsAsync(int id_region, int id_comuna)
        {
            return await _repo.GetByIdsAsync(id_region, id_comuna);
        }
        public async Task CrearAsync(ComunaDto dto)
        {
            var comuna = new Comuna
            {
                Id_Region = dto.Id_Region,
                Nombre = dto.Nombre,
                Vigencia = dto.Vigencia
            };
            await _repo.InsertAsync(comuna);
        }

        public async Task ActualizarAsync(int id, ComunaDto dto)
        {
            var comuna = new Comuna
            {
                Id_Comuna = id,
                Id_Region = dto.Id_Region,
                Nombre = dto.Nombre,
                Vigencia = dto.Vigencia
            };
            await _repo.UpdateAsync(comuna);
        }

        public async Task EliminarAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
