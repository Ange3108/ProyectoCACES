using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades
{
    [Table("HorariosDisponibles")]
    public class HorariosDisponibles
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id_Horario { get; set; }


        public int Id_Medico { get; set; }


        public int DiaSemana { get; set; }

 
        public TimeSpan HoraInicio { get; set; }


        public bool Estado { get; set; }

        public virtual Medico Medico { get; set; } = null!;

        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();

        public virtual ICollection<Cirugias> Cirugias { get; set; } = new List<Cirugias>();
    }
}