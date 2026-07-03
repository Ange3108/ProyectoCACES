using System;
using System.Collections.Generic;
using System.Text;
using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace CACES.DAL.Repositorios.Soportes
{
    public class SoporteRepositorio : ISoporteRepositorio
    {
        private readonly CACESDbContext _context;

        public SoporteRepositorio(CACESDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CrearConsultaAsync(Soporte soporte)
        {
            if (soporte == null)
                return false;

            soporte.FechaConsulta = DateTime.Now;
            soporte.Estado = true;

            await _context.Soportes.AddAsync(soporte);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Soporte>> GetConsultasAsync()
        {
            return await _context.Soportes
                .Include(s => s.Usuario)
                .OrderByDescending(s => s.FechaConsulta)
                .ToListAsync();
        }

        public async Task<Soporte?> GetConsultaByIdAsync(int id)
        {
            return await _context.Soportes
                .Include(s => s.Usuario)
                .FirstOrDefaultAsync(s => s.IdSoporte == id);
        }
    }
}