using System;
using System.Collections.Generic;
using System.Text;
namespace CACES.BLL.DTOs.Receta
{
    public class MostrarRecetaDTO
    {
        public int IdReceta { get; set; }
        public int IdCita { get; set; }

        public string NombrePaciente { get; set; } = string.Empty;
        public string NombreMedico { get; set; } = string.Empty;
        public string NombreEspecialidad { get; set; } = string.Empty;

        public string Medicamentos { get; set; } = string.Empty;
        public string? Instrucciones { get; set; }

        public DateTime FechaDeRegistro { get; set; }
        public DateTime FechaDeVencimiento { get; set; }

        public bool Vencida => FechaDeVencimiento.Date < DateTime.Today;

        public string EstadoTexto => Vencida
            ? "Vencida"
            : "Vigente";
    }
}