using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades
{
    [Table("Medicos")]
    public partial class Medico
    {
        [Key]
        [Column("Id_Medico")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMedico { get; set; }

        [Required]
        [Column("Nombre")]
        public string Nombre { get; set; } = null!;

        [Required]
        [Column("Especialidad")]
        public string Especialidad { get; set; } = null!;

        [Required]
        [Column("Experiencia")]
        public int Experiencia { get; set; }

        [Required]
        [Column("Correo")]
        public string Correo { get; set; } = null!;

        [Required]
        [Column("Telefono")]
        public string Telefono { get; set; } = null!;

        [Column("Descripcion")]
        public string Descripcion { get; set; } = null!;

        [Column("Estado")]
        public string Estado { get; set; } = null!;

        [Column("Foto")]
        public string? Foto { get; set; }
    }
}