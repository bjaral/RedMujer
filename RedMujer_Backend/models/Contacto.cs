namespace RedMujer_Backend.models
{
    public class Contacto
    {
        public enum TipoContacto
        {

            tipo1,
            tipo2,
            tipo3,
        }

        public int Id_Contacto { get; set; }
        public int Id_Emprendimiento { get; set; }
        public string Valor { get; set; } = string.Empty;
        public bool Vigencia { get; set; } = true;
        public required TipoContacto Tipo_Contacto { get; set; }
    }
}
