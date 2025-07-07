using System.ComponentModel.DataAnnotations.Schema;

namespace RedMujer_Backend.models {
    
    public class Persona {
        public int Id_Persona { get; set; }
        public int Id_Ubicacion { get; set; }
        public int Id_Usuario { get; set; }
        public string RUN { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        
        [Column("primer_apellido")]
        public string PrimerApellido { get; set; } = string.Empty;
        
        
        [Column("segundo_apellido")]
        public string SegundoApellido { get; set; } = string.Empty;
        public bool Vigencia { get; set; } = true;

    }
}
