using CACES.DAL.Entidades.Roles;
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

        public string PrimerApellido { get; set; } = null!;

        public string SegundoApellido { get; set; } = null!;

        public string CorreoElectronico { get; set; } = null!;

        public string DUI { get; set; } = null!;

        public string Foto { get; set; } = null!;

        public DateTime FechaDeRegistro { get; set; } = DateTime.Now;

        public DateTime? FechaDeModificacion { get; set; }

        public bool Estado { get; set; } = true;

        public string Direccion { get; set; } = null!;

        public int Edad { get; set; }

        public string Telefono { get; set; } = null!;

        public DateTime Nacimiento { get; set; }

        public string PasswordHash { get; set; } = null!;

        public string SecurityStamp { get; set; } = null!;

        public bool TwoFactorEnabled { get; set; } = false;

        public DateTime? LockoutEnd { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        public bool EmailConfirmed { get; set; } = false;

        public virtual ICollection<UsuarioRoles> UsuarioRoles { get; set; } = new List<UsuarioRoles>();
    }
}