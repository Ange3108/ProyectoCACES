using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.Procedimientos
{
    public class EditarProcedimientosDTO
    {
        public int Id_Procedimiento { get; set; }
        public string? Nombre { get; set; } 
        public string? NombreMedico { get; set; }      
        public string? NombrePaciente { get; set; }   
        
        public string? PrimerApellidoPaciente { get; set; }
        public string? SegundoApellidoPaciente { get; set; }

        public DateTime Fecha { get; set; }             

        // true = Pendiente | false = Realizada o Cancelada
        public bool Estado{ get; set; }

        public string Descripcion { get; set; } = string.Empty;
    }
}
