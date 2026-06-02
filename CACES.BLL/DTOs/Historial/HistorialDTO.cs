using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CACES.BLL.DTOs.Historial
{
    public class HistorialDTO
    {
        [Required]
        public int IdPaciente { get; set; }
        // Historial Médico
        [Required]
        public string TipoSangre { get; set; }
        [Required]
        public string Alergias { get; set; }
        [Required]
        public string EnfermedadesCronicas { get; set; }
        [Required]
        public string Antecedentes { get; set; }
        [Required]
        public string Detalles { get; set; }
    }
}
