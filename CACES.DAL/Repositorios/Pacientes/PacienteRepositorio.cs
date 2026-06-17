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

        public async Task<Entidades.Usuario> GetInfoMedicaByIdAsync(int id)
        {

            //*Mover esto a la logiva de paciente, no es responsabilidad del repositorio de usuario traer la receta, ademas de que esta consulta es muy pesada y puede afectar el rendimientoreturn await _context.Usuarios
               var usuario = await _context.Usuarios
                   .Include(u => u.Paciente)
                   .ThenInclude(p => p.HistorialMedico)
                   .Include(u => u.Paciente)
                   .FirstOrDefaultAsync(u => u.IdUsuario == id);
            if (usuario?.Paciente != null)
            {
                var ultimaReceta = await (from cita in _context.Citas
                                          join receta in _context.Set<Receta>() on cita.IdCita equals receta.IdCita
                                          where cita.IdPaciente == usuario.Paciente.IdPaciente
                                          orderby cita.IdCita descending // Trae la última cita registrada
                                          select receta)
                                         .FirstOrDefaultAsync();

                if (ultimaReceta != null)
                {
                    usuario.Paciente.Cita = new Cita
                    {
                        IdCita = ultimaReceta.IdCita,
                        IdPaciente = usuario.Paciente.IdPaciente,
                        Receta = ultimaReceta
                    };
                }
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
    }
}
