using CACES.BLL.DTOs.Convenios;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Convenios
{
    public interface IConvenioServicio
    {
        Task<List<MostrarConvenios>> GetConveniosAsync();
        Task<List<MostrarConvenios>> GetConveniosSoloActivosAsync();
        Task<MostrarConvenios?> GetConvenioByIdAsync(int id);
        Task<bool> CreateConvenioAsync(CrearModificarConvenio dto);
        Task<bool> UpdateConvenioAsync(int id, CrearModificarConvenio dto);
    }
}
