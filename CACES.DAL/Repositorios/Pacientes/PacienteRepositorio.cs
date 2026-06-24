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

            {
                // 1. Una sola consulta eficiente usando proyecciones y sin rastreo de memoria
                var datos = await _context.Usuarios
                    .AsNoTracking() // ◄ Evita que EF guarde esto en caché de seguimiento (mejora radical de velocidad)
                    .Where(u => u.IdUsuario == id)
                    .Select(u => new
                    {
                        Usuario = u,
                        Paciente = u.Paciente,
                        Historial = u.Paciente != null ? u.Paciente.HistorialMedico : null,
                        // 2. Traemos la última receta directamente en la misma consulta de SQL
                        UltimaReceta = u.Paciente != null
                            ? _context.Citas
                                .AsNoTracking()
                                .Where(c => c.IdPaciente == u.Paciente.IdPaciente)
                                .OrderByDescending(c => c.IdCita)
                                .Select(c => c.Receta) // Asumiendo relación directa Cita -> Receta
                                .FirstOrDefault()
                            : null
                    })
                    .FirstOrDefaultAsync();

                if (datos == null) return null;

                var usuario = datos.Usuario;

                // 3. Reconstruimos los objetos en memoria sin volver a tocar la base de datos
                if (usuario.Paciente != null)
                {
                    usuario.Paciente.HistorialMedico = datos.Historial;

                    if (datos.UltimaReceta != null)
                    {
                        usuario.Paciente.Cita = new Cita
                        {
                            IdCita = datos.UltimaReceta.IdCita,
                            IdPaciente = usuario.Paciente.IdPaciente,
                            Receta = datos.UltimaReceta
                        };
                    }
                }

                return usuario;

            }
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
    }
}