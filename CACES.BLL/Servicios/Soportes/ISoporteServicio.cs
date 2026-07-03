using System;
using System.Collections.Generic;
using System.Text;
using CACES.DAL.Entidades;

namespace CACES.BLL.Servicios.Soportes
{
    public interface ISoporteServicio
    {
        Task<bool> CrearConsultaAsync(int idUsuario, string asunto, string mensaje);

        Task<List<Soporte>> GetConsultasAsync();

        Task<Soporte?> GetConsultaByIdAsync(int id);
    }
}