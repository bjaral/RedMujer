namespace RedMujer_Backend.models {
    
    public class Persona {
        public int IdPersona { get; set; }
        public int IdUbicacion { get; set; }
        public string RUN { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string PrimerApellido { get; set; } = string.Empty;
        public string SegundoApellido { get; set; } = string.Empty;
        public bool Vigencia { get; set; } = true;
        public int UsuarioId { get; set; }
    }
}
