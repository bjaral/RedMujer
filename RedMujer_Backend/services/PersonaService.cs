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
                Id_Persona = p.Id_Persona,
                Id_Ubicacion = p.Id_Ubicacion,
                Id_Usuario = p.Id_Usuario,
                RUN = p.RUN,
                Nombre = p.Nombre,
                PrimerApellido = p.PrimerApellido,
                SegundoApellido = p.SegundoApellido,
                Vigencia = p.Vigencia
                
            });
        }

        public async Task<PersonaDto?> GetByIdAsync(int id)
        {
            var p = await _repo.GetByIdAsync(id);
            if (p == null) return null;

            return new PersonaDto
            {
                Id_Persona = p.Id_Persona,
                Id_Ubicacion = p.Id_Ubicacion,
                Id_Usuario = p.Id_Usuario,
                RUN = p.RUN,
                Nombre = p.Nombre,
                PrimerApellido = p.PrimerApellido,
                SegundoApellido = p.SegundoApellido,
                Vigencia = p.Vigencia
                
            };
        }

        public async Task<int> CrearAsync(PersonaCreateDto dto)
        {
            var persona = new Persona
            {
                Id_Ubicacion = dto.Id_Ubicacion,
                Id_Usuario = dto.Id_Usuario,
                RUN = dto.RUN,
                Nombre = dto.Nombre,
                PrimerApellido = dto.PrimerApellido,
                SegundoApellido = dto.SegundoApellido,
                Vigencia = dto.Vigencia
                
            };
            int id = await _repo.InsertAsync(persona);
            return id;
        }

        public async Task ActualizarAsync(int id, PersonaCreateDto dto)
        {
            var persona = new Persona
            {
                Id_Persona = id,
                Id_Ubicacion = dto.Id_Ubicacion,
                Id_Usuario = dto.Id_Usuario,
                RUN = dto.RUN,
                Nombre = dto.Nombre,
                PrimerApellido = dto.PrimerApellido,
                SegundoApellido = dto.SegundoApellido,
                Vigencia = dto.Vigencia
                
            };
            await _repo.UpdateAsync(persona);
        }

        public async Task EliminarAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
        public async Task<PersonaDto?> GetByUsuarioIdAsync(int idUsuario)
        {
            var persona = await _repo.GetByUsuarioIdAsync(idUsuario);
            if (persona == null) return null;
            return new PersonaDto
            {
                Id_Persona = persona.Id_Persona,
                Id_Ubicacion = persona.Id_Ubicacion,
                Id_Usuario = persona.Id_Usuario,
                RUN = persona.RUN,
                Nombre = persona.Nombre,
                PrimerApellido = persona.PrimerApellido,
                SegundoApellido = persona.SegundoApellido,
                Vigencia = persona.Vigencia
            };
        }

    }
}
