using System;
using System.Collections.Generic;
using System.Text;
using CACES.BLL.DTOs.Precio;
using CACES.BLL.DTOs;

namespace CACES.BLL.Servicios.Precio
{
    public interface IPrecioServicio
    {
        Task<respuestaErrores<List<MostrarPrecioDTO>>>
            ObtenerTodosAsync();

        Task<respuestaErrores<EditarPrecioDTO>>
            ObtenerEditarAsync(int idPrecio);

        Task<respuestaErrores<MostrarPrecioDTO>>
            ActualizarAsync(EditarPrecioDTO dto);
    }
}