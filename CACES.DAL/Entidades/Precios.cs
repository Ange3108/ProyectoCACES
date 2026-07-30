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

        [Required]
        [Column("Id_Medico")]
        public int Id_Medico { get; set; }

        [Required]
        [Column("Id_Procedimiento")]
        public int Id_Procedimiento { get; set; }

        [Required]
        [Column("Costo", TypeName = "decimal(10,2)")]
        public decimal Costo { get; set; }

        [Required]
        [StringLength(100)]
        [Column("Detalles")]
        public string Detalles { get; set; } = string.Empty;

        [ForeignKey(nameof(Id_Medico))]
        public virtual Medico Medico { get; set; } = null!;

        [ForeignKey(nameof(Id_Procedimiento))]
        public virtual Procedimiento Procedimiento { get; set; } = null!;
    }
}