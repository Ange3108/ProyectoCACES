using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CACES.BLL.DTOs.Precio
{
    public class RegistrarPrecioDTO
    {
        [Required(ErrorMessage = "El médico es obligatorio.")]
        public int IdMedico { get; set; }

        [Required(ErrorMessage = "El procedimiento es obligatorio.")]
        public int IdProcedimiento { get; set; }

        [Required(ErrorMessage = "El costo es obligatorio.")]
        [Range(0.01, 99999999.99, ErrorMessage = "El costo debe ser mayor a 0.")]
        public decimal Costo { get; set; }

        [Required(ErrorMessage = "Los detalles son obligatorios.")]
        [StringLength(100, ErrorMessage = "Los detalles no pueden superar los 100 caracteres.")]
        public string Detalles { get; set; } = string.Empty;
    }
}
