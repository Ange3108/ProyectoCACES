using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.Procedimientos
{
    public class MostrarProcedimientosDTO
    {
        public string Nombre { get; set; } // Ej: "Cirugía de vesícula biliar"
        public string NombreMedico { get; set; }         // Ej: "Dr. Juan Pérez"
        public DateTime Fecha { get; set; }             // Ej: 15/06/2026

        // true = Pendiente | false = Realizada o Cancelada
        public bool Estado{ get; set; }

        // Ahora es dinámico y vendrá de la base de datos
        public string Descripcion { get; set; }
    }
}
