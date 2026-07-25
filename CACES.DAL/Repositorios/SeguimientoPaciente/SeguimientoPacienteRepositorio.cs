using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using CACES.DAL.Entidades.SeguimientoPostOperatorio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CACES.DAL.Repositorios.SeguimientoPaciente
{
    public class SeguimientoPacienteRepositorio : ISeguimientoPacienteRepositorio
    {
        private readonly CACESDbContext _context;

        public SeguimientoPacienteRepositorio(CACESDbContext context)
        {
            _context = context;
        }

        public async Task<List<Entidades.SeguimientoPostOperatorio.SeguimientoPaciente>> ObtenerTodos()
        {
            return await _context.SeguimientoPacientes
                .OrderByDescending(s => s.FechaProgramada)
                .ToListAsync();
        }

        public async Task AgregarRango(List<Entidades.SeguimientoPostOperatorio.SeguimientoPaciente> entidades)
        {
            _context.SeguimientoPacientes.AddRange(entidades);
            await _context.SaveChangesAsync();
        }

        public async Task<Cirugias?> ObtenerCirugiaConFecha(int idCirugia)
        {
            return await _context.Cirugias
                .Include(c => c.Cita)
                    .ThenInclude(cita => cita!.Horario)
                .FirstOrDefaultAsync(c => c.Id_Cirugia == idCirugia);
        }

        public async Task<List<Entidades.SeguimientoPostOperatorio.SeguimientoPaciente>> ObtenerPorCirugia(int idCirugia)
        {
            return await _context.SeguimientoPacientes
                .Where(s => s.Id_Cirugia == idCirugia)
                .OrderBy(s => s.DiaCheckpoint)
                .ToListAsync();
        }

        public async Task<List<Entidades.SeguimientoPostOperatorio.SeguimientoPaciente>> ObtenerProgramadosParaHoy()
        {
            var hoy = DateTime.Today;
            return await _context.SeguimientoPacientes
                .Where(s => s.Estado == EstadoSeguimiento.Pendiente && s.FechaProgramada.Date == hoy)
                .ToListAsync();
        }
    }
}