using System.ComponentModel.DataAnnotations;

namespace CACES.BLL.DTOs.Historial
{
    public class HistorialDTO
    {
        public int IdHistorial { get; set; }
        [Required]
        public string TipoSangre { get; set; } = null!;

        [Required]
        public string Medicamentos { get; set; } = null!;

        [Required]
        public string Alergias { get; set; } = null!;

        [Required]
        public string EnfermedadesCronicas { get; set; } = null!;

        [Required]
        public string Antecedentes { get; set; } = null!;

        [Required]
        public string Detalles { get; set; } = null!;
    }
}