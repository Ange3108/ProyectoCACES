using CACES.DAL.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.Convenios
{
    public interface IConvenioRepositorio
    {
        Task<bool> CreateConvenioAsync(DAL.Entidades.Convenios convenio);
        Task<List<DAL.Entidades.Convenios>> GetConveniosAsync();
        Task<List<DAL.Entidades.Convenios>> GetConveniosSoloActivosAsync();
        Task<bool> UpdateConvenioAsync(DAL.Entidades.Convenios convenio);
        Task<DAL.Entidades.Convenios?> GetConvenioByIdAsync(int id);
    }
}
