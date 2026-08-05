using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.Precio
{
    public class MostrarPrecioDTO
    {
        public int IdPrecio { get; set; }

        public int IdMedico { get; set; }

        public int IdProcedimiento { get; set; }

        public string NombreMedico { get; set; } = string.Empty;

        public string NombreEspecialidad { get; set; } = string.Empty;

        public string NombreProcedimiento { get; set; } = string.Empty;

        public decimal PrecioBase { get; set; }

        public decimal HonorariosMedico { get; set; }

        public string Detalles { get; set; } = string.Empty;

        public bool Estado { get; set; }

        public string EstadoTexto =>
            Estado ? "Activo" : "Inactivo";

        public DateTime FechaDeRegistro { get; set; }

        public DateTime? FechaDeModificacion { get; set; }
    }
}