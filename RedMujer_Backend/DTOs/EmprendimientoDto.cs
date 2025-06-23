namespace RedMujer_Backend.DTOs
{
    public class EmprendimientoDto
    {
        public string? RUT { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string? Modalidad { get; set; }  // string porque viene del cliente como texto
        public string? Horario_Atencion { get; set; }
        public bool Vigencia { get; set; }
        public string? Imagen { get; set; }
    }
}
