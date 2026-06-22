using System;
using System.Collections.Generic;
using System.Text;
 
namespace CACES.BLL.DTOs.Medico
{
    public class mostrarMedicoEspecialidadDTO
    {
        public int IdMedico { get; set; }
        public string Nombres { get; set; } = null!;
        public string PrimerApellido { get; set; } = null!;
        public string SegundoApellido { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string? Foto { get; set; }
    }
}