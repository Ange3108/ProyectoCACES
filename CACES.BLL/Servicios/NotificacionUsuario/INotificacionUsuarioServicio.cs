using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Notificacion;

namespace CACES.BLL.Servicios.Notificacion
{
    public interface INotificacionUsuarioServicio
    {
        Task<respuestaErrores<List<NotificacionUsuarioDTO>>> ObtenerPorUsuario(int idUsuario, bool soloNoLeidas = false);
        Task<respuestaErrores<int>> ContarNoLeidas(int idUsuario);
        Task<respuestaErrores<bool>> Crear(NotificacionUsuarioDTO dto);
        Task<respuestaErrores<bool>> MarcarLeida(int id);
        Task<respuestaErrores<bool>> MarcarTodasLeidas(int idUsuario);
    }
}
