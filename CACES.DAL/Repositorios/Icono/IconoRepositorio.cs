using CACES.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.Icono
{
    public class IconoRepositorio : IIconoRepositorio
    {

        private readonly CACESDbContext _context;

        public IconoRepositorio(CACESDbContext context)
        {
            _context = context;
        }
        public async Task<bool> ActualizarAsync(Entidades.Icono icono)
        {
            _context.Iconos.Update(icono);
            var filas = await _context.SaveChangesAsync();
            return filas > 0;
        }

        public async Task<bool> CrearAsync(Entidades.Icono icono)
        {
            _context.Iconos.Add(icono);
            var filas = await _context.SaveChangesAsync();
            return filas > 0;
        }

        public async Task<Entidades.Icono?> GetPorIdAsync(int id)
        {
            return await _context.Iconos
            .FirstOrDefaultAsync(i => i.IdIcono == id);
        }

        public async Task<List<Entidades.Icono>> GetTodosLosIconosAsync()
        {
            return await _context.Iconos
            .OrderBy(i => i.Nombre)
            .ToListAsync();
        }
    }
}
