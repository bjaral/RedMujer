using RedMujer_Backend.DTOs;
using RedMujer_Backend.models;
using RedMujer_Backend.repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace RedMujer_Backend.services
{
    public class EmprendimientoService : IEmprendimientoService
    {
        private readonly IEmprendimientoRepository _repo;

        public EmprendimientoService(IEmprendimientoRepository repo)
        {
            _repo = repo;
        }

        private TipoModalidad? MapearModalidad(string? modalidad)
        {
            if (string.IsNullOrWhiteSpace(modalidad)) return null;
            if (Enum.TryParse<TipoModalidad>(modalidad.Replace(" ", ""), true, out var result))
                return result;

            modalidad = modalidad.Trim().ToLower();
            return modalidad switch
            {
                "presencial" => TipoModalidad.Presencial,
                "online" => TipoModalidad.Online,
                "presencialyonline" => TipoModalidad.PresencialYOnline,
                _ => null
            };
        }

        public async Task<IEnumerable<EmprendimientoDto>> GetAllAsync()
        {
            var lista = await _repo.GetAllAsync();
            return lista.Select(e => MapToDto(e));
        }

        public async Task<IEnumerable<EmprendimientoDto>> GetRandomAsync(int cantidad)
        {
            var lista = await _repo.GetRandomAsync(cantidad);
            return lista.Select(e => MapToDto(e));
        }

        public async Task<EmprendimientoDto?> GetByIdAsync(int id)
        {
            var e = await _repo.GetByIdAsync(id);
            return e == null ? null : MapToDto(e);
        }

        public async Task<Emprendimiento> CrearAsync(EmprendimientoCreateDto dto, string? rutaImagen)
        {
            var entidad = new Emprendimiento
            {
                RUT = dto.RUT,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Horario_Atencion = dto.Horario_Atencion,
                Vigencia = dto.Vigencia ?? false,
                Imagen = !string.IsNullOrEmpty(rutaImagen) ? rutaImagen.Replace("\\", "/") : null,
                Modalidad = MapearModalidad(dto.Modalidad)
            };
            var id = await _repo.InsertarEmprendimientoAsync(entidad);
            entidad.Id_Emprendimiento = id;
            return entidad;
        }

        public async Task ActualizarAsync(int id, EmprendimientoCreateDto dto, string? rutaImagen)
        {
            var emprendimientoExistente = await _repo.GetByIdAsync(id);
            if (emprendimientoExistente == null)
                throw new KeyNotFoundException("Emprendimiento no encontrado");

            emprendimientoExistente.RUT = dto.RUT;
            emprendimientoExistente.Nombre = dto.Nombre;
            emprendimientoExistente.Descripcion = dto.Descripcion;
            emprendimientoExistente.Horario_Atencion = dto.Horario_Atencion;
            emprendimientoExistente.Vigencia = dto.Vigencia ?? emprendimientoExistente.Vigencia;
            emprendimientoExistente.Modalidad = MapearModalidad(dto.Modalidad);

            if (!string.IsNullOrEmpty(rutaImagen))
                emprendimientoExistente.Imagen = rutaImagen.Replace("\\", "/");

            await _repo.ActualizarEmprendimientoAsync(emprendimientoExistente);
        }

        public async Task<Emprendimiento> ActualizarImagenAsync(int idEmprendimiento, string? rutaImagen)
        {
            var emprendimiento = await _repo.GetByIdAsync(idEmprendimiento);
            if (emprendimiento == null)
                throw new KeyNotFoundException("Emprendimiento no encontrado");

            emprendimiento.Imagen = !string.IsNullOrEmpty(rutaImagen) ? rutaImagen.Replace("\\", "/") : null;
            await _repo.ActualizarEmprendimientoAsync(emprendimiento);

            return emprendimiento;
        }

        public async Task EliminarAsync(int id)
        {
            await _repo.EliminarEmprendimientoAsync(id);
        }

        public async Task<bool> ExisteAsync(int idEmprendimiento)
        {
            var emprendimiento = await _repo.GetByIdAsync(idEmprendimiento);
            return emprendimiento != null;
        }

        public async Task<string?> ObtenerRutaImagenPrincipalAsync(int id)
        {
            return await _repo.GetImagenPrincipalAsync(id);
        }

        public async Task ActualizarImagenPrincipalAsync(int id, string? ruta)
        {
            await _repo.UpdateImagenPrincipalAsync(id, ruta);
        }

        private EmprendimientoDto MapToDto(Emprendimiento e)
        {
            return new EmprendimientoDto
            {
                Id_Emprendimiento = e.Id_Emprendimiento,
                RUT = e.RUT,
                Nombre = e.Nombre,
                Descripcion = e.Descripcion,
                Horario_Atencion = e.Horario_Atencion,
                Vigencia = e.Vigencia,
                Imagen = e.Imagen,
                Modalidad = e.Modalidad switch
                {
                    TipoModalidad.PresencialYOnline => "PresencialYOnline",
                    TipoModalidad.Presencial => "Presencial",
                    TipoModalidad.Online => "Online",
                    _ => ""
                }
            };
        }
        public async Task<IEnumerable<Emprendimiento>> ObtenerPorPersonaAsync(int id_Persona)
        {
            return await _repo.GetByPersonaIdAsync(id_Persona);
        }
        public async Task<string?> ObtenerVideoPrincipalAsync(int id)
        {
            return await _repo.GetVideoPrincipalAsync(id);
        }
        public async Task ActualizarVideoPrincipalAsync(int id, string? videoUrl)
        {
            await _repo.UpdateVideoPrincipalAsync(id, videoUrl);
        }
    }
}