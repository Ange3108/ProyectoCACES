using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.Usuario
{
    public class ActualizarUsuarioDTO
    {
        public int idUsuario { get; set; }
        public string Nombres { get; set; } = null!;
        public string PrimerApellido { get; set; } = null!;
        public string SegundoApellido { get; set; } = null!;
        public string CorreoElectronico { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public bool Estado { get; set; } = true;

    }
}
