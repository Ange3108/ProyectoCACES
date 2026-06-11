using System;
using System.Collections.Generic;
using System.Text;

using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace CACES.DAL.Repositorios.Citas
{
    public class CitaRepositorio : ICitaRepositorio
    {
        private readonly CACESDbContext _context;

        public CitaRepositorio(CACESDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cita>> GetCitasAsync()
        {
            return await _context.Citas
                .OrderBy(c => c.FechaCita)
                .ThenBy(c => c.Hora)
                .ToListAsync();
        }

        public async Task<Cita?> GetCitaByIdAsync(int idCita)
        {
            return await _context.Citas
                .FirstOrDefaultAsync(c => c.IdCita == idCita);
        }

        public async Task<bool> ActualizarFechaCitaAsync(int idCita, DateTime nuevaFecha)
        {
            var cita = await _context.Citas
                .FirstOrDefaultAsync(c => c.IdCita == idCita);

            if (cita == null)
                return false;

            cita.FechaCita = nuevaFecha.Date;
            cita.FechaDeModificacion = DateTime.Now;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CancelarCitasPorMedicoYFechaAsync(int idMedico, DateTime fechaCita)
        {
            var citas = await _context.Citas
                .Where(c => c.IdMedico == idMedico &&
                            c.FechaCita.Date == fechaCita.Date &&
                            c.Estado == 1)
                .ToListAsync();

            if (!citas.Any())
                return false;

            foreach (var cita in citas)
            {
                cita.Estado = 0;
                cita.FechaDeModificacion = DateTime.Now;
            }

            return await _context.SaveChangesAsync() > 0;
        }
    }
}