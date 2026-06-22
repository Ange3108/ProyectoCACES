using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CACES.DAL.Entidades
{
    [Table("Precios")]
    public class Precios
    {
        [Key]
        [Column("Id_Precio")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Precio { get; set; }

        [Column("Id_Medico")]
        public int Id_Medico { get; set; }

        [Column("Id_Procedimiento")]
        public int Id_Procedimiento { get; set; }

        [Column("Costo")]
        public decimal Costo { get; set; }

        [Column("Detalles")]
        public string Detalles { get; set; }

        // Propiedades de navegación (siguiendo tu estilo)
        public Medico? Medico { get; set; }
        public Procedimiento? Procedimiento { get; set; }
    }
}
