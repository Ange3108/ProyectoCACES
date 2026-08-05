using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.SolicitudMedicos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.SolicitudMedicos
{
    public class SolicitudMedicoRepositorio
        : ISolicitudMedicoRepositorio
    {
        private readonly CACESDbContext _context;

        public SolicitudMedicoRepositorio(
            CACESDbContext context)
        {
            _context = context;
        }

        public async Task<SolicitudMedico> RegistrarAsync(
            SolicitudMedico solicitud)
        {
            solicitud.Estado = 1;
            solicitud.FechaSolicitud = DateTime.UtcNow;

            await _context.SolicitudesMedico
                .AddAsync(solicitud);

            await _context.SaveChangesAsync();

            return solicitud;
        }

        public async Task<List<SolicitudMedico>>
            ObtenerTodasAsync()
        {
            return await _context.SolicitudesMedico
                .AsNoTracking()
                .Include(s => s.Especialidad)
                .OrderBy(s => s.Estado)
                .ThenByDescending(s => s.FechaSolicitud)
                .ToListAsync();
        }

        public async Task<SolicitudMedico?>
            ObtenerPorIdAsync(int idSolicitud)
        {
            return await _context.SolicitudesMedico
                .Include(s => s.Especialidad)
                .FirstOrDefaultAsync(s =>
                    s.IdSolicitud == idSolicitud
                );
        }

        public async Task<SolicitudMedico?>
            ObtenerPendientePorCorreoAsync(
                string correoElectronico)
        {
            var correo = correoElectronico
                .Trim()
                .ToLower();

            return await _context.SolicitudesMedico
                .AsNoTracking()
                .FirstOrDefaultAsync(s =>
                    s.CorreoElectronico.ToLower() == correo &&
                    (s.Estado == 1 || s.Estado == 2)
                );
        }

        public async Task<SolicitudMedico> ActualizarAsync(
            SolicitudMedico solicitud)
        {
            _context.SolicitudesMedico.Update(solicitud);

            await _context.SaveChangesAsync();

            return solicitud;
        }

        public async Task<List<Especialidad>>
            ObtenerEspecialidadesActivasAsync()
        {
            return await _context.Especialidades
                .AsNoTracking()
                .Where(e => e.Estado)
                .OrderBy(e => e.Nombre)
                .ToListAsync();
        }
    }
}