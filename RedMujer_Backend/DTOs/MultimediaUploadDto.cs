namespace RedMujer_Backend.DTOs
{   public class MultimediaUploadDto
{
    public IFormFile Archivo { get; set; } = default!;
    public string Tipo_Multimedia { get; set; } = string.Empty; 
    public string? Descripcion { get; set; }
    public bool EsPrincipal { get; set; } = false; 
}

}
