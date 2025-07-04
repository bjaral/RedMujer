namespace RedMujer_Backend.models
{
    public class Contacto
    {
        public int IdContacto { get; set; }
        public int IdEmprendimiento { get; set; }
        public string Valor { get; set; } = string.Empty;
        public bool Vigencia { get; set; } = true;
        public string TipoContacto { get; set; } = string.Empty;
    }
}
