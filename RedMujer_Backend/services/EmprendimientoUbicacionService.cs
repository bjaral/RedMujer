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
                IdUbicacion = eu.IdUbicacion,
                IdEmprendimiento = eu.IdEmprendimiento
            });
        }

        public async Task<EmprendimientoUbicacionDto?> GetByIdsAsync(int idEmprendimiento, int idUbicacion)
        {
            var eu = await _repo.GetByIdsAsync(idEmprendimiento, idUbicacion);
            if (eu == null) return null;

            return new EmprendimientoUbicacionDto
            {
                IdUbicacion = eu.IdUbicacion,
                IdEmprendimiento = eu.IdEmprendimiento
            };
        }

        public async Task CrearAsync(EmprendimientoUbicacionDto dto)
        {
            var eu = new EmprendimientoUbicacion
            {
                IdUbicacion = dto.IdUbicacion,
                IdEmprendimiento = dto.IdEmprendimiento
            };
            await _repo.InsertAsync(eu);
        }

        public async Task EliminarAsync(int idEmprendimiento, int idUbicacion)
        {
            await _repo.DeleteAsync(idEmprendimiento, idUbicacion);
        }
    }
}
