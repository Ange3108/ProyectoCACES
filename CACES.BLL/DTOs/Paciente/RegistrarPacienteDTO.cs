using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.Paciente
{
    public class RegistrarPacienteDTO
    {
        // Usuario
        public string Nombres { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string CorreoElectronico { get; set; }
        public string DUI { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public int Edad { get; set; }
        public string Password { get; set; }

        // Historial Médico
        public string TipoSangre { get; set; }
        public string Alergias { get; set; }
        public string EnfermedadesCronicas { get; set; }
        public string Antecedentes { get; set; }
        public string Detalles { get; set; }
    }
}