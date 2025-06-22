using RedMujer_Backend.models;

namespace RedMujer_Backend.repositories
{
    public interface IEmprendimientoRepository
    {
        Task<IEnumerable<Emprendimiento>> GetAllAsync();
        Task<IEnumerable<Emprendimiento>> GetRandomAsync(int cantidad);
    }
}
