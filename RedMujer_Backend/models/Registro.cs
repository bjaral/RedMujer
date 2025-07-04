namespace RedMujer_Backend.models
{
    public class Registro
    {
        public int IdRegistro { get; set; }
        public int UsuarioId { get; set; }
        public DateTime Fecha { get; set; }
        public string? ValorActual { get; set; }
        public string TipoRegistro { get; set; } = string.Empty;
    }
}
