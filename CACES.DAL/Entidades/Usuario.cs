using CACES.DAL.Entidades.Roles;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades
{
    [Table("Usuarios")]
    public partial class Usuario
    {
        [Key]
        [Column("Id_Usuario")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }
        public string Nombres { get; set; } = null!;
<<<<<<< HEAD
        [Column("PrimerApellido")]
        public string PrimerApellido { get; set; } = null!;
        [Column("SegundoApellido")]
        public string SegundoApellido { get; set; } = null!;
        [Column("CorreoElectronico")]
        public string CorreoElectronico { get; set; } = null!;
        [Column("DUI")]
        public string DUI { get; set; } = null!;

        [Column("Foto")]
        public string? Foto { get; set; }

        [Column("Telefono")]
        public string Telefono { get; set; } = null!;
        [Column("Direccion")]
        [StringLength(250)]
=======

        public string PrimerApellido { get; set; } = null!;
        public string SegundoApellido { get; set; } = null!;

        public string CorreoElectronico { get; set; } = null!;
   
        public string DUI { get; set; } = null!;

        public string Telefono { get; set; } = null!;
>>>>>>> 78e43479a88e6319ed1a30dc30587c1a8375219d
        public string? Direccion { get; set; }

        public DateTime Nacimiento { get; set; } 

        public DateTime FechaDeRegistro { get; set; } = DateTime.Now;

        public DateTime? FechaDeModificacion { get; set; }

        public bool Estado { get; set; } = true;

        public string PasswordHash { get; set; } = null!;

        public string SecurityStamp { get; set; } = null!;


        public bool twoFactorEnabled { get; set; } = false;

        public DateTime? lockoutEnd { get; set; }

        public bool LockoutEnabled { get; set; }

        public int accessFailedCount { get; set; }

        public bool emailConfirmed { get; set; } = false;

        [NotMapped]
        public virtual ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
    }
}