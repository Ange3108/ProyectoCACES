using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades
{
    [Table("Cirugias")]
    public class Cirugias
    {
        [Key]
        [Column("Id_Cirugia")]
        public int Id_Cirugia { get; set; }

        [Column("Id_Paciente")]
        public int Id_Paciente { get; set; }

        [Column("Id_Medico")]
        public int Id_Medico { get; set; }

        [Column("Id_Procedimiento")]
        public int Id_Procedimiento { get; set; }

        [Column("Id_Horario")]
        public int Id_Horario { get; set; }

        [Column("Id_Cita")]
        public int Id_Cita { get; set; }

        [Column("Estado")]
        public bool Estado { get; set; }

        public virtual Cita? Cita { get; set; }

        public virtual Paciente Paciente { get; set; } = null!;

        public virtual Medico Medico { get; set; } = null!;

        public virtual Procedimiento Procedimiento { get; set; } = null!;

        public virtual HorariosDisponibles Horario { get; set; } = null!;
    }
}