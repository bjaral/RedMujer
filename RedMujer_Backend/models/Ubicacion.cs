namespace RedMujer_Backend.models
{
    public class Ubicacion
    {
        public int Id_Ubicacion { get; set; }
        public int Id_Comuna { get; set; }
        public string Calle { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string? Referencia { get; set; }
        public bool Vigencia { get; set; } = true;
    }
}
