using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.DTOs.Especialidad
{
    public class mostrarEspecialidadDTO
    {
        public string IdEspecialidad { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public int IdIcono { get; set; } 
        public string NombreIcono { get; set; } = null!;
        
    }
}
