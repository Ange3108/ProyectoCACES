using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.Horarios
{
    public class HorariosRepositorio : IHorariosRepositorio
    {
        private readonly CACESDbContext _context;

        public HorariosRepositorio(CACESDbContext context)
        {
            _context = context;
        }
        public async Task<bool> ActualizarHorarioDisponibleAsync(HorariosDisponibles horario)
        {
            if (horario == null) return false;
            var horarioExistente = await _context.HorariosDisponibles.FindAsync(horario.Id_Horario);
            if (horarioExistente == null) return false;

            horarioExistente.DiaSemana = horario.DiaSemana;
            horarioExistente.HoraInicio = horario.HoraInicio;

            horarioExistente.Activo = horario.Activo;
            horarioExistente.Id_Medico = horario.Id_Medico;

            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<bool> CrearHorarioDisponibleAsync(HorariosDisponibles horario)
        {
            if (horario == null) return false;
            await _context.HorariosDisponibles.AddAsync(horario);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DesactivarHorarioDisponibleAsync(int idHorario)
        {
            var horario = await _context.HorariosDisponibles.FindAsync(idHorario);
            if (horario == null) return false;
            horario.Activo = false;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<HorariosDisponibles?> GetHorarioDisponiblePorIdAsync(int idHorario)
        {
            return await _context.HorariosDisponibles.FindAsync(idHorario);
        }

            int filasAfectadas = await _context.SaveChangesAsync(cancellationToken);
            return filasAfectadas > 0;
        }
    }
}
