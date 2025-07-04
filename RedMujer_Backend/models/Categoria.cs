namespace RedMujer_Backend.models
{
    public class Categoria
    {
        public int Id_Categoria { get; set; }
        public string Descripcion { get; set; }
        public bool Vigencia { get; set; } = true;
        public string Grupo_Categoria { get; set; }
    }
}
