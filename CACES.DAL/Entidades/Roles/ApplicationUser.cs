using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CACES.DAL.Entidades.Roles
{
    [Table("AspNetUsers")]
    public class ApplicationUser
    {
        [Key]
        [Column("Id")]
        public string Id { get; set; } = null!;

        [Column("Email")]
        public string? Email { get; set; }

        [Column("UserName")]
        public string? UserName { get; set; }
    }
}