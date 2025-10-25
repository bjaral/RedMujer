using Microsoft.AspNetCore.Http;

namespace RedMujer_Backend.DTOs;

public class EmprendimientoCreateDto
{
    public string? RUT { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Modalidad { get; set; } = "";
    public string? Horario_Atencion { get; set; }
    public bool? Vigencia { get; set; } // <-- ahora es anulable
    public IFormFile? Imagen { get; set; }
    public string? VideoUrl { get; set; }
    public bool EsVacio()
    {
        return string.IsNullOrWhiteSpace(RUT)
            && string.IsNullOrWhiteSpace(Nombre)
            && string.IsNullOrWhiteSpace(Descripcion)
            && string.IsNullOrWhiteSpace(Modalidad)
            && string.IsNullOrWhiteSpace(Horario_Atencion)
            && Vigencia is null; 
    }
}
