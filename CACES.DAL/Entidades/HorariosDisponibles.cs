using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades
{
    [Table("HorariosDisponibles")]
    public class HorariosDisponibles
    {
        [Key]
        [Column("Id_Horario")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Horario { get; set; }

        [Column("Id_Medico")]
        public int Id_Medico { get; set; }

        [Column("DiaSemana")]
        public int DiaSemana { get; set; }

        [Column("HoraInicio")]
        public TimeSpan HoraInicio { get; set; }

        [Column("HoraFin")]
        public TimeSpan HoraFin { get; set; }

        [Column("Activo")]
        public bool Activo { get; set; }

        public virtual Medico Medico { get; set; } = null!;

        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();

        public virtual ICollection<Cirugias> Cirugias { get; set; } = new List<Cirugias>();
    }
}