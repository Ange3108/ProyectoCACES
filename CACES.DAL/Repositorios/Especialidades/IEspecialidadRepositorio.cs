using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.Especialidades
{
    public interface IEspecialidadRepositorio
    {
        Task<List<Entidades.>> GetEspecialidadesAsync();
        Task<Entidades.Especialidad> GetEspecialidadByIdAsync(int id);
        Task<Entidades.Especialidad> GetEspecialidadByNameAsync(string name);
        Task<bool> CreateEspecialidadAsync(Entidades.Especialidad especialidad);
        Task<bool> UpdateEspecialidadAsync(Entidades.Especialidad especialidad);
        Task<bool> DeleteEspecialidadAsync(int id);
        Task<bool> DesactivarEspecialidadAsync(int id);
    }
}
