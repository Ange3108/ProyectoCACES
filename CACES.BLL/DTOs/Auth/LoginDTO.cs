using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CACES.BLL.DTOs.Auth
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Debe ingresar el correo electrónico")]
        [EmailAddress(ErrorMessage = "Correo electrónico inválido")]
        public string CorreoElectronico { get; set; } = null!;

        [Required(ErrorMessage = "Debe ingresar la contraseña")]
        public string Password { get; set; } = null!;


    }
}