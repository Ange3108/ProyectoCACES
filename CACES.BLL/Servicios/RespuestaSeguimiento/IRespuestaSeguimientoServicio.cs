using CACES.BLL.DTOs;
using CACES.BLL.DTOs.SeguimientoPostOperatorio;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.RespuestaSeguimiento
{
    public interface IRespuestaSeguimientoServicio
    {
        Task<respuestaErrores<List<MostrarRespuestaSeguimientoDTO>>> ObtenerTodas();
        Task<respuestaErrores<List<MostrarRespuestaSeguimientoDTO>>> ObtenerPorSeguimiento(int idSeguimiento);
        Task<respuestaErrores<List<MostrarRespuestaSeguimientoDTO>>> RegistrarRespuestas(List<RegistrarRespuestaSeguimientoDTO> respuestas);
    }
}

