using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades
{
    [Table("Recetas")]
    public class Receta
    {
        [Key]
        [Column("Id_Receta")]
        public int IdReceta { get; set; }

        [Column("Id_Cita")]
        public int IdCita { get; set; }

        public string Medicamentos { get; set; } = null!;

        public string? Instrucciones { get; set; }

        public DateTime FechaDeRegistro { get; set; }

        public DateTime FechaDeVencimiento { get; set; }

        public virtual Cita Cita { get; set; } = null!;
    }
}