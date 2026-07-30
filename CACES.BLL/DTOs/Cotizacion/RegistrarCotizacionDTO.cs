using System.ComponentModel.DataAnnotations;

namespace CACES.BLL.DTOs.Cotizacion
{
    public class RegistrarCotizacionDTO
    {
        [Required(ErrorMessage = "El paciente es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un paciente válido.")]
        public int IdPaciente { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un médico.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un médico válido.")]
        public int IdMedico { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un procedimiento.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un procedimiento válido.")]
        public int IdProcedimiento { get; set; }

        [Required(ErrorMessage = "Debe indicar los días de estadía.")]
        [Range(1, 30, ErrorMessage = "Los días de estadía deben estar entre 1 y 30.")]
        [Display(Name = "Días de estadía")]
        public int DiasEstadia { get; set; } = 1;

        [StringLength(
            500,
            ErrorMessage = "Las observaciones no pueden superar los 500 caracteres."
        )]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }
    }
}