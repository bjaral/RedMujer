using System.ComponentModel.DataAnnotations;

namespace RedMujer_Backend.DTOs
{
    public class PersonaDto
    {
        public int IdUbicacion { get; set; }

        public required string RUN { get; set; }
        public required string Nombre { get; set; }
        public required string PrimerApellido { get; set; }
        public required string SegundoApellido { get; set; }
        public bool Vigencia { get; set; } = true;
        public int UsuarioId { get; set; } 
    }
}
