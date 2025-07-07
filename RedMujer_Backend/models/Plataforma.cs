using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RedMujer_Backend.models
{
    public class Plataforma
    {
        // Enum anidado
        public enum TipoPlataforma
        {
            red_social,
            sitio_web,
            mercado_online,
            aplicacion_movil,
            otro
        }
        
        [Key]
        public int Id_Plataforma { get; set; }

        [Required]
        public int Id_Emprendimiento { get; set; }

        [ForeignKey("Id_Emprendimiento")]
        public Emprendimiento? Emprendimiento { get; set; }

        [Required]
        [MaxLength(2048)]
        public string Ruta { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        public bool Vigencia { get; set; } = true;

        [Required]
        public TipoPlataforma Tipo_Plataforma { get; set; }

        
    }
}
