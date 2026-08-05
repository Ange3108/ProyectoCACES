using System;
using System.Collections.Generic;
using System.Text;
using CACES.DAL.Entidades;

namespace CACES.DAL.Repositorios.Precio
{
    public interface IPrecioRepositorio
    {
        Task<List<Precios>> ObtenerTodosAsync();

        Task<Precios?> ObtenerPorIdAsync(int idPrecio);

        Task<Precios> ActualizarAsync(Precios precio);
    }
}