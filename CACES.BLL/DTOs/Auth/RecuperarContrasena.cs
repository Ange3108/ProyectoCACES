using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CACES.BLL.DTOs.Auth
{
    public class OlvidoContrasenaDTO
    {
        public string CorreoElectronico { get; set; } = null!;
    }

    public class RestablecerContrasenaDTO
    {
        public string CorreoElectronico { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string NuevaContrasena { get; set; } = null!;

    }
}
