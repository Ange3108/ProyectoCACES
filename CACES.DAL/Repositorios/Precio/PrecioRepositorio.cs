using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.Precio;
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

        public async Task<List<Precios>> ObtenerTodosAsync()
        {
            return await _context.Precios
                .AsNoTracking()
                .Include(p => p.Medico)
                    .ThenInclude(m => m.Usuario)
                .Include(p => p.Medico)
                    .ThenInclude(m => m.Especialidad)
                .Include(p => p.Procedimiento)
                .OrderBy(p => p.Procedimiento.Nombre.ToLower())
                .ToListAsync();
        }

        public async Task<Precios?> ObtenerPorIdAsync(int idPrecio)
        {
            return await _context.Precios
                .Include(p => p.Medico)
                    .ThenInclude(m => m.Usuario)
                .Include(p => p.Medico)
                    .ThenInclude(m => m.Especialidad)
                .Include(p => p.Procedimiento)
                .FirstOrDefaultAsync(p =>
                    p.Id_Precio == idPrecio
                );
        }

        public async Task<Precios> ActualizarAsync(Precios precio)
        {
            precio.FechaDeModificacion = DateTime.UtcNow;

            _context.Precios.Update(precio);

            await _context.SaveChangesAsync();

            return precio;
        }
    }
}