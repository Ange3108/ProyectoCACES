using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades
{
    [Table("Precios")]
    public class Precios
    {
        [Key]
        [Column("Id_Precio")]
        public int Id_Precio { get; set; }

        [Column("Id_Medico")]
        public int Id_Medico { get; set; }

        [Column("Id_Procedimiento")]
        public int Id_Procedimiento { get; set; }

        [Column("Costo")]
        public decimal Costo { get; set; }

        [Column("Detalles")]
        public string Detalles { get; set; } = null!;

        public virtual Medico Medico { get; set; } = null!;

        public virtual Procedimiento Procedimiento { get; set; } = null!;
    }
}
