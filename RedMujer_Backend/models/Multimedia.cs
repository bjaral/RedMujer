namespace RedMujer_Backend.models
{
    public enum TipoMultimedia
    {
        video,
        imagen
    }

    public class Multimedia
    {
        public int Id_Multimedia { get; set; }
        public int Id_Emprendimiento { get; set; }
        public TipoMultimedia Tipo_Multimedia { get; set; }
        public string Ruta { get; set; }
        public string? Descripcion { get; set; }
        public bool Vigencia { get; set; }
    }
}
