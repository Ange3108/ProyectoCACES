using CACES.DAL.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.Pacientes
{
    public interface IPacienteRepositorio
    {
        Task<List<Entidades.Paciente>> GetPacientesAsync();
        Task<Entidades.Paciente> GetPacienteByIdAsync(int id);
        Task<Entidades.Paciente> GetPacienteByDUIAsync(string dui);
        Task<bool> CreatePacienteAsync(Entidades.Paciente paciente);
        Task<bool> UpdatePacienteAsync(Entidades.Paciente paciente);
        Task<bool> DeletePacienteAsync(int id);
        Task<Entidades.Usuario> GetInfoMedicaByIdAsync(int id);
        Task<Paciente?> ObtenerPorUsuarioIdAsync(int idUsuario);
    }
}
