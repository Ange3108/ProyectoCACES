using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.Notificacion
{
    public class NotificacionDTO
    {
        public int Id_Notificacion { get; set; }
        public string Evento { get; set; }
        public bool CanalPlataforma { get; set; }
        public bool CanalEmail { get; set; }
        public bool Estado { get; set; }
    }
}
