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
                IdEmprendimiento = c.IdEmprendimiento,
                Valor = c.Valor,
                Vigencia = c.Vigencia,
                TipoContacto = c.TipoContacto
            });
        }

        public async Task<ContactoDto?> GetByIdAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return null;

            return new ContactoDto
            {
                IdEmprendimiento = c.IdEmprendimiento,
                Valor = c.Valor,
                Vigencia = c.Vigencia,
                TipoContacto = c.TipoContacto
            };
        }

        public async Task CrearAsync(ContactoDto dto)
        {
            var contacto = new Contacto
            {
                IdEmprendimiento = dto.IdEmprendimiento,
                Valor = dto.Valor,
                Vigencia = dto.Vigencia,
                TipoContacto = dto.TipoContacto
            };
            await _repo.InsertAsync(contacto);
        }

        public async Task ActualizarAsync(int id, ContactoDto dto)
        {
            var contacto = new Contacto
            {
                IdContacto = id,
                IdEmprendimiento = dto.IdEmprendimiento,
                Valor = dto.Valor,
                Vigencia = dto.Vigencia,
                TipoContacto = dto.TipoContacto
            };
            await _repo.UpdateAsync(contacto);
        }

        public async Task EliminarAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
