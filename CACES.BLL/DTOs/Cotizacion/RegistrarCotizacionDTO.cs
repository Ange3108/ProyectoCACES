using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CACES.BLL.DTOs.Cotizacion
{
    public class RegistrarCotizacionDTO
    {
        [Required]
        public int IdPaciente { get; set; }

        [Required]
        public int IdMedico { get; set; }

        [Required]
        public int IdProcedimiento { get; set; }

        [StringLength(500)]
        public string? Observaciones { get; set; }
    }
}