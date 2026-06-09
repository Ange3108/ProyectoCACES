using CACES.BLL.DTOs.Historial;
using CACES.BLL.DTOs.Usuario;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CACES.BLL.DTOs.Paciente
{
    public class RegistrarPacienteDTO
    {
        [Required]
        public RegistrarUsuarioDTO Usuario { get; set; } = new RegistrarUsuarioDTO();

        [Required]
        public HistorialDTO Historial { get; set; } = new HistorialDTO();
    }
}