using System;
using System.ComponentModel.DataAnnotations;

namespace RedMujer_Backend.DTOs
{
    public class RegistroDto
    {
        public int Id_Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public string? ValorActual { get; set; }
        public required string TipoRegistro { get; set; }
    }
}
