using RedMujer_Backend.DTOs;
using RedMujer_Backend.models;
using RedMujer_Backend.repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public class UbicacionService : IUbicacionService
    {
        private readonly IUbicacionRepository _repo;
        private readonly IComunaRepository _comunaRepo; // <-- INYECTA EL REPO DE COMUNAS

        public UbicacionService(IUbicacionRepository repo, IComunaRepository comunaRepo)
        {
            _repo = repo;
            _comunaRepo = comunaRepo;
        }

        public async Task<IEnumerable<Ubicacion>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<Ubicacion?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task<int> CrearAsync(UbicacionCreateDto dto)
        {
            var comuna = await _comunaRepo.GetByIdsAsync(dto.Id_Region, dto.Id_Comuna);
            if (comuna == null)
                throw new System.Exception("La comuna no pertenece a la región indicada.");

            var ubicacion = new Ubicacion
            {
                Id_Comuna = dto.Id_Comuna,
                Calle = dto.Calle,
                Numero = dto.Numero,
                Referencia = dto.Referencia,
                Vigencia = dto.Vigencia
            };
            return await _repo.InsertAsync(ubicacion);
        }

        public async Task ActualizarAsync(int id, UbicacionCreateDto dto)
        {
            // Puedes repetir la validación aquí si tiene sentido para tu negocio
            var comuna = await _comunaRepo.GetByIdsAsync(dto.Id_Region, dto.Id_Comuna);
            if (comuna == null)
                throw new System.Exception("La comuna no pertenece a la región indicada.");

            var ubicacion = new Ubicacion
            {
                Id_Ubicacion = id,
                Id_Comuna = dto.Id_Comuna,
                Calle = dto.Calle,
                Numero = dto.Numero,
                Referencia = dto.Referencia,
                Vigencia = dto.Vigencia
            };
            await _repo.UpdateAsync(ubicacion);
        }

        public async Task EliminarAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
