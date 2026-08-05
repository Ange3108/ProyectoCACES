using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.Precio
{
    public class MostrarPrecioDTO
    {
        public int IdPrecio { get; set; }
        public int IdMedico { get; set; }
        public string? NombreMedico { get; set; }
        public int IdProcedimiento { get; set; }
        public string? NombreProcedimiento { get; set; }
        public decimal Costo { get; set; }
        public string Detalles { get; set; } = string.Empty;
    }
}
