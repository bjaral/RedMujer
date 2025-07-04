using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RedMujer_Backend.DTOs
{
    public class UsuarioDto
    {
        public int? Id_Usuario { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50)]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [StringLength(50)]
        public string Contrasenna { get; set; }

        public bool Vigencia { get; set; } = true;

        [Required(ErrorMessage = "El tipo de usuario es obligatorio.")]
        [EnumDataType(typeof(RedMujer_Backend.models.TipoUsuario), ErrorMessage = "Tipo de usuario no válido (admin, emprendedora, visitante)")]
        public string Tipo_Usuario { get; set; } // El usuario lo escribe como string

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no es válido.")]
        [StringLength(100)]
        public string Correo { get; set; }

        // Si después quieres permitir subir una foto, aquí podrías poner:
        // public IFormFile Foto { get; set; }
    }
}
