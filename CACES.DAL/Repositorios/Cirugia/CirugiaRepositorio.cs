using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.Cirugia
{
    public class CirugiaRepositorio : ICirugiaRepositorio
    {
        private readonly CACESDbContext _context;

        public CirugiaRepositorio(CACESDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CambiarEstadoAsync(int id)
        {
            var cirugiaExistente = await _context.Cirugias
                .Include(c => c.Cita)
                .FirstOrDefaultAsync(c => c.Id_Cirugia == id);

            if (cirugiaExistente == null)
            {
                return false;
            }

            if (cirugiaExistente.Cita.Fecha <= DateTime.UtcNow)
            {
                cirugiaExistente.Estado = EstadoCirugia.Finalizada; 
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Cirugias>> conseguirCirugiaPorPAciente(int paciente)
        {

            return await _context.Cirugias.AsNoTracking()
                .Include(c => c.Paciente).ThenInclude(p => p.Usuario)
                .Include(c => c.Medico)
                    .ThenInclude(m => m.Usuario)
                 .Include(c => c.Procedimiento)
                 .Include(c => c.Cita)
                 .Include(c=> c.Horario)
                 .Where(c => c.Id_Paciente == paciente)
                .OrderByDescending(c => c.Cita.Fecha)
                .ToListAsync();
        }

        public async Task<Cirugias?> ObtenerConDetalleAsync(int id)
        {
            return await _context.Cirugias
                .Include(c => c.Paciente).ThenInclude(p => p.Usuario)
                .Include(c => c.Medico).ThenInclude(m => m.Usuario)
                .Include(c => c.Procedimiento)
                .Include(c => c.Cita)
                .Include(c => c.Horario)
                .FirstOrDefaultAsync(c => c.Id_Cirugia == id);
        }

        public async Task<List<Cirugias>> ObtenerTodosConDetalleAsync()
        {
            return await _context.Cirugias
                .Include(c => c.Paciente).ThenInclude(p => p.Usuario)
                .Include(c => c.Medico).ThenInclude(m => m.Usuario)
                .Include(c => c.Procedimiento)
                .Include(c => c.Cita)
                .Include(c => c.Horario)
                .OrderByDescending(c => c.Cita.Fecha)
                .ToListAsync();
        }
    }
}
