using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CACES.DAL.Entidades
{
    [Table("Usuarios")]
    public partial class Usuario
    {
        [Key]
        [Column("Id_Usuario")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100)]
        [Column("Nombres")]
        public string Nombres { get; set; } = null!;

        [Required(ErrorMessage = "El primer apellido es requerido")]
        [StringLength(100)]
        [Column("PrimerApellido")]
        public string PrimerApellido { get; set; } = null!;

        [Required(ErrorMessage = "El segundo apellido es requerido")]
        [StringLength(100)]
        [Column("SegundoApellido")]
        public string SegundoApellido { get; set; } = null!;

        [Required(ErrorMessage = "El correo es requerido")]
        [StringLength(200)]
        [EmailAddress(ErrorMessage = "Correo electrónico inválido")]
        [Column("CorreoElectronico")]
        public string CorreoElectronico { get; set; } = null!;

        [Required(ErrorMessage = "El DUI es requerido")]
        [StringLength(10)]
        [Column("DUI")]
        public string DUI { get; set; } = null!;

        [StringLength(200)]
        [Column("Foto")]
        public string Foto { get; set; } = null!;

        [Required(ErrorMessage = "El teléfono es requerido")]
        [StringLength(30)]
        [Column("Telefono")]
        public string Telefono { get; set; } = null!;

        [Column("FechaDeRegistro")]
        public DateTime FechaDeRegistro { get; set; } = DateTime.Now;

        [Column("FechaDeModificacion")]
        public DateTime? FechaDeModificacion { get; set; }

        [Column("Estado")]
        public bool Estado { get; set; } = true;


    }
}
