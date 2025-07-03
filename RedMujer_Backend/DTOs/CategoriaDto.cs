namespace RedMujer_Backend.DTOs
{
    public class CategoriaDto
    {
        public string? Descripcion { get; set; }
        public bool Vigencia { get; set; }
        public string GrupoCategoria { get; set; } = string.Empty;
    }
}
