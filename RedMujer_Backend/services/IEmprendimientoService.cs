using RedMujer_Backend.DTOs;

namespace RedMujer_Backend.services
{
    public interface IEmprendimientoService
    {
        Task<IEnumerable<EmprendimientoDto>> GetAllAsync();
        Task<IEnumerable<EmprendimientoDto>> GetRandomAsync(int cantidad);
    }
}
