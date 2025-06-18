namespace RedMujer_Backend.DTOs
{
    public class EmprendimientoDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Modalidad { get; set; } = string.Empty;
        public bool Vigencia { get; set; }
    }
}
