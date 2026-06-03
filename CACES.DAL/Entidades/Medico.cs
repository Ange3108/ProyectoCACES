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

        [Column("Id_Especialidad")]
        public int IdEspecialidad { get; set; }

        [Column("Id_Usuario")]
        public int IdUsuario { get; set; }

        [Column("Experiencia")]
        public int Experiencia { get; set; }

        [Column("Telefono")]
        public string Telefono { get; set; } = null!;

        [Column("Certificaciones")]
        public string Certificaciones { get; set; } = null!;

        [Column("FechaDeRegistro")]
        public DateTime FechaDeRegistro { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; } = null!;
    }
}