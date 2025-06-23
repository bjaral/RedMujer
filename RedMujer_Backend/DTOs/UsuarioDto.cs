namespace RedMujer_Backend.DTOs
{
    public class UsuarioDto
    {
        public string UsuarioNombre { get; set; } = string.Empty;
        public string Contrasenna { get; set; } = string.Empty;
        public bool Vigencia { get; set; }
        public string Tipo_Usuario { get; set; } = string.Empty;  // string para recibir del API
        public string Correo { get; set; } = string.Empty;
    }
}
