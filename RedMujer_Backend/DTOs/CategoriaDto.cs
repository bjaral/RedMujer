using RedMujer_Backend.models;

namespace RedMujer_Backend.DTOs
{
    public class CategoriaDto
    {
        public int Id_Categoria { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Vigencia { get; set; }
        public GrupoCategoria Grupo_Categoria { get; set; }  // usa el enum del modelo
    }
}
