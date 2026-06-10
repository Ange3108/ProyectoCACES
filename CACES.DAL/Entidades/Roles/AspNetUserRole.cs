using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades.Roles
{
    [Table("AspNetUserRoles")]
    public class AspNetUserRole
    {
        [Column("UserId")]
        public string UserId { get; set; } = null!;

        [Column("RoleId")]
        public string RoleId { get; set; } = null!;
    }
}