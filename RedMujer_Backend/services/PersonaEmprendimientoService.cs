using RedMujer_Backend.DTOs;
using RedMujer_Backend.models;
using RedMujer_Backend.repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public class PersonaEmprendimientoService : IPersonaEmprendimientoService
    {
        private readonly IPersonaEmprendimientoRepository _repo;

        public PersonaEmprendimientoService(IPersonaEmprendimientoRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<PersonaEmprendimientoDto>> GetAllAsync()
        {
            var items = await _repo.GetAllAsync();
            return items.Select(pe => new PersonaEmprendimientoDto
            {
                Id_Persona = pe.Id_Persona,
                Id_Emprendimiento = pe.Id_Emprendimiento
            });
        }

        public async Task<PersonaEmprendimientoDto?> GetByIdsAsync(int idPersona, int idEmprendimiento)
        {
            var pe = await _repo.GetByIdsAsync(idPersona, idEmprendimiento);
            if (pe == null) return null;

            return new PersonaEmprendimientoDto
            {
                Id_Persona = pe.Id_Persona,
                Id_Emprendimiento = pe.Id_Emprendimiento
            };
        }

        public async Task CrearAsync(PersonaEmprendimientoDto dto)
        {
            var pe = new PersonaEmprendimiento
            {
                Id_Persona = dto.Id_Persona,
                Id_Emprendimiento = dto.Id_Emprendimiento
            };
            await _repo.InsertAsync(pe);
        }

        public async Task EliminarAsync(int idPersona, int idEmprendimiento)
        {
            await _repo.DeleteAsync(idPersona, idEmprendimiento);
        }
    }
}
