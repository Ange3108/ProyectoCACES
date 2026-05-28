using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.Medicos
{

    public interface IMedicoRepositorio
    {
        Task<List<Entidades.Medico>> GetMedicosAsync();

        Task<Entidades.Medico> GetMedicoByIdAsync(int id);

        Task<Entidades.Medico> GetMedicoByEspecialidadAsync(string especialidad);

        Task<bool> CreateMedicoAsync(Entidades.Medico medico);

        Task<bool> UpdateMedicoAsync(Entidades.Medico medico);

        Task<bool> DeleteMedicoAsync(int id);
    }
}