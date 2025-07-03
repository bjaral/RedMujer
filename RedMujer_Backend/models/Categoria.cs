namespace RedMujer_Backend.models
{
    public class Categoria
    {
        public int IdCategoria { get; set; }
        public string? Descripcion { get; set; }
        public bool Vigencia { get; set; } = true;
        public string GrupoCategoria { get; set; } = string.Empty;
    }
}
