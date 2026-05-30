using System;
using System.Collections.Generic;
using System.Text;
using CACES.DAL.Entidades;

namespace CACES.BLL.Servicios.Paciente
{
    public interface IPacienteServicio
    {
        Task<List<DAL.Entidades.Paciente>> GetPacientesAsync();

        Task<DAL.Entidades.Paciente> GetPacienteByIdAsync(int id);

        Task<DAL.Entidades.Paciente> GetPacienteByDUIAsync(string dui);

        Task<bool> CreatePacienteAsync(
            DAL.Entidades.Paciente paciente);

        Task<bool> UpdatePacienteAsync(
            DAL.Entidades.Paciente paciente);

        Task<bool> DeletePacienteAsync(int id);
    }
}