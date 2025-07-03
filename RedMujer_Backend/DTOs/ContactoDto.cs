namespace RedMujer_Backend.DTOs
{
    public class ContactoDto
    {
        public int IdEmprendimiento { get; set; }
        public string Valor { get; set; } = string.Empty;
        public bool Vigencia { get; set; }
        public string TipoContacto { get; set; } = string.Empty;
    }
}
