using System;
using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace CACES.DAL.Repositorios.HistorialMedicos
{
    public class HistorialMedicoRepositorio
        : IHistorialMedicoRepositorio
    {
        private readonly CACESDbContext _context;

        public HistorialMedicoRepositorio(
            CACESDbContext context)
        {
            _context = context;
        }

        public async Task<HistorialMedico>
            CreateHistorialAsync(
            HistorialMedico historial)
        {
            await _context.HistorialesMedicos
                .AddAsync(historial);

            await _context.SaveChangesAsync();

            return historial;
        }

        public async Task<HistorialMedico>
            GetHistorialByIdAsync(int id)
        {
            return await _context.HistorialesMedicos
                .FirstOrDefaultAsync(
                    h => h.IdHistorial == id);
        }

        public async Task<bool>
            UpdateHistorialAsync(
            HistorialMedico historial)
        {
            _context.HistorialesMedicos
                .Update(historial);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}