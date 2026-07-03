using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades
{
    [Table("Soportes")]
    public class Soporte
    {
        [Key]
        [Column("Id_Soporte")]
        public int IdSoporte { get; set; }

        [Required]
        [Column("Id_Usuario")]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(150)]
        public string Asunto { get; set; }

        [Required]
        [StringLength(1000)]
        public string Mensaje { get; set; }

        public DateTime FechaConsulta { get; set; }

        public bool Estado { get; set; }

        [ForeignKey(nameof(IdUsuario))]
        public virtual Usuario Usuario { get; set; }
    }
}