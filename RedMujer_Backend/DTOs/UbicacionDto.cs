namespace RedMujer_Backend.DTOs
{
    public class UbicacionDto
    {
        public int Id_Region { get; set; }              // <-- Campo requerido
        public int id_comuna { get; set; }
        public int Id_Emprendimiento { get; set; }
        public string Calle { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string? Referencia { get; set; }
        public bool Vigencia { get; set; } = true;
    }
}
