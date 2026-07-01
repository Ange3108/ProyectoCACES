using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CACES.BLL.DTOs.Usuario
{
    public class MostrarUsuarioDTO
    {
        public int idUsuario { get; set; }
        public string Nombres { get; set; } = null!;
        public string PrimerApellido { get; set; } = null!;
        public string CorreoElectronico { get; set; } = null!;
        public string DUI { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        [StringLength(250)]
        public string? Direccion { get; set; }

        public DateTime Nacimiento { get; set; }

        public bool Estado { get; set; }
        public string SegundoApellido { get; set; } = null!;
        public string? Foto { get; set; }

        public string NombreCompleto => $"{Nombres} {PrimerApellido} {SegundoApellido}".Trim();

    }
}
