using RedMujer_Backend.models;

public interface IMultimediaRepository
{
    Task<Multimedia?> GetByIdAsync(int idMultimedia);
    Task<IEnumerable<Multimedia>> GetByEmprendimientoIdAsync(int idEmprendimiento);
    Task<int> AddAsync(Multimedia multimedia);
    Task UpdateAsync(Multimedia multimedia);
    Task DeleteAsync(int idMultimedia);
}
