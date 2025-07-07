using RedMujer_Backend.DTOs;
using RedMujer_Backend.models;
using RedMujer_Backend.repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public class EmprendimientoCategoriaService : IEmprendimientoCategoriaService
    {
        private readonly IEmprendimientoCategoriaRepository _repo;

        public EmprendimientoCategoriaService(IEmprendimientoCategoriaRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<EmprendimientoCategoriaDto>> GetAllAsync()
        {
            var items = await _repo.GetAllAsync();
            return items.Select(ec => new EmprendimientoCategoriaDto
            {
                Id_Categoria = ec.Id_Categoria,
                Id_Emprendimiento = ec.Id_Emprendimiento
            });
        }

        public async Task<EmprendimientoCategoriaDto?> GetByIdsAsync(int idEmprendimiento, int idCategoria)
        {
            var ec = await _repo.GetByIdsAsync(idEmprendimiento, idCategoria);
            if (ec == null) return null;

            return new EmprendimientoCategoriaDto
            {
                Id_Categoria = ec.Id_Categoria,
                Id_Emprendimiento = ec.Id_Emprendimiento
            };
        }

        public async Task CrearAsync(EmprendimientoCategoriaDto dto)
        {
            var ec = new EmprendimientoCategoria
            {
                Id_Categoria = dto.Id_Categoria,
                Id_Emprendimiento = dto.Id_Emprendimiento
            };
            await _repo.InsertAsync(ec);
        }

        public async Task EliminarAsync(int idEmprendimiento, int idCategoria)
        {
            await _repo.DeleteAsync(idEmprendimiento, idCategoria);
        }
    }
}
