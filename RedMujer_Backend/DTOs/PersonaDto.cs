using System.ComponentModel.DataAnnotations;

namespace RedMujer_Backend.DTOs
{
    public class PersonaDto
    {
        [Required]
        public int IdUbicacion { get; set; }

        [Required]
        [StringLength(50)]
        public string RUN { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string PrimerApellido { get; set; }

        [Required]
        [StringLength(50)]
        public string SegundoApellido { get; set; }

        public bool Vigencia { get; set; } = true;

        [Required]
        public int UsuarioId { get; set; } // <--- Nuevo campo requerido
    }
}
