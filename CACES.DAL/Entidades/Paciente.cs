using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades
{
    [Table("Pacientes")]
    public partial class Paciente
    {
        [Key]
        [Column("Id_Paciente")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPaciente { get; set; }

        [Required(ErrorMessage = "El usuario es requerido")]
        [Column("Id_Usuario")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El historial médico es requerido")]
        [Column("Id_Historial")]
        public int IdHistorial { get; set; }

        public virtual Usuario Usuario { get; set; } = null!;

        public virtual HistorialMedico HistorialMedico { get; set; } = null!;

        public virtual ICollection<Cita> Citas { get; set; } = new List<Cita>();

        public virtual ICollection<Cirugias> Cirugias { get; set; } = new List<Cirugias>();
    }
}