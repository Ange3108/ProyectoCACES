using CACES.DAL.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.Horarios
{
    public interface IHorariosRepositorio
    {
     
        Task<List<HorariosDisponibles>> GetHorariosDisponiblesPorMedicoAsync(int idMedico);
        Task<HorariosDisponibles?> GetHorarioDisponiblePorIdAsync(int idHorario);
        Task<bool> CrearHorarioDisponibleAsync(HorariosDisponibles horario);
        Task<bool> ActualizarHorarioDisponibleAsync(HorariosDisponibles horario);
        Task<bool> DesactivarHorarioDisponibleAsync(int idHorario);
        Task<bool> TieneHorarioActivoAsync(int idMedico, int diaSemana, int idExcluir = 0);
    }
}
