using CACES.BLL.DTOs.Historial;
using CACES.BLL.DTOs.Usuario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CACES.BLL.DTOs.Paciente
{
    public class RegistrarPacienteDTO
    {
        // Usuario
        [Required]
        public RegistrarUsuarioDTO Usuario { get; set; }

        // Historial Médico
        [Required]
        public HistorialDTO Historial { get; set; }
    }
}