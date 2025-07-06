using RedMujer_Backend.models;
using RedMujer_Backend.DTOs;

public interface IMultimediaService
{
    Task<IEnumerable<Multimedia>> ListarPorEmprendimientoAsync(int idEmprendimiento);
    Task<Multimedia?> GetByIdAsync(int idMultimedia);
    Task<int> SubirMultimediaAsync(int idEmprendimiento, MultimediaUploadDto dto, string mediaRootPath);
    Task EliminarMultimediaAsync(int idMultimedia, string mediaRootPath);
    Task UpdateAsync(Multimedia multimedia);
}
