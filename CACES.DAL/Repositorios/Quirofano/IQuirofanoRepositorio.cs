using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.Quirofano
{
    public interface IQuirofanoRepositorio
    {
        Task<int> GetCupoMaximoAsync();
        Task<bool> ActualizarCupoAsync(int nuevoCupo);
    }
}
