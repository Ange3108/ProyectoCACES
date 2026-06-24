using System;
using System.Collections.Generic;
using System.Text;
using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace CACES.DAL.Repositorios.ArchivosHistorial
{
    public class ArchivoHistorialRepositorio : IArchivoHistorialRepositorio
    {
        private readonly CACESDbContext _context;

        public ArchivoHistorialRepositorio(CACESDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CrearArchivoAsync(ArchivoHistorial archivo)
        {
            await _context.ArchivosHistorial.AddAsync(archivo);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<ArchivoHistorial>> GetArchivosByHistorialAsync(int idHistorial)
        {
            return await _context.ArchivosHistorial
                .Where(x => x.IdHistorial == idHistorial)
                .OrderByDescending(x => x.FechaDeSubida)
                .ToListAsync();
        }
    }
}