using CACES.BLL.DTOs.Especialidad;
using CACES.BLL.DTOs.Usuario;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.Medico
{
    public class MedicoDTO
    {
        public int IdMedico { get; set; }
        public int Experiencia { get; set; }
        public string Certificaciones { get; set; } = null!;
        public string NombreEspecialidad { get; set; } = null!;
        public MostrarUsuarioDTO Usuario { get; set; } = null!;
        
    }
}
