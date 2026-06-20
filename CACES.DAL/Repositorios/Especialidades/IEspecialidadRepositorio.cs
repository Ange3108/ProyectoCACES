using CACES.DAL.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.Especialidades
{
    public interface IEspecialidadRepositorio
    {
        Task<List<Entidades.Especialidad>> GetEspecialidadesAsync();
        Task<Entidades.Especialidad> GetEspecialidadByIdAsync(int id);
        Task<Entidades.Especialidad> GetEspecialidadByNameAsync(string name);
        Task<List<Especialidad>> GetEspecialidadesActivasAsync();
        Task<Especialidad?> GetEspecialidadDetallesByIdAsync(int id);
        Task<bool> CreateEspecialidadAsync(Entidades.Especialidad especialidad);
        Task<bool> UpdateEspecialidadAsync(Entidades.Especialidad especialidad);
        Task<bool> DesactivarEspecialidadAsync(int id);
    }
}
