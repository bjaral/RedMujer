namespace RedMujer_Backend.DTOs
{
    public class UbicacionDto
    {
        public int? Id_Ubicacion { get; set; }
        public int Id_Region { get; set; }              // <-- Campo requerido
<<<<<<< HEAD
        public int id_comuna { get; set; }
        public int Id_Emprendimiento { get; set; }
=======
        public int Id_Comuna { get; set; }
>>>>>>> 848ffadbbde22cf0434e7a5441d434c236e670d5
        public string Calle { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string? Referencia { get; set; }
        public bool Vigencia { get; set; } = true;
    }
}
