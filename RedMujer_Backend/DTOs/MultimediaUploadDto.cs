namespace RedMujer_Backend.DTOs
{
    public class MultimediaUploadDto
    {
        public IFormFile Archivo { get; set; }
        public string Tipo_Multimedia { get; set; } // "imagen" o "video"
        public string? Descripcion { get; set; }
    }
}
