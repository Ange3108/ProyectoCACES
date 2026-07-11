using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Icono;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Icono
{
    public interface IIconoServicio
    {
        Task<respuestaErrores<List<IconoDTO>>> GetListadoIconosAsync();
        Task<respuestaErrores<IconoDTO>> CrearIconoAsync(IconoDTO iconoDTO);
        Task<respuestaErrores<IconoDTO>> ActualizarIconoAsync(int id, IconoDTO iconoDTO);
    }
}
