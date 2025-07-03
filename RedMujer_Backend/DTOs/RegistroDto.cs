namespace RedMujer_Backend.DTOs
{
    public class RegistroDto
    {
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public string? ValorActual { get; set; }
        public string TipoRegistro { get; set; } = string.Empty;
    }
}
