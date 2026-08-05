using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CACES.BLL.DTOs.Precio
{
    public class EditarPrecioDTO
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El precio seleccionado no es válido.")]
        public int IdPrecio { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un médico.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un médico válido.")]
        public int IdMedico { get; set; }

        [Required(ErrorMessage = "Los honorarios médicos son obligatorios.")]
        [Range(
            0.01,
            99999999.99,
            ErrorMessage = "Los honorarios deben ser mayores a cero."
        )]
        [Display(Name = "Honorarios médicos")]
        public decimal HonorariosMedico { get; set; }

        [Required(ErrorMessage = "Debe indicar los detalles del precio.")]
        [StringLength(
            100,
            ErrorMessage = "Los detalles no pueden superar los 100 caracteres."
        )]
        public string Detalles { get; set; } = string.Empty;

        public bool Estado { get; set; }
    }
}