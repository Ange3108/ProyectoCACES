using System;
using System.Collections.Generic;
using System.Text;
using CACES.DAL.Entidades;

namespace CACES.BLL.Servicios.HistorialMedicos
{
    public interface IHistorialMedicoServicio
    {
        Task<HistorialMedico> GetHistorialByIdAsync(int id);

        Task<bool> UpdateHistorialAsync(HistorialMedico historial);

    }
}
