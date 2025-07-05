using System.ComponentModel.DataAnnotations;

namespace RedMujer_Backend.DTOs
{
    public class CategoriaDto
    {
        public int? Id_Categoria { get; set; } 
        public string? Descripcion { get; set; }
        public bool Vigencia { get; set; } = true;
        public required string Grupo_Categoria { get; set; }
    }
}
