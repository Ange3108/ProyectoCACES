using System;
using CACES.DAL.Entidades;

namespace CACES.DAL.Repositorios.HistorialMedicos
{
    public interface IHistorialMedicoRepositorio
    {
        Task<HistorialMedico> CreateHistorialAsync(
            HistorialMedico historial);

        Task<HistorialMedico> GetHistorialByIdAsync(int id);

        Task<bool> UpdateHistorialAsync(
            HistorialMedico historial);
    }
}