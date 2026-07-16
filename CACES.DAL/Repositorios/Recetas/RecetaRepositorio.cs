using System;
using System.Collections.Generic;
using System.Text;
using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace CACES.DAL.Repositorios.Recetas
{
    public class RecetaRepositorio : IRecetaRepositorio
    {
        private readonly CACESDbContext _context;

        public RecetaRepositorio(CACESDbContext context)
        {
            _context = context;
        }

        public async Task<Receta?> ObtenerPorIdAsync(int idReceta)
        {
            return await _context.Recetas
                .AsNoTracking()
                .Include(r => r.Cita)
                    .ThenInclude(c => c.Paciente)
                        .ThenInclude(p => p.Usuario)
                .Include(r => r.Cita)
                    .ThenInclude(c => c.Medico)
                        .ThenInclude(m => m.Usuario)
                .Include(r => r.Cita)
                    .ThenInclude(c => c.Especialidad)
                .FirstOrDefaultAsync(r => r.IdReceta == idReceta);
        }

        public async Task<Receta?> ObtenerPorCitaAsync(int idCita)
        {
            return await _context.Recetas
                .AsNoTracking()
                .Include(r => r.Cita)
                    .ThenInclude(c => c.Paciente)
                        .ThenInclude(p => p.Usuario)
                .Include(r => r.Cita)
                    .ThenInclude(c => c.Medico)
                        .ThenInclude(m => m.Usuario)
                .Include(r => r.Cita)
                    .ThenInclude(c => c.Especialidad)
                .FirstOrDefaultAsync(r => r.IdCita == idCita);
        }

        public async Task<List<Receta>> ObtenerPorPacienteAsync(int idPaciente)
        {
            return await _context.Recetas
                .AsNoTracking()
                .Include(r => r.Cita)
                    .ThenInclude(c => c.Medico)
                        .ThenInclude(m => m.Usuario)
                .Include(r => r.Cita)
                    .ThenInclude(c => c.Especialidad)
                .Where(r => r.Cita.IdPaciente == idPaciente)
                .OrderByDescending(r => r.FechaDeRegistro)
                .ToListAsync();
        }

        public async Task<List<Receta>> ObtenerPorMedicoAsync(int idMedico)
        {
            return await _context.Recetas
                .AsNoTracking()
                .Include(r => r.Cita)
                    .ThenInclude(c => c.Paciente)
                        .ThenInclude(p => p.Usuario)
                .Include(r => r.Cita)
                    .ThenInclude(c => c.Especialidad)
                .Where(r => r.Cita.IdMedico == idMedico)
                .OrderByDescending(r => r.FechaDeRegistro)
                .ToListAsync();
        }

        public async Task<bool> ExistePorCitaAsync(int idCita)
        {
            return await _context.Recetas
                .AsNoTracking()
                .AnyAsync(r => r.IdCita == idCita);
        }

        public async Task<Receta> RegistrarAsync(Receta receta)
        {
            await _context.Recetas.AddAsync(receta);
            await _context.SaveChangesAsync();

            return receta;
        }

        public async Task<Receta> ActualizarAsync(Receta receta)
        {
            _context.Recetas.Update(receta);
            await _context.SaveChangesAsync();

            return receta;
        }
    }
}