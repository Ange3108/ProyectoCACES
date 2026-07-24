using CACES.BLL.DTOs;
using CACES.BLL.DTOs.SeguimientoPostOperatorio;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.ConfiguracionCheckPoints
{
    public interface IConfiguracionCheckPointsServicio
    {
        Task<respuestaErrores<List<ConfiguracionCheckPointDTO>>> ObtenerConfiguracionesCheckPoints();
        Task<respuestaErrores<ConfiguracionCheckPointDTO>> ObtenerConfiguracionCheckPointPorId(int id);
        Task<respuestaErrores<List<ConfiguracionCheckPointDTO>>> ObtenerConfiguracionesCheckPointsActivas();
        Task<respuestaErrores<ConfiguracionCheckPointDTO>> CrearConfiguracionCheckPoint(RegistrarConfiguracionCheckpointDTO configuracionCheckPoint);
        Task<respuestaErrores<ConfiguracionCheckPointDTO>> ActualizarConfiguracionCheckPoint(ConfiguracionCheckPointDTO configuracionCheckPoint);
        Task<respuestaErrores<ConfiguracionCheckPointDTO>> EliminarConfiguracionCheckPoint(int id);
        Task <respuestaErrores<ConfiguracionCheckPointDTO>> DesactivarConfiguracionCheckPoint(int id);
    }
}
