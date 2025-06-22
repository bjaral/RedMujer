namespace RedMujer_Backend.models
{
    public enum TipoModalidad
    {
        Presencial,
        Online,
        Mixta
    }

    public class Emprendimiento
    {
        public int Id_Emprendimiento { get; set; }
        public string RUT { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public TipoModalidad Modalidad { get; set; }
        public string Horario_Atencion { get; set; } = string.Empty;
        public DateTime Fecha_Inicio { get; set; }
        public bool Vigencia { get; set; }
    }
}

