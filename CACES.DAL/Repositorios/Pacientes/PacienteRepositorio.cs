using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.Pacientes
{
    public class PacienteRepositorio : IPacienteRepositorio
    {

        private readonly CACESDbContext _context;

        public PacienteRepositorio(CACESDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreatePacienteAsync(Paciente paciente)
        {
            if (paciente == null) return false;

            await _context.Pacientes.AddAsync(paciente);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletePacienteAsync(int id)
        {
            var entity = await _context.Pacientes.FindAsync(id);
            if (entity == null) return false;

            _context.Pacientes.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Paciente> GetPacienteByDUIAsync(string dui)
        {
            return await _context.Pacientes
                .Include(p => p.Usuario)
                .Include(p => p.HistorialMedico)
                .FirstOrDefaultAsync(p => p.Usuario.DUI == dui);
        }

        public async Task<Entidades.Usuario> GetInfoMedicaByIdAsync(int idUsuario)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);

            if (usuario == null) return null;

            usuario.Paciente = await _context.Pacientes
                .Include(p => p.HistorialMedico)
                .FirstOrDefaultAsync(p => p.IdUsuario == idUsuario);

            if (usuario.Paciente != null)
            {
                var ultimaCita = await _context.Citas
                    .Where(c => c.IdPaciente == usuario.Paciente.IdPaciente)
                    .OrderByDescending(c => c.IdCita)
                    .FirstOrDefaultAsync();

                if (ultimaCita != null)
                {
                    var recetaAsociada = await _context.Recetas
                        .FirstOrDefaultAsync(r => r.IdCita == ultimaCita.IdCita);

                    ultimaCita.Receta = recetaAsociada;
                }

                usuario.Paciente.Citas = ultimaCita != null
                ? new List<Cita> { ultimaCita }
                : new List<Cita>();
            }

            return usuario;
        }
        public async Task<Paciente> GetPacienteByIdAsync(int id)
        {
            return await _context.Pacientes
                .Include(p => p.Usuario)
                .Include(p => p.HistorialMedico)
                .FirstOrDefaultAsync(p => p.IdPaciente == id);
        }

        public async Task<List<Paciente>> GetPacientesAsync()
        {
            return await _context.Pacientes
                .Include(p => p.Usuario)
                .Include(p => p.HistorialMedico)
                .ToListAsync();
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

        public async Task<CACES.DAL.Entidades.Paciente> GetPacienteByUsuarioIdAsync(int idUsuario)
        {
            return await _context.Pacientes
                .Include(p => p.HistorialMedico)
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.IdUsuario == idUsuario);
        }

        public async Task<Paciente?> ObtenerPorUsuarioIdAsync(int idUsuario)
        {
            return await _context.Pacientes
                .FirstOrDefaultAsync(p => p.IdUsuario == idUsuario);
        }

        public async Task<IEnumerable<Paciente>> ObtenerPacientesActivosAsync()
        {
            return await _context.Pacientes
                .Include(p => p.Usuario) 
                .Where(p => p.Usuario.Estado == true) 
                .OrderBy(p => p.Usuario.PrimerApellido)
                .ToListAsync();
        }
    }
}