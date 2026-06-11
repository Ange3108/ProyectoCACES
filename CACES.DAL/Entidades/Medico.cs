using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades
{
    [Table("Medicos")]
    public partial class Medico
    {
        [Key]
        [Column("Id_Medico")]
        public int IdMedico { get; set; }

        [Required]
        [Column("Id_Especialidad")]
        public int IdEspecialidad { get; set; }

        [Required]
        [Column("Id_Usuario")]
        public int IdUsuario { get; set; }

        [Required]
        [Column("Experiencia")]
        public int Experiencia { get; set; }

        [Required]
        [Column("Certificaciones")]
        public string Certificaciones { get; set; } = null!;

        [Required]
        [Column("FechaDeRegistro")]
        public DateTime FechaDeRegistro { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; } = null!;
    }
}