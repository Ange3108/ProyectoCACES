using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Notificacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Notificacion
{
    public interface INotificacionServicio
    {
        Task<respuestaErrores<List<NotificacionDTO>>> ObtenerTodos();
        Task<respuestaErrores<NotificacionDTO>> ObtenerPorId(int id);
        Task<respuestaErrores<NotificacionDTO>> ObtenerPorEvento(string evento);
        Task<respuestaErrores<bool>> Crear(NotificacionDTO dto);
        Task<respuestaErrores<bool>> Actualizar(NotificacionDTO dto);
        Task<respuestaErrores<bool>> CambiarEstado(int id);
    }
}
