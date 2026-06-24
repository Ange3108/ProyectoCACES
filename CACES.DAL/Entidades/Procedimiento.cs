using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CACES.DAL.Entidades
{
    [Table("Procedimiento")]
    public class Procedimiento
    {
        [Key]
        [Column("Id_Procedimiento")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Procedimiento { get; set; }
        [Column("Id_Especialidad")]
        public int Id_Especialidad { get; set; }
        [Column("Nombre")]
        public string Nombre { get; set; }
        [Column("Descripcion")]
        public string Descripcion { get; set; }
        [Column("PrecioBase")]
        public decimal PrecioBase { get; set; }
        [Column("Estado")]
        public bool Estado { get; set; }

        [ForeignKey("Id_Especialidad")]
        public Especialidad? Especialidad { get; set; }

        public ICollection<Precios>? Precios { get; set; }
        public ICollection<Cirugias>? Cirugias { get; set; }

    }
}
