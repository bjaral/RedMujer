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

        // --- GETS ahora devuelven DTOs ---
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

        // --- CREAR Y ACTUALIZAR usan el DTO de entrada (POST/PUT) ---
        public async Task<Emprendimiento> CrearAsync(EmprendimientoCreateDto dto, string? rutaImagen)
        {
            var entidad = new Emprendimiento
            {
                RUT = dto.RUT,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Horario_Atencion = dto.Horario_Atencion,
                Vigencia = dto.Vigencia,
                Imagen = !string.IsNullOrEmpty(rutaImagen) ? rutaImagen.Replace("\\", "/") : null,
                Modalidad = MapearModalidad(dto.Modalidad)
            };
            var id = await _repo.InsertarEmprendimientoAsync(entidad);
            entidad.Id_Emprendimiento = id;
            return entidad;
        }

        public async Task ActualizarAsync(int id, EmprendimientoCreateDto dto, string? rutaImagen)
        {
            var emprendimiento = new Emprendimiento
            {
                Id_Emprendimiento = id,
                RUT = dto.RUT,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Horario_Atencion = dto.Horario_Atencion,
                Vigencia = dto.Vigencia,
                Imagen = !string.IsNullOrEmpty(rutaImagen) ? rutaImagen.Replace("\\", "/") : null,
                Modalidad = MapearModalidad(dto.Modalidad)
            };

            await _repo.ActualizarEmprendimientoAsync(emprendimiento);
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

        // --- Mapear modelo -> DTO para GETs ---
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
                    TipoModalidad.PresencialYOnline => "Presencial y Online",
                    TipoModalidad.Presencial => "Presencial",
                    TipoModalidad.Online => "Online",
                    _ => ""
                }
            };
        }
    }
}
