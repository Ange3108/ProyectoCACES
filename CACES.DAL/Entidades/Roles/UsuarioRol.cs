using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades.Roles
{
    [NotMapped]
    public class UsuarioRoles
    {
        public string UserId { get; set; } = null!;
        public string RoleId { get; set; } = null!;
    }
}