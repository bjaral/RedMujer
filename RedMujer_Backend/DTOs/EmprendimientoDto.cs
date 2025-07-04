using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RedMujer_Backend.DTOs
{
    public class EmprendimientoDto
    {
        [Required(ErrorMessage = "El RUT es obligatorio")]
        [StringLength(50, ErrorMessage = "El RUT no debe superar los 50 caracteres")]
        public string? RUT { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50, ErrorMessage = "El nombre no debe superar los 50 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La modalidad es obligatoria")]
        public string? Modalidad { get; set; }

        public string? Horario_Atencion { get; set; }

        public bool Vigencia { get; set; }

        public IFormFile? Imagen { get; set; }
    }
}
