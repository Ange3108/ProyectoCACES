using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades.Roles
{
    [Table("AspNetRoles")]
    public class AspNetRole
    {
        [Key]
        [Column("Id")]
        public string Id { get; set; } = null!;

        [Column("Name")]
        public string Name { get; set; } = null!;

        public virtual ICollection<UsuarioRoles> UsuarioRoles { get; set; } = new List<UsuarioRoles>();
    }
}