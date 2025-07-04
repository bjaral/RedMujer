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

        public async Task CrearAsync(UbicacionDto dto)
        {
            // 1. Valida que la comuna existe y pertenece a la región indicada
            var comuna = await _comunaRepo.GetByIdsAsync(dto.Id_Region, dto.id_comuna);
            if (comuna == null)
                throw new System.Exception("La comuna no pertenece a la región indicada.");

            var ubicacion = new Ubicacion
            {
                Id_comuna = dto.id_comuna,
                Id_Emprendimiento = dto.Id_Emprendimiento,
                Calle = dto.Calle,
                Numero = dto.Numero,
                Referencia = dto.Referencia,
                Vigencia = dto.Vigencia
            };
            await _repo.InsertAsync(ubicacion);
        }

        public async Task ActualizarAsync(int id, UbicacionDto dto)
        {
            // Puedes repetir la validación aquí si tiene sentido para tu negocio
            var comuna = await _comunaRepo.GetByIdsAsync(dto.Id_Region, dto.id_comuna);
            if (comuna == null)
                throw new System.Exception("La comuna no pertenece a la región indicada.");

            var ubicacion = new Ubicacion
            {
                Id_Ubicacion = id,
                Id_comuna = dto.id_comuna,
                Id_Emprendimiento = dto.Id_Emprendimiento,
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
