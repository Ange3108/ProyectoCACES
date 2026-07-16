using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.DTOs.Roles
{
    public class GestionRolDTO
    {
        public int IdUsuario { get; set; }

        public string UserId { get; set; } = null!;

        public string NombreCompleto { get; set; } = null!;

        public string CorreoElectronico { get; set; } = null!;

        public string RoleId { get; set; } = null!;

        public string NombreRol { get; set; } = null!;

        public bool Estado { get; set; }
    }
}