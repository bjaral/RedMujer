using System.ComponentModel.DataAnnotations;

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
        [Key] // <----- AGREGA ESTO
        public int Id_Usuario { get; set; }
        public string UsuarioNombre { get; set; }
        public string Contrasenna { get; set; }
        public bool Vigencia { get; set; }
        public TipoUsuario Tipo_Usuario { get; set; }
        public string Correo { get; set; }
    }

    }
