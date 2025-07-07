using RedMujer_Backend.DTOs;

namespace RedMujer_Backend.services
{
    public interface IPlataformaService
    {
        Task<IEnumerable<PlataformaDto>> GetAllAsync();
        Task<PlataformaDto?> GetByIdAsync(int id);
        Task<int> CrearAsync(PlataformaCreateDto dto);
        Task<int> ActualizarAsync(int id, PlataformaCreateDto dto);
        Task DeleteAsync(int id);
    }
}
