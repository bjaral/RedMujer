namespace RedMujer_Backend.DTOs
{
    public class PersonaDto
    {
        public int IdUbicacion { get; set; }
        public string RUN { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string PrimerApellido { get; set; } = string.Empty;
        public string SegundoApellido { get; set; } = string.Empty;
        public bool Vigencia { get; set; }
        public int UsuarioId { get; set; }
    }
}
