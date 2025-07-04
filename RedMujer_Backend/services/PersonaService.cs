using RedMujer_Backend.DTOs;
using RedMujer_Backend.models;
using RedMujer_Backend.repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public class PersonaService : IPersonaService
    {
        private readonly IPersonaRepository _repo;

        public PersonaService(IPersonaRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<PersonaDto>> GetAllAsync()
        {
            var personas = await _repo.GetAllAsync();
            return personas.Select(p => new PersonaDto
            {
                IdUbicacion = p.IdUbicacion,
                RUN = p.RUN,
                Nombre = p.Nombre,
                PrimerApellido = p.PrimerApellido,
                SegundoApellido = p.SegundoApellido,
                Vigencia = p.Vigencia,
                UsuarioId = p.UsuarioId
            });
        }

        public async Task<PersonaDto?> GetByIdAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return null;

            return new PersonaDto
            {
                IdUbicacion = p.IdUbicacion,
                RUN = p.RUN,
                Nombre = p.Nombre,
                PrimerApellido = p.PrimerApellido,
                SegundoApellido = p.SegundoApellido,
                Vigencia = p.Vigencia,
                UsuarioId = p.UsuarioId
            };
        }

        public async Task CrearAsync(PersonaDto dto)
        {
            var persona = new Persona
            {
                IdUbicacion = dto.IdUbicacion,
                RUN = dto.RUN,
                Nombre = dto.Nombre,
                PrimerApellido = dto.PrimerApellido,
                SegundoApellido = dto.SegundoApellido,
                Vigencia = dto.Vigencia,
                UsuarioId = dto.UsuarioId
            };
            await _repo.InsertAsync(persona);
        }

        public async Task ActualizarAsync(int id, PersonaDto dto)
        {
            var persona = new Persona
            {
                IdPersona = id,
                IdUbicacion = dto.IdUbicacion,
                RUN = dto.RUN,
                Nombre = dto.Nombre,
                PrimerApellido = dto.PrimerApellido,
                SegundoApellido = dto.SegundoApellido,
                Vigencia = dto.Vigencia,
                UsuarioId = dto.UsuarioId
            };
            await _repo.UpdateAsync(persona);
        }

        public async Task EliminarAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
