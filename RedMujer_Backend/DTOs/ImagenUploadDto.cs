public class ImagenUploadDto
{
    public IFormFile Imagen { get; set; } = default!; 
    public string Tipo { get; set; } = string.Empty; 
}
