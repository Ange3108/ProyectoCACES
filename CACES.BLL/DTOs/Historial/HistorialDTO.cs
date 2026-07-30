using System.ComponentModel.DataAnnotations;

namespace CACES.BLL.DTOs.Historial
{
    public class HistorialDTO
    {
        public int IdHistorial { get; set; }
        [Required(ErrorMessage ="El tipo de sangre es requerido")]
        public string TipoSangre { get; set; } = null!;

        [Required(ErrorMessage = "Ingrese sus medicamentos actuales, si consume alguno")]
        public string Medicamentos { get; set; } = null!;

        [Required(ErrorMessage ="Sus alergías son requeridas")]
        public string Alergias { get; set; } = null!;

        [Required(ErrorMessage ="Ingrese sus enfermedades crónicas, si padece de alguna")]
        public string EnfermedadesCronicas { get; set; } = null!;

        [Required(ErrorMessage = "Sus antecedentes son requeridos")]
        public string Antecedentes { get; set; } = null!;

        [Required(ErrorMessage ="Si tiene algo más que comentar, ingreselo aquí")]
        public string Detalles { get; set; } = null!;
    }
}