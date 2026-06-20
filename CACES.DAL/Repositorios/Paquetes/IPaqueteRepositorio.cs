using CACES.DAL.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.Paquetes
{
    public interface IPaqueteRepositorio
    {
        Task<bool> CreatePaqueteAsync(DAL.Entidades.Paquete paquete);
        Task<List<Paquete>> GetPaquetesAsync();
        Task<List<Paquete>> GetPaquetesSoloActivosAsync();
        Task<bool> UpdatePaqueteAsync(Paquete paquete);
        Task<Paquete> GetPaqueteByIdAsync(int id);
    }
}
