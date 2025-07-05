namespace RedMujer_Backend.models
{
    public class UsuarioDto
    {
        public string Usuario { get; set; }
        public string Contrasenna { get; set; }
        public bool Vigencia { get; set; }
        public string Tipo_Usuario { get; set; }
        public string Correo { get; set; }
    }
}
