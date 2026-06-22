using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades
{
    [Table("Medicos")]
    public partial class Medico
    {
        [Key]
        public int IdMedico { get; set; }

        public int IdEspecialidad { get; set; }

        public int IdUsuario { get; set; }

        public int Experiencia { get; set; }

        public string Certificaciones { get; set; } = null!;

        public DateTime FechaDeRegistro { get; set; }

        public virtual Usuario Usuario { get; set; } = null!;

        public Especialidad? Especialidad { get; set; }
    }
}