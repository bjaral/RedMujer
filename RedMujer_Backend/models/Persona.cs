namespace RedMujer_Backend.models {
    
    public class Persona {
        public int Id_Persona { get; set; }
        public int Id_Ubicacion { get; set; }
        public string RUN { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string PrimerApellido { get; set; } = string.Empty;
        public string SegundoApellido { get; set; } = string.Empty;
        public bool Vigencia { get; set; } = true;
        public int UsuarioId { get; set; }
    }
}
