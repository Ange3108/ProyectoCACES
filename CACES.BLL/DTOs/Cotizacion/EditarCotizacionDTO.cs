using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CACES.BLL.DTOs.Cotizacion
{
    public class EditarCotizacionDTO
    {
        [Required]
        public int IdCotizacion { get; set; }

        [Required]
        public decimal PrecioBase { get; set; }

        public decimal Descuento { get; set; }

        public decimal Impuesto { get; set; }

        [StringLength(500)]
        public string? Observaciones { get; set; }

        [Required]
        public byte Estado { get; set; }
    }
}