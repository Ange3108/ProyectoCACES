using CACES.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using CACES.DAL.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.Convenios
{
    public class ConvenioRepositorio : IConvenioRepositorio
    {
        private readonly CACESDbContext _context;

        public ConvenioRepositorio(CACESDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateConvenioAsync(DAL.Entidades.Convenios convenio)
        {
            if (convenio == null)
                return false;

            await _context.Convenios.AddAsync(convenio);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<DAL.Entidades.Convenios>> GetConveniosAsync()
        {
            return await _context.Convenios.ToListAsync();
        }

        public async Task<List<DAL.Entidades.Convenios>> GetConveniosSoloActivosAsync()
        {
            return await _context.Convenios.Where(c => c.Estado).ToListAsync();
        }

        public async Task<bool> UpdateConvenioAsync(DAL.Entidades.Convenios convenio)
        {
            if (convenio == null) return false;

            _context.Convenios.Update(convenio);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<DAL.Entidades.Convenios?> GetConvenioByIdAsync(int id)
        {
            return await _context.Convenios.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
