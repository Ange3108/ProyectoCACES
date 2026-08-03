using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.Procedimientos
{
    public class MostrarProcedimientosDTO
    {
       


        public string Nombre { get; set; } 
        public string NombreMedico { get; set; }      
       

        public DateTime Fecha { get; set; }             

        // true = Pendiente | false = Realizada o Cancelada
        public bool Estado{ get; set; }

        public string Descripcion { get; set; }
    }
}
