using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CACES.DAL.Entidades.Configuración
{
    [Table("Notificaciones")]
    public  class Notificacion
    {
        [Key]
        public int Id_Notificacion { get; set; }

        [Required, MaxLength(100)]
        public string? Evento { get; set; }

        public bool CanalPlataforma { get; set; } = true;

        public bool CanalEmail { get; set; } = true;

        public bool Estado { get; set; } = true;
    }
}
