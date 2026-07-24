using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.SeguimientoPostOperatorio
{
    public class MostrarSeguimientoPacienteDTO
    {
        public int IdSeguimiento { get; set; }
        public int IdCirugia { get; set; }
        public int DiaCheckpoint { get; set; }
        public DateTime FechaProgramada { get; set; }
        public string Estado { get; set; } = null!;
        public DateTime? FechaRegistro { get; set; }
        
    }
}
