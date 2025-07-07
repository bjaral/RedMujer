using RedMujer_Backend.DTOs;
using RedMujer_Backend.models;
using RedMujer_Backend.repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public class ContactoService : IContactoService
    {
        private readonly IContactoRepository _repo;

        public ContactoService(IContactoRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ContactoDto>> GetAllAsync()
        {
            var contactos = await _repo.GetAllAsync();
            return contactos.Select(c => new ContactoDto
            {
                Id_Emprendimiento = c.Id_Emprendimiento,
                Valor = c.Valor,
                Vigencia = c.Vigencia,
                Tipo_Contacto = c.Tipo_Contacto
            });
        }

        public async Task<ContactoDto?> GetByIdAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return null;

            return new ContactoDto
            {
                Id_Emprendimiento = c.Id_Emprendimiento,
                Valor = c.Valor,
                Vigencia = c.Vigencia,
                Tipo_Contacto = c.Tipo_Contacto
            };
        }

        public async Task CrearAsync(ContactoCreateDto dto)
        {
            var contacto = new Contacto
            {
                Id_Emprendimiento = dto.Id_Emprendimiento,
                Valor = dto.Valor,
                Vigencia = dto.Vigencia,
                Tipo_Contacto = dto.Tipo_Contacto
            };
            await _repo.InsertAsync(contacto);
        }

        public async Task ActualizarAsync(int id, ContactoCreateDto dto)
        {
            var contacto = new Contacto
            {
                Id_Contacto = id,
                Id_Emprendimiento = dto.Id_Emprendimiento,
                Valor = dto.Valor,
                Vigencia = dto.Vigencia,
                Tipo_Contacto = dto.Tipo_Contacto
            };
            await _repo.UpdateAsync(contacto);
        }

        public async Task EliminarAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
