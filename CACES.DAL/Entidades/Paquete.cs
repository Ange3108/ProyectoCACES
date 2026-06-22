using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades
{
    [Table("Paquetes")]
    public partial class Paquete
    {
        [Key]
        [Column("Id_Paquete")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPaquete { get; set; }

        [Column("Nombre")]
        public string Nombre { get; set; } = null!;

        [Column("Descripcion")]
        public string Descripcion { get; set; } = null!;

        [Column("Duracion")]
        public string Duracion { get; set; } = null!;

        [Column("Precio")]
        public decimal Precio { get; set; }

        [Column("FechaDeRegistro")]
        public DateTime FechaDeRegistro { get; set; } = DateTime.Now;

        [Column("Estado")]
        public bool Estado { get; set; }
    }
}
