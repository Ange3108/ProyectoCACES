using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CACES.DAL.Entidades.Configuración
{
    [Table("Configuracion")]
    public class Configuracion
    {
        [Key]
        public int IdConfiguracion { get; set; }

        [Required, MaxLength(100)]
        public string? Clave { get; set; }

        [Required, MaxLength(500)]
        public string? Valor { get; set; }

        [Required, MaxLength(20)]
        public string? Tipo { get; set; } // "int", "bool", "string"

        [Required, MaxLength(100)]
        public string? Categoria { get; set; }

        [MaxLength(500)]
        public string? Descripcion { get; set; }
    }
}
