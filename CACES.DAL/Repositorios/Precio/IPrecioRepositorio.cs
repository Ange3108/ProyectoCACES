using CACES.DAL.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.Precio
{
    public interface IPrecioRepositorio
    {
        Task<IEnumerable<Precios>> GetAllAsync();
        Task<Precios?> GetByIdAsync(int id);
        Task<IEnumerable<Precios>> GetByMedicoIdAsync(int idMedico);
        Task<Precios?> GetByMedicoYProcedimientoAsync(int idMedico, int idProcedimiento);
        Task AddAsync(Precios precio);
        void Update(Precios precio);
        Task<bool> SaveChangesAsync();
    }
}
