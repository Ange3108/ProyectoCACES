using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.Especialidad
{
    public class mostrarEspecialidadDTO
    {
        public int IdEspecialidad { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string Icono { get; set; } = null!;
        public bool Estado { get; set; } = true;
        public DateTime FechaDeRegistro { get; set; } 
    }
}
