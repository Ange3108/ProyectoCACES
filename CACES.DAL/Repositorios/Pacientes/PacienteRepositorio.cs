using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.Usuario;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.Pacientes
{
    public class PacienteRepositorio : IPacienteRepositorio
    {
       
            //Inyerccion de dependencia de la BD
            private readonly CACESDbContext _context;

            public PacienteRepositorio(CACESDbContext context)
            {
                _context = context;
            }
            public Task<bool> CreatePacienteAsync(Paciente paciente)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeletePacienteAsync(int id)
        {
            var entity = await _context.Pacientes.FindAsync(id);
            if (entity == null) return false;

            _context.Pacientes.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public Task<Paciente> GetPacienteByDUIAsync(string dui)
        {
            throw new NotImplementedException();
        }

        public Task<Paciente> GetPacienteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Paciente>> GetPacientesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdatePacienteAsync(Paciente paciente)
        {
            if (paciente == null) return false;
            var existing = await _context.Pacientes.FindAsync(paciente.IdPaciente);
            if (existing == null) return false;



            //Actualizar los campos
            existing.IdHistorial = paciente.IdHistorial;
            existing.IdUsuario = paciente.IdUsuario;

            _context.Pacientes.Update(existing);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
