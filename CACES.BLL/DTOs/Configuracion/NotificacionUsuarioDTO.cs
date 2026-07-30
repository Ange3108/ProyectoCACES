using System;

namespace CACES.BLL.DTOs.Notificacion
{
    public class NotificacionUsuarioDTO
    {
        public int IdNotificacionUsuario { get; set; }
        public int IdUsuario { get; set; }
        public string Evento { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public bool Leido { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
