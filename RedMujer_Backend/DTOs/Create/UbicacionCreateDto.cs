namespace RedMujer_Backend.DTOs
{
    public class UbicacionCreateDto
    {

        public int Id_Region { get; set; }              // <-- Campo requerido
        public int Id_Comuna { get; set; }
        public string Calle { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string? Referencia { get; set; }
        public bool Vigencia { get; set; } = true;
    }
}
