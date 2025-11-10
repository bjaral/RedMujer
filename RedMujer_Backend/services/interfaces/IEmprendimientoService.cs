using RedMujer_Backend.DTOs;
using RedMujer_Backend.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public interface IEmprendimientoService
    {
        Task<IEnumerable<EmprendimientoDto>> GetAllAsync();
        Task<IEnumerable<EmprendimientoDto>> GetRandomAsync(int cantidad);
        Task<EmprendimientoDto?> GetByIdAsync(int id);
        Task<Emprendimiento> CrearAsync(EmprendimientoCreateDto dto, string? rutaImagen);
        Task ActualizarAsync(int id, EmprendimientoCreateDto dto, string? rutaImagen);
        Task EliminarAsync(int id);
        Task<Emprendimiento> ActualizarImagenAsync(int idEmprendimiento, string? rutaImagen);
        Task<bool> ExisteAsync(int idEmprendimiento);
        Task<string?> ObtenerRutaImagenPrincipalAsync(int id);
        Task ActualizarImagenPrincipalAsync(int id, string? ruta);
        // obtener los emprendimientos de una persona
        Task<IEnumerable<Emprendimiento>> ObtenerPorPersonaAsync(int idPersona);
        // Task<string?> ObtenerVideoPrincipalAsync(int id);
        // Task ActualizarVideoPrincipalAsync(int id, string? videoUrl);
        Task<bool> EsPropietariaAsync(int idEmprendimiento, int idUsuario);
    }
}
