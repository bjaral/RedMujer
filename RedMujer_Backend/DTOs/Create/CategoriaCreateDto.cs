using RedMujer_Backend.models;

namespace RedMujer_Backend.DTOs
{
    public class CategoriaCreateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Vigencia { get; set; } = true;
        public GrupoCategoria Grupo_Categoria { get; set; }
    }
}
