using System;

namespace CACES.BLL.DTOs.Cotizacion
{
    public class MostrarCotizacionDTO
    {
        public int IdCotizacion { get; set; }

        public string NombrePaciente { get; set; } = string.Empty;

        public string NombreMedico { get; set; } = string.Empty;

        public string NombreProcedimiento { get; set; } = string.Empty;

        public DateTime FechaSolicitud { get; set; }

        // =============================
        // DESGLOSE DE COSTOS
        // =============================

        public decimal PrecioBase { get; set; }

        public decimal HonorariosMedico { get; set; }

        public decimal CostoEquipo { get; set; }

        public decimal CostoEstadia { get; set; }

        public int DiasEstadia { get; set; }

        public decimal Descuento { get; set; }

        public decimal Impuesto { get; set; }

        public decimal Total { get; set; }

        // =============================
        // INFORMACIÓN
        // =============================

        public string? Observaciones { get; set; }

        public byte Estado { get; set; }

        public string EstadoTexto { get; set; } = string.Empty;

        // =============================
        // PROPIEDADES CALCULADAS
        // =============================

        public string NumeroCotizacion
            => $"COT-{IdCotizacion:D6}";

        public bool PuedeDescargarPdf
            => Total > 0;

        public bool EstaPendiente
            => Estado == 1;

        public bool EstaAprobada
            => Estado == 3;

        public bool EstaRechazada
            => Estado == 4;
    }
}