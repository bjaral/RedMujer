namespace RedMujer_Backend.DTOs
{
    public class PlataformaDto
    {
        public int Id_Plataforma { get; set; }
        public int Id_Emprendimiento { get; set; }
        public string Ruta { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Vigencia { get; set; }
        public string Tipo_Plataforma { get; set; } = string.Empty;
    }
}
