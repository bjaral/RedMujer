using System.ComponentModel.DataAnnotations;

namespace RedMujer_Backend.DTOs
{
    public class PersonaDto
    {
        public int Id_Persona { get; set; }
        public int Id_Ubicacion { get; set; }
        public int Id_Usuario { get; set; } 
        public required string RUN { get; set; }
        public required string Nombre { get; set; }
        public required string PrimerApellido { get; set; }
        public required string SegundoApellido { get; set; }
        public bool Vigencia { get; set; } = true;
        
    }
}
