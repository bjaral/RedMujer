using System;
using System.ComponentModel.DataAnnotations;

namespace RedMujer_Backend.DTOs
{
    public class RegistroDto
    {
        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        public string? ValorActual { get; set; }

        [Required]
        public string TipoRegistro { get; set; } // Como string, se parsea al enum en el service
    }
}
