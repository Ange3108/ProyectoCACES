using System;
using System.Collections.Generic;
using System.Text;
using CACES.BLL.DTOs.Paciente;
using CACES.BLL.DTOs.Usuario;

namespace CACES.BLL.Servicios.Paciente
{
    public interface IPacienteServicio
    {
        Task<List<DAL.Entidades.Paciente>> GetPacientesAsync();

        Task<DAL.Entidades.Paciente?> GetPacienteByIdAsync(int id);

        Task<DAL.Entidades.Paciente?> GetPacienteByDUIAsync(string dui);

        Task<MostrarUsuarioDTO> CreatePacienteAsync(RegistrarPacienteDTO pacienteDto);

        Task<bool> DesactivarPacienteAsync(int idPaciente);

        Task<bool> RegistrarPacienteAsync(RegistrarPacienteDTO pacienteDto);

        Task<CACES.DAL.Entidades.Paciente> GetPacienteByUsuarioIdAsync(int idUsuario);

        Task<int> ObtenerIdPacientePorUsuarioIdAsync(int idUsuario);
    }
}