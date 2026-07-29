using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CACES.BLL.DTOs.Icono
{
    public class IconoDTO
    {
            public int IdIcono { get; set; }
        [Required(ErrorMessage ="Ingrese el código del ícono")]
            public string Codigo { get; set; } = null!;
        [Required(ErrorMessage = "El nombre para el ícono es obligatorio")]
            public string Nombre { get; set; } = null!;
        
    }
}

