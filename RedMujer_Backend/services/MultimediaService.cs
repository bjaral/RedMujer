using RedMujer_Backend.models;
using RedMujer_Backend.repositories;
using RedMujer_Backend.DTOs;

public class MultimediaService : IMultimediaService
{
    private readonly IMultimediaRepository _multimediaRepo;

    public MultimediaService(IMultimediaRepository multimediaRepo)
    {
        _multimediaRepo = multimediaRepo;
    }

    public async Task<IEnumerable<Multimedia>> ListarPorEmprendimientoAsync(int idEmprendimiento)
        => await _multimediaRepo.GetByEmprendimientoIdAsync(idEmprendimiento);

    public async Task<Multimedia?> GetByIdAsync(int idMultimedia)
        => await _multimediaRepo.GetByIdAsync(idMultimedia);

    public async Task<int> SubirMultimediaAsync(int idEmprendimiento, MultimediaUploadDto dto, string mediaRootPath)
    {
        if (!Enum.TryParse<TipoMultimedia>(dto.Tipo_Multimedia, true, out var tipo))
            throw new Exception("Tipo de multimedia no válido. Debe ser 'imagen' o 'video'.");

        string subCarpeta;
        if (dto.EsPrincipal)
            subCarpeta = "imagen_principal";
        else
            subCarpeta = tipo == TipoMultimedia.imagen
                ? "imagenes_emprendimiento"
                : "videos_emprendimiento";

        string carpeta = Path.Combine(mediaRootPath, "emprendimientos", idEmprendimiento.ToString(), subCarpeta);
        if (!Directory.Exists(carpeta))
            Directory.CreateDirectory(carpeta);

        if (dto.EsPrincipal)
        {
            var archivosPrincipales = Directory.GetFiles(carpeta);
            foreach (var file in archivosPrincipales)
                File.Delete(file);
        }

        var nombreArchivo = Guid.NewGuid() + Path.GetExtension(dto.Archivo.FileName);
        var rutaRelativa = Path.Combine("emprendimientos", idEmprendimiento.ToString(), subCarpeta, nombreArchivo);
        var rutaFisica = Path.Combine(mediaRootPath, rutaRelativa);

        using (var stream = new FileStream(rutaFisica, FileMode.Create))
            await dto.Archivo.CopyToAsync(stream);

        var multimedia = new Multimedia
        {
            Id_Emprendimiento = idEmprendimiento,
            Ruta = rutaRelativa.Replace("\\", "/"),
            Descripcion = dto.Descripcion,
            Vigencia = true,
            Tipo_Multimedia = tipo
        };

        return await _multimediaRepo.AddAsync(multimedia);
    }

    public async Task EliminarMultimediaAsync(int idMultimedia, string mediaRootPath)
    {
        var multimedia = await _multimediaRepo.GetByIdAsync(idMultimedia);
        if (multimedia == null)
            throw new Exception("Archivo no encontrado.");

        var rutaFisica = Path.Combine(mediaRootPath, multimedia.Ruta);
        if (System.IO.File.Exists(rutaFisica))
            System.IO.File.Delete(rutaFisica);

        await _multimediaRepo.DeleteAsync(idMultimedia);
    }

    public async Task UpdateAsync(Multimedia multimedia)
        => await _multimediaRepo.UpdateAsync(multimedia);
}
