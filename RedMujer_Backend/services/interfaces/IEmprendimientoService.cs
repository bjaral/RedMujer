using RedMujer_Backend.DTOs;
using RedMujer_Backend.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RedMujer_Backend.services
{
    public interface IEmprendimientoService
    {
        Task<IEnumerable<Emprendimiento>> GetAllAsync();
        Task<IEnumerable<Emprendimiento>> GetRandomAsync(int cantidad);
        Task<Emprendimiento> CrearAsync(EmprendimientoDto dto, string? rutaImagen);
        Task ActualizarAsync(int id, EmprendimientoDto dto, string? rutaImagen);    
        Task EliminarAsync(int id);
        Task<Emprendimiento> ActualizarImagenAsync(int idEmprendimiento, string? rutaImagen);
        Task<bool> ExisteAsync(int idEmprendimiento);
    }
}
