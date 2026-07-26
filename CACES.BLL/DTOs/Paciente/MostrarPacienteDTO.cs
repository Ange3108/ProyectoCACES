using CACES.BLL.DTOs.Usuario;

namespace CACES.BLL.DTOs.Paciente
{
    public class MostrarPacienteDTO
    {
        public int IdPaciente { get; set; }
        public int IdHistorial { get; set; }
        public MostrarUsuarioDTO Usuario { get; set; } = null!;
    }
}