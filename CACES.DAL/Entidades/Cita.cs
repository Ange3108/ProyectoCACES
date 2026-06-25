using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades
{
    [Table("Citas")]
    public class Cita
    {
        [Key]
        [Column("Id_Cita")]
        public int IdCita { get; set; }

        [Column("Id_Paciente")]
        public int IdPaciente { get; set; }

        [Column("Id_Medico")]
        public int IdMedico { get; set; }

        [Column("Id_Especialidad")]
        public int IdEspecialidad { get; set; }

        [Column("Id_Horario")]
        public int IdHorario { get; set; }

        [Column("Fecha")]
        public DateTime Fecha { get; set; }

        [Column("Hora")]
        public TimeSpan Hora { get; set; }

        [Column("Motivo")]
        public string Motivo { get; set; } = null!;

        [Column("FechaCita")]
        public DateTime FechaCita { get; set; }

        [Column("FechaDeRegistro")]
        public DateTime FechaDeRegistro { get; set; }

        [Column("FechaDeModificacion")]
        public DateTime? FechaDeModificacion { get; set; }

        [Column("Estado")]
        public byte Estado { get; set; }

        [NotMapped]
        public virtual Receta? Receta { get; set; }
    }
}