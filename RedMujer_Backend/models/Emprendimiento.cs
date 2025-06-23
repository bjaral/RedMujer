using System.Runtime.Serialization;

namespace RedMujer_Backend.models
{
    public enum TipoModalidad
    {
        Presencial,
        Online,
        PresencialYOnline
    }

    public class Emprendimiento
    {
        public int Id_Emprendimiento { get; set; }
        public string? RUT { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public TipoModalidad? Modalidad { get; set; }  // Nullable
        public string? Horario_Atencion { get; set; }
        public bool Vigencia { get; set; }
        public string? Imagen { get; set; }
    }
}
