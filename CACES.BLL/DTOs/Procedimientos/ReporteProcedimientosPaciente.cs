using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.Procedimientos
{
    public class ReporteProcedimientosPaciente
    {
        public string NombrePaciente { get; set; } = null!;
        public string Identificacion { get; set; } = null!;
        public string Procedimiento { get; set; } = null!;
        public DateTime FechaProcedimiento { get; set; }
        public string Estado { get; set; } = null!; // Pendiente, Realizado, Cancelado
        public string MedicoResponsable { get; set; } = null!;
        public string? Observaciones { get; set; }
    }
}
