using System.ComponentModel.DataAnnotations;

namespace RedMujer_Backend.DTOs
{
    public class CategoriaDto
    {
        public int? Id_Categoria { get; set; } // Opcional para POST, obligatorio en GET/PUT

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; }

        public bool Vigencia { get; set; } = true;

        [Required(ErrorMessage = "El grupo de categoría es obligatorio.")]
        public string Grupo_Categoria { get; set; }
    }
}
