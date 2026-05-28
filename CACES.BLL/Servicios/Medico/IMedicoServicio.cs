using System;
using CACES.DAL.Entidades;
using System.Collections.Generic;

namespace CACES.BLL.Servicios.Medicos
{
    public interface IMedicoServicio
    {
        Task<List<Medico>> GetMedicosAsync();

        Task<Medico> GetMedicoByIdAsync(int id);

        Task<bool> CreateMedicoAsync(Medico medico);

        Task<bool> UpdateMedicoAsync(Medico medico);

        Task<bool> DeleteMedicoAsync(int id);
    }
}