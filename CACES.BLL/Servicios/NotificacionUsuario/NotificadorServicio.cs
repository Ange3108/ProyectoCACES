using CACES.BLL.DTOs.Notificacion;
using CACES.BLL.Servicios.Usuario;

namespace CACES.BLL.Servicios.Notificacion
{
    public class NotificadorServicio : INotificadorServicio
    {
        private readonly INotificacionServicio _notificacionServicio;          
        private readonly INotificacionUsuarioServicio _notificacionUsuarioServicio; // campanita
        private readonly IEmailServicio _emailServicio;
        private readonly IUsuarioService _usuarioCorreoProveedor;

        public NotificadorServicio(
            INotificacionServicio notificacionServicio,
            INotificacionUsuarioServicio notificacionUsuarioServicio,
            IEmailServicio emailServicio,
            IUsuarioService usuarioCorreoProveedor)
        {
            _notificacionServicio = notificacionServicio;
            _notificacionUsuarioServicio = notificacionUsuarioServicio;
            _emailServicio = emailServicio;
            _usuarioCorreoProveedor = usuarioCorreoProveedor;
        }

        public async Task NotificarAsync(string evento, int idUsuario, string titulo, string mensaje, string? correoDestinoManual = null)
        {
            var configuracion = await _notificacionServicio.ObtenerPorEvento(evento);


            if (!configuracion.EsCorrecto || configuracion.Dato == null || !configuracion.Dato.Estado)
                return;

            if (configuracion.Dato.CanalPlataforma)
            {
                await _notificacionUsuarioServicio.Crear(new NotificacionUsuarioDTO
                {
                    IdUsuario = idUsuario,
                    Evento = evento,
                    Titulo = titulo,
                    Mensaje = mensaje
                });
            }

            if (configuracion.Dato.CanalEmail)
            {

                var usuario = await _usuarioCorreoProveedor.GetUsuarioPorIdAsync(idUsuario);
                if (usuario == null) return;
                if (!string.IsNullOrWhiteSpace(usuario.Dato.CorreoElectronico))
                {
                    await _emailServicio.EnviarCorreoAsync(usuario.Dato.CorreoElectronico, titulo, mensaje);
                }
            }
        }
    }

    
   
}
