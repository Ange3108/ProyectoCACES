using System.Collections.Generic;

using CACES.BLL.DTOs.Medico;

namespace CACES.BLL.DTOs.Especialidad
{
    public class mostrarDetalleEspecialidadDTO
    {
        public int IdEspecialidad { get; set; }
        public string Nombre { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public string Icono { get; set; } = null!;

       // public List<mostrarProcedimientoDTO> Procedimientos { get; set; } = new List<mostrarProcedimientoDTO>();
        public List<mostrarMedicoEspecialidadDTO> Medicos { get; set; } = new List<mostrarMedicoEspecialidadDTO>();
    }
}