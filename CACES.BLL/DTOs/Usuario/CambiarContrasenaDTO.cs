using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.Usuario
{
    public class CambiarContrasenaDTO
    {
        public string PasswordActual { get; set; }
        public string PasswordNueva { get; set; } = string.Empty;
    }
}
