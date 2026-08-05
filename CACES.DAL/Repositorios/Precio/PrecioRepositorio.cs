using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.Precio
{
    public class PrecioRepositorio : IPrecioRepositorio
    {
        private readonly CACESDbContext _context;

        public PrecioRepositorio(CACESDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Precios>> GetAllAsync()
        {
            return await _context.Precios
                .Include(p => p.Medico)
                    .ThenInclude(m => m.Usuario)
                .Include(p => p.Procedimiento)
                .ToListAsync();
        }

        public async Task<Precios?> GetByIdAsync(int id)
        {
            return await _context.Precios
                .Include(p => p.Medico)
                    .ThenInclude(m => m.Usuario)
                .Include(p => p.Procedimiento)
                .FirstOrDefaultAsync(p => p.Id_Precio == id);
        }

        public async Task<IEnumerable<Precios>> GetByMedicoIdAsync(int idMedico)
        {
            return await _context.Precios
                .Include(p => p.Procedimiento)
                .Include(p => p.Medico)
                    .ThenInclude(m => m.Usuario)
                .Where(p => p.Id_Medico == idMedico)
                .ToListAsync();
        }

        public async Task<Precios?> GetByMedicoYProcedimientoAsync(int idMedico, int idProcedimiento)
        {
            return await _context.Precios
                .FirstOrDefaultAsync(p => p.Id_Medico == idMedico && p.Id_Procedimiento == idProcedimiento);
        }

        public async Task AddAsync(Precios precio)
        {
            await _context.Precios.AddAsync(precio);
        }

        public void Update(Precios precio)
        {
            _context.Precios.Update(precio);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
