using System;
using System.Collections.Generic;
using System.Text;
namespace CACES.BLL.DTOs.Cotizacion
{
    public class MostrarCotizacionDTO
    {
        public int IdCotizacion { get; set; }

        public string NombrePaciente { get; set; } = string.Empty;

        public string NombreMedico { get; set; } = string.Empty;

        public string NombreProcedimiento { get; set; } = string.Empty;

        public DateTime FechaSolicitud { get; set; }

        public decimal PrecioBase { get; set; }

        public decimal Descuento { get; set; }

        public decimal Impuesto { get; set; }

        public decimal Total { get; set; }

        public string? Observaciones { get; set; }

        public byte Estado { get; set; }

        public string EstadoTexto { get; set; } = string.Empty;
    }
}