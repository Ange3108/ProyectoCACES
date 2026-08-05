using CACES.DAL.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.Cirugia
{
    public class MostrarCirugiaDTO
    {
        public string NombrePaciente { get; set; } = null!;
        public string Procedimiento { get; set; } = null!;
        public DateTime FechaProcedimiento { get; set; }
        public TimeSpan HoraProcedimiento { get; set; }
        public EstadoCirugia Estado { get; set; } // Pendiente, Realizado, Cancelado
        public string MedicoResponsable { get; set; } = null!;
    }
}
