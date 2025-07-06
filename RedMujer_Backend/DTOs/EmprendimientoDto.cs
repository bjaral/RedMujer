using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RedMujer_Backend.DTOs
{
    public class EmprendimientoDto
    {
        public int Id_Emprendimiento { get; set; }
        public string? RUT { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Modalidad { get; set; } = "";
        public string? Horario_Atencion { get; set; }
        public bool Vigencia { get; set; }
        public string? Imagen { get; set; }  // aquí va la ruta/url de la imagen

    }
}