namespace RedMujer_Backend.models
{
    public enum TipoUsuario
    {
        admin,
        emprendedora,
        visitante
    }

    public class Usuario
    {
        public int Id_Usuario { get; set; }
        public string UsuarioNombre { get; set; } = string.Empty;
        public string Contrasenna { get; set; } = string.Empty;
        public bool Vigencia { get; set; }
        public TipoUsuario Tipo_Usuario { get; set; }  // Aquí está el enum embebido
        public string Correo { get; set; } = string.Empty;
    }
}
