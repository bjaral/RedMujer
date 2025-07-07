using RedMujer_Backend.DTOs;
using RedMujer_Backend.models;
using RedMujer_Backend.repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public class EmprendimientoUbicacionService : IEmprendimientoUbicacionService
    {
        private readonly IEmprendimientoUbicacionRepository _repo;

        public EmprendimientoUbicacionService(IEmprendimientoUbicacionRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<EmprendimientoUbicacionDto>> GetAllAsync()
        {
            var items = await _repo.GetAllAsync();
            return items.Select(eu => new EmprendimientoUbicacionDto
            {
                Id_Ubicacion = eu.Id_Ubicacion,
                Id_Emprendimiento = eu.Id_Emprendimiento
            });
        }

        public async Task<EmprendimientoUbicacionDto?> GetByIdsAsync(int idEmprendimiento, int idUbicacion)
        {
            var eu = await _repo.GetByIdsAsync(idEmprendimiento, idUbicacion);
            if (eu == null) return null;

            return new EmprendimientoUbicacionDto
            {
                Id_Ubicacion = eu.Id_Ubicacion,
                Id_Emprendimiento = eu.Id_Emprendimiento
            };
        }

        public async Task CrearAsync(EmprendimientoUbicacionDto dto)
        {
            var eu = new EmprendimientoUbicacion
            {
                Id_Ubicacion = dto.Id_Ubicacion,
                Id_Emprendimiento = dto.Id_Emprendimiento
            };
            await _repo.InsertAsync(eu);
        }

        public async Task EliminarAsync(int idEmprendimiento, int idUbicacion)
        {
            await _repo.DeleteAsync(idEmprendimiento, idUbicacion);
        }
    }
}
