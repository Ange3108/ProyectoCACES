using CACES.BLL.DTOs.Procedimientos;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Procedimientos
{
    public interface IProcedimientosServicio
    {
        Task<List<MostrarProcedimientosDTO?>> ObtenerDetalleCirugiaAsync(int idPaciente);
    }
}
