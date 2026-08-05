using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.Cirugia
{
    public interface ICirugiaRepositorio
    {
        Task<bool> CambiarEstadoAsync(int id);
        Task<List<Cirugias>> conseguirCirugiaPorPAciente(int paciente);
    }
}
