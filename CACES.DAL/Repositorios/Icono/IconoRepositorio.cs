using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.Icono
{
    public class IconoRepositorio : IIconoRepositorio
    {
        public Task<bool> ActualizarAsync(Icono icono)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CrearAsync(Icono icono)
        {
            throw new NotImplementedException();
        }

        public Task<Icono?> GetPorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Icono>> GetTodosLosIconosAsync()
        {
            throw new NotImplementedException();
        }
    }
}
