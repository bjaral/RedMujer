namespace RedMujer_Backend.models
{
    public enum GrupoCategoria
    {
        AlimentosYBebidas,
        ArtesaniaYManualidades,
        ModaYBelleza,
        TecnologiaYServiciosDigitales,
        Servicios,
        SaludYBienestar,
        HogarYJardineria
    }

    public class Categoria
    {
        public int Id_Categoria { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Vigencia { get; set; } = true;
        public required GrupoCategoria Grupo_Categoria { get; set; }
    }
}
