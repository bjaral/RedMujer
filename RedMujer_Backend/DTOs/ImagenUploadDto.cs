public class ImagenUploadDto
{
    public IFormFile Imagen { get; set; }
    public string Tipo { get; set; } // "principal" o "adicional"
}
