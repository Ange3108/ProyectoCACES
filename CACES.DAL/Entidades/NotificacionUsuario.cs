using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades.Configuración
{
    [Table("NotificacionUsuario")]
    public class NotificacionUsuario
    {
        [Key]
        public int IdNotificacionUsuario { get; set; }
        public int IdUsuario { get; set; }
        public string Evento { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public bool Leido { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaLectura { get; set; }
    }
}
