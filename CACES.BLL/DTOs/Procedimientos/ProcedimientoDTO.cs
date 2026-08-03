using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.Procedimientos
{
    public class ProcedimientoDTO
    {
        public int Id_Procedimiento { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal? PrecioBase { get; set; }
        public bool Estado { get; set; }
    }
}
