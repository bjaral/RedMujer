using RedMujer_Backend.DTOs;
using RedMujer_Backend.models;
using RedMujer_Backend.repositories;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RedMujer_Backend.services
{
    public class EmprendimientoService : IEmprendimientoService
    {
        private readonly IEmprendimientoRepository _repo;

        public EmprendimientoService(IEmprendimientoRepository repo)
        {
            _repo = repo;
        }

        private TipoModalidad? ConvertirModalidad(string? modalidad)
        {
            if (string.IsNullOrWhiteSpace(modalidad))
                return null;

            modalidad = modalidad.Trim();

            return modalidad switch
            {
                "Presencial" => TipoModalidad.Presencial,
                "Online" => TipoModalidad.Online,
                "PresencialYOnline" => TipoModalidad.PresencialYOnline,
                _ => null,
            };
        }

        public async Task<IEnumerable<Emprendimiento>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<IEnumerable<Emprendimiento>> GetRandomAsync(int cantidad)
        {
            return await _repo.GetRandomAsync(cantidad);
        }

        public async Task<Emprendimiento> CrearAsync(EmprendimientoDto dto)
        {
            var entidad = new Emprendimiento
            {
                RUT = dto.RUT,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Horario_Atencion = dto.Horario_Atencion,
                Vigencia = dto.Vigencia,
                Imagen = dto.Imagen,
                Modalidad = ConvertirModalidad(dto.Modalidad)
            };

            await _repo.InsertarEmprendimientoAsync(entidad);

            return entidad;
        }
    }
}
