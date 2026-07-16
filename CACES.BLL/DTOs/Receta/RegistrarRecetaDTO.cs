using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CACES.BLL.DTOs.Receta
{
    public class RegistrarRecetaDTO
    {
        [Required(ErrorMessage = "La cita es requerida.")]
        public int IdCita { get; set; }

        [Required(ErrorMessage = "Debe indicar los medicamentos.")]
        public string Medicamentos { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Las instrucciones no pueden superar los 500 caracteres.")]
        public string? Instrucciones { get; set; }

        [Required(ErrorMessage = "La fecha de vencimiento es requerida.")]
        public DateTime FechaDeVencimiento { get; set; }
    }
}