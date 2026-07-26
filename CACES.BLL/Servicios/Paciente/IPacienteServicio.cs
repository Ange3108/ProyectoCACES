using System;
using System.Collections.Generic;
using System.Text;
using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Paciente;
using CACES.BLL.DTOs.Usuario;

namespace CACES.BLL.Servicios.Paciente
{
    public interface IPacienteServicio
    {
        Task<respuestaErrores<List<MostrarPacienteDTO>>> GetPacientesAsync();

        Task<respuestaErrores<MostrarPacienteDTO?>> GetPacienteByIdAsync(int id);

        Task<respuestaErrores<MostrarPacienteDTO?>> GetPacienteByDUIAsync(string dui);

        Task<respuestaErrores<MostrarUsuarioDTO>> CreatePacienteAsync(RegistrarPacienteDTO pacienteDto);

        Task<respuestaErrores<bool>> DesactivarPacienteAsync(int idPaciente);

        Task<respuestaErrores<bool>> RegistrarPacienteAsync(RegistrarPacienteDTO pacienteDto);

        Task<respuestaErrores<MostrarPacienteDTO>> GetPacienteByUsuarioIdAsync(int idUsuario);

        Task<int> ObtenerIdPacientePorUsuarioIdAsync(int idUsuario);

        Task<respuestaErrores<IEnumerable<MostrarPacienteDTO>>> ObtenerPacientesActivosAsync();
        Task<respuestaErrores<bool>> ActivarPacienteAsync(int idPaciente);
    }
}