using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
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

        [Required(ErrorMessage = "El usuario es requerido")]
        [Column("Id_Usuario")]
        [ForeignKey("Usuario")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "La especialidad es requerida")]
        [Column("Especialidad")]
        [StringLength(100)]
        public string Especialidad { get; set; } = null!;

        [Required(ErrorMessage = "Los años de experiencia son requeridos")]
        [Column("Anios_Experiencia")]
        public int AniosExperiencia { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        [Column("Estado")]
        [StringLength(50)]
        public string Estado { get; set; } = null!;

        // RELACION CON USUARIO
        public virtual Usuario Usuario { get; set; } = null!;
    }
}