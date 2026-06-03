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

<<<<<<< HEAD
        [Column("Id_Especialidad")]
        public int IdEspecialidad { get; set; }

=======
        [Required]
        [Column("Id_Especialidad")]
        public int IdEspecialidad { get; set; }

        [Required]
>>>>>>> 78e43479a88e6319ed1a30dc30587c1a8375219d
        [Column("Id_Usuario")]
        public int IdUsuario { get; set; }

        [Column("Experiencia")]
        public int Experiencia { get; set; }

<<<<<<< HEAD
        [Column("Telefono")]
        public string Telefono { get; set; } = null!;

        [Column("Certificaciones")]
        public string Certificaciones { get; set; } = null!;

        [Column("FechaDeRegistro")]
        public DateTime FechaDeRegistro { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; } = null!;
=======
        [Required]
        [Column("Certificaciones")]
        public string Certificaciones { get; set; } = null!;

        [Required]
        [Column("FechaDeRegistro")]
        public DateTime FechaDeRegistro { get; set; }
        [Required]
        [Column("Foto")]
        public string Foto { get; set; } = null!;
>>>>>>> 78e43479a88e6319ed1a30dc30587c1a8375219d
    }
}