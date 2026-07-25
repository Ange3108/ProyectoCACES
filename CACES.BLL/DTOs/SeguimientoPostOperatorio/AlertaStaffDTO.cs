using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.SeguimientoPostOperatorio
{
    public class AlertaStaffDTO
    {
        public int idAlerta { get; set; }
        public int IdSeguimiento { get; set; }
        public int IdCirugia { get; set; }        // útil para mostrar directo en la tabla, sin otro query
        public DateTime FechaGenerada { get; set; }
        public string Estado { get; set; } = null!;
        public string? NombreUsuarioAtendio { get; set; }
        public string? Observaciones { get; set; }
        public DateTime? FechaAtencion { get; set; }
    }
}
