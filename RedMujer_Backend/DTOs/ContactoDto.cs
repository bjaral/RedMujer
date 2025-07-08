using RedMujer_Backend.models;

namespace RedMujer_Backend.DTOs
{
    public class ContactoDto
    {
        public int Id_Contacto { get; set; }
        public int Id_Emprendimiento { get; set; }
        public string Valor { get; set; } = string.Empty;
        public bool Vigencia { get; set; }
        public Contacto.TipoContacto Tipo_Contacto { get; set; }
    }
}
