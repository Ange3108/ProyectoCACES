using System;
using CACES.DAL.Entidades;

namespace CACES.DAL.Repositorios.HistorialMedicos
{
    public interface IHistorialMedicoRepositorio
    {
        Task<HistorialMedico> CreateHistorialAsync(
            HistorialMedicos historial);

        Task<HistorialMedico> GetHistorialByIdAsync(int id);

        Task<bool> UpdateHistorialAsync(
            HistorialMedicos historial);
    }
}