using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.Medico
{
    public class RegistrarMedicoDTO
    {
        public int IdEspecialidad { get; set; }
        public int IdUsuario { get; set; }
        public int Experiencia { get; set; }
        public string Telefono { get; set; } = null!;
        public string Certificaciones { get; set; } = null!;
    }
}
