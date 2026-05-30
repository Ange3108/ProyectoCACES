using System;
using System.Collections.Generic;
using System.Text;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.Pacientes;

namespace CACES.BLL.Servicios.Paciente
{
    public class PacienteServicio : IPacienteServicio
    {
        private readonly IPacienteRepositorio _pacienteRepositorio;

        public PacienteServicio(
            IPacienteRepositorio pacienteRepositorio)
        {
            _pacienteRepositorio = pacienteRepositorio;
        }

        public async Task<List<DAL.Entidades.Paciente>>
            GetPacientesAsync()
        {
            return await _pacienteRepositorio
                .GetPacientesAsync();
        }

        public async Task<DAL.Entidades.Paciente>
            GetPacienteByIdAsync(int id)
        {
            return await _pacienteRepositorio
                .GetPacienteByIdAsync(id);
        }

        public async Task<DAL.Entidades.Paciente>
            GetPacienteByDUIAsync(string dui)
        {
            return await _pacienteRepositorio
                .GetPacienteByDUIAsync(dui);
        }

        public async Task<bool>
            CreatePacienteAsync(
            DAL.Entidades.Paciente paciente)
        {
            return await _pacienteRepositorio
                .CreatePacienteAsync(paciente);
        }

        public async Task<bool>
            UpdatePacienteAsync(
            DAL.Entidades.Paciente paciente)
        {
            return await _pacienteRepositorio
                .UpdatePacienteAsync(paciente);
        }

        public async Task<bool>
            DeletePacienteAsync(int id)
        {
            return await _pacienteRepositorio
                .DeletePacienteAsync(id);
        }
    }
}