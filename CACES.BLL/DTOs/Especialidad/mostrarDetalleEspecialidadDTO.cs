using System.Collections.Generic;

using CACES.BLL.DTOs.Medico;
using CACES.BLL.DTOs.Procedimientos;

namespace CACES.BLL.DTOs.Especialidad
{
    public class mostrarDetalleEspecialidadDTO
    {
        public int IdEspecialidad { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public int IdIcono { get; set; } 
        public string NombreIcono { get; set; } = null!;

        public List<MostrarProcedimientosDTO> Procedimientos { get; set; } = new List<MostrarProcedimientosDTO>();
        public List<mostrarMedicoEspecialidadDTO> Medicos { get; set; } = new List<mostrarMedicoEspecialidadDTO>();
    }
}