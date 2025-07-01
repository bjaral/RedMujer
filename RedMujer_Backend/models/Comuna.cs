namespace RedMujer_Backend.models
{
    public class Comuna
    {
        public int Id_Comuna { get; set; }
        public int Id_Region { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Vigencia { get; set; } = true;
    }
}
