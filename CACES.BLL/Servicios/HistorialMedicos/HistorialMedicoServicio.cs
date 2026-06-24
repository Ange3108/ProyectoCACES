using System;
using System.Collections.Generic;
using System.Text;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.HistorialMedicos;

namespace CACES.BLL.Servicios.HistorialMedicos
{
    public class HistorialMedicoServicio : IHistorialMedicoServicio
    {
        private readonly IHistorialMedicoRepositorio _historialRepositorio;

        public HistorialMedicoServicio(IHistorialMedicoRepositorio historialRepositorio)
        {
            _historialRepositorio = historialRepositorio;
        }

        public async Task<HistorialMedico> GetHistorialByIdAsync(int id)
        {
            return await _historialRepositorio.GetHistorialByIdAsync(id);
        }

        public async Task<bool> UpdateHistorialAsync(HistorialMedico historial)
        {
            historial.FechaDeModificacion = DateTime.Now;

            return await _historialRepositorio.UpdateHistorialAsync(historial);
        }
    }
}