using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CACES.DAL.Entidades.Roles
{
    public class AspNetRole
    {
        [Key]
        [Column("Id")]
        public string Id { get; set; } = null!;

        [Column("Name")]
        public string Name { get; set; } = null!;

        // Navegación a usuarios con este rol
        public virtual ICollection<UsuarioRol> UsuarioRoles { get; set; } = new List<UsuarioRol>();
    }
}
