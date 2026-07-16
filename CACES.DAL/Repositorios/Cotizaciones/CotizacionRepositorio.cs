using System;
using System.Collections.Generic;
using System.Text;
using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using Microsoft.EntityFrameworkCore;

namespace CACES.DAL.Repositorios.Cotizaciones
{
    public class CotizacionRepositorio : ICotizacionRepositorio
    {
        private readonly CACESDbContext _context;

        public CotizacionRepositorio(CACESDbContext context)
        {
            _context = context;
        }

        public async Task<Cotizacion> RegistrarAsync(Cotizacion cotizacion)
        {
            cotizacion.FechaDeRegistro = DateTime.Now;
            cotizacion.FechaSolicitud = DateTime.Now;
            cotizacion.Estado = 1;

            await _context.Cotizaciones.AddAsync(cotizacion);
            await _context.SaveChangesAsync();

            return cotizacion;
        }

        public async Task<Cotizacion> ActualizarAsync(Cotizacion cotizacion)
        {
            cotizacion.FechaDeModificacion = DateTime.Now;

            _context.Cotizaciones.Update(cotizacion);

            await _context.SaveChangesAsync();

            return cotizacion;
        }

        public async Task<Cotizacion?> ObtenerPorIdAsync(int idCotizacion)
        {
            return await _context.Cotizaciones

                .Include(c => c.Paciente)
                    .ThenInclude(p => p.Usuario)

                .Include(c => c.Medico)
                    .ThenInclude(m => m.Usuario)

                .Include(c => c.Procedimiento)

                .FirstOrDefaultAsync(c =>
                    c.IdCotizacion == idCotizacion);
        }

        public async Task<List<Cotizacion>> ObtenerTodasAsync()
        {
            return await _context.Cotizaciones

                .AsNoTracking()

                .Include(c => c.Paciente)
                    .ThenInclude(p => p.Usuario)

                .Include(c => c.Medico)
                    .ThenInclude(m => m.Usuario)

                .Include(c => c.Procedimiento)

                .OrderByDescending(c => c.FechaSolicitud)

                .ToListAsync();
        }

        public async Task<List<Cotizacion>> ObtenerPorPacienteAsync(int idPaciente)
        {
            return await _context.Cotizaciones

                .AsNoTracking()

                .Include(c => c.Medico)
                    .ThenInclude(m => m.Usuario)

                .Include(c => c.Procedimiento)

                .Where(c => c.IdPaciente == idPaciente)

                .OrderByDescending(c => c.FechaSolicitud)

                .ToListAsync();
        }

        public async Task<List<Paciente>> ObtenerPacientesAsync()
        {
            return await _context.Pacientes

                .AsNoTracking()

                .Include(p => p.Usuario)

                .Where(p => p.Usuario.Estado)

                .OrderBy(p => p.Usuario.Nombres)

                .ToListAsync();
        }

        public async Task<List<Medico>> ObtenerMedicosAsync()
        {
            return await _context.Medicos

                .AsNoTracking()

                .Include(m => m.Usuario)

                .Include(m => m.Especialidad)

                .Where(m => m.Usuario.Estado)

                .OrderBy(m => m.Usuario.Nombres)

                .ToListAsync();
        }

        public async Task<List<Procedimiento>> ObtenerProcedimientosAsync()
        {
            return await _context.Procedimientos

                .AsNoTracking()

                .Where(p => p.Estado)

                .OrderBy(p => p.Nombre)

                .ToListAsync();
        }
    }

}