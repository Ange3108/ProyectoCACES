using System;
using System.Collections.Generic;
using System.Text;
using CACES.DAL.Entidades;

namespace CACES.DAL.Repositorios.Soportes
{
    public interface ISoporteRepositorio
    {
        Task<bool> CrearConsultaAsync(Soporte soporte);

        Task<List<Soporte>> GetConsultasAsync();

        Task<Soporte?> GetConsultaByIdAsync(int id);
    }
}