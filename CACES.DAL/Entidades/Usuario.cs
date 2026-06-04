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

        [Column("Nombres")]
        public string Nombres { get; set; } = null!;

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

        [Column("FechaDeRegistro")]
        public DateTime FechaDeRegistro { get; set; } = DateTime.Now;

        [Column("FechaDeModificacion")]
        public DateTime? FechaDeModificacion { get; set; }

        [Column("Estado")]
        public bool Estado { get; set; } = true;

        [Column("Direccion")]
        [StringLength(250)]
        public string Direccion { get; set; } = null!;

        [Column("Edad")]
        public int Edad { get; set; }

        [Column("Telefono")]
        public string Telefono { get; set; } = null!;

        [Column("Nacimiento")]
        public DateTime Nacimiento { get; set; }

        [Column("PasswordHash")]
        public string PasswordHash { get; set; } = null!;

        [Column("SecurityStamp")]
        public string SecurityStamp { get; set; } = null!;

        [Column("TwoFactorEnabled")]
        public bool twoFactorEnabled { get; set; } = false;

        [Column("LockoutEndDateUtc")]
        public DateTime? lockoutEnd { get; set; }

        [Column("LockoutEnabled")]
        public bool LockoutEnabled { get; set; }

        [Column("AccessFailedCount")]
        public int accessFailedCount { get; set; }

        [Column("EmailConfirmed")]
        public bool emailConfirmed { get; set; } = false;

        [NotMapped]
        public virtual ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
    }
}