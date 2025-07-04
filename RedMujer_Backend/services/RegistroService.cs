using RedMujer_Backend.DTOs;
using RedMujer_Backend.models;
using RedMujer_Backend.repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Runtime.Serialization;

namespace RedMujer_Backend.services
{
    public class RegistroService : IRegistroService
    {
        private readonly IRegistroRepository _repo;

        public RegistroService(IRegistroRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<RegistroDto>> GetAllAsync()
        {
            var registros = await _repo.GetAllAsync();
            return registros.Select(r => new RegistroDto
            {
                UsuarioId = r.UsuarioId,
                Fecha = r.Fecha,
                ValorActual = r.ValorActual,
                TipoRegistro = GetEnumMemberValue(r.TipoRegistro)
            });
        }

        public async Task<RegistroDto?> GetByIdAsync(int id)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r == null) return null;

            return new RegistroDto
            {
                UsuarioId = r.UsuarioId,
                Fecha = r.Fecha,
                ValorActual = r.ValorActual,
                TipoRegistro = GetEnumMemberValue(r.TipoRegistro)
            };
        }

        public async Task CrearAsync(RegistroDto dto)
        {
            var tipoEnum = ParseTipoRegistro(dto.TipoRegistro);

            var registro = new Registro
            {
                UsuarioId = dto.UsuarioId,
                Fecha = dto.Fecha,
                ValorActual = dto.ValorActual,
                TipoRegistro = tipoEnum
            };
            await _repo.InsertAsync(registro);
        }

        public async Task ActualizarAsync(int id, RegistroDto dto)
        {
            var tipoEnum = ParseTipoRegistro(dto.TipoRegistro);

            var registro = new Registro
            {
                IdRegistro = id,
                UsuarioId = dto.UsuarioId,
                Fecha = dto.Fecha,
                ValorActual = dto.ValorActual,
                TipoRegistro = tipoEnum
            };
            await _repo.UpdateAsync(registro);
        }

        public async Task EliminarAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }

        // Auxiliar: string → enum
        private TipoRegistro ParseTipoRegistro(string value)
        {
            foreach (var field in typeof(TipoRegistro).GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field, typeof(EnumMemberAttribute)) as EnumMemberAttribute;
                if (attribute != null && attribute.Value == value)
                    return (TipoRegistro)field.GetValue(null);
            }
            throw new ArgumentException("TipoRegistro no válido: " + value);
        }

        // Auxiliar: enum → string
        private string GetEnumMemberValue(TipoRegistro tipo)
        {
            var type = typeof(TipoRegistro);
            var memInfo = type.GetMember(tipo.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(EnumMemberAttribute), false);
            return attributes.Length > 0
                ? ((EnumMemberAttribute)attributes[0]).Value
                : tipo.ToString();
        }
    }
}
