using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CACES.DAL.Entidades.Roles
{
    public class UsuarioRol
    {
        [Column("Id_Usuario")]
        public int IdUsuario { get; set; }

        [Column("RoleId")]
        public string RoleId { get; set; } = null!;

        // Navegación
        public virtual Usuario Usuario { get; set; } = null!;
        public virtual AspNetRole Rol { get; set; } = null!;
    }
}
