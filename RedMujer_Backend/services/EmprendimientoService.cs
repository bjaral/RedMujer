using RedMujer_Backend.DTOs;
using RedMujer_Backend.models;
using RedMujer_Backend.repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public class EmprendimientoService : IEmprendimientoService
    {
        private readonly IEmprendimientoRepository _repo;

        public EmprendimientoService(IEmprendimientoRepository repo)
        {
            _repo = repo;
        }

        // --- Mapea string a Enum de forma segura (case insensitive, admite espacios) ---
        private TipoModalidad? MapearModalidad(string? modalidad)
        {
            if (string.IsNullOrWhiteSpace(modalidad)) return null;

            // Quita espacios, compara sin distinguir mayúsculas/minúsculas
            if (Enum.TryParse<TipoModalidad>(modalidad.Replace(" ", ""), true, out var result))
                return result;

            // Opcional: fallback manual
            modalidad = modalidad.Trim().ToLower();
            return modalidad switch
            {
                "presencial" => TipoModalidad.Presencial,
                "online" => TipoModalidad.Online,
                "presencialyonline" => TipoModalidad.PresencialYOnline,
                _ => null
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

        public async Task<Emprendimiento> CrearAsync(EmprendimientoDto dto, string? rutaImagen)
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

        public async Task ActualizarAsync(int id, EmprendimientoDto dto, string? rutaImagen)
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

        public async Task<Emprendimiento?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
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
    }
}
