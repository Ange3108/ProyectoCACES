using CACES.BLL.DTOs;
using CACES.BLL.DTOs.SeguimientoPostOperatorio;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.SeguimientoPaciente
{
    public interface ISeguimientoPacienteServicio
    {
        Task<respuestaErrores<List<MostrarSeguimientoPacienteDTO>>> ObtenerPorCirugia(int idCirugia);
        Task<respuestaErrores<bool>> GenerarCheckpoints(int idCirugia);
        Task<respuestaErrores<List<MostrarSeguimientoPacienteDTO>>> ObtenerTodos();

        Task<respuestaErrores<int>> EnviarRecordatoriosDelDiaAsync();
    }
}
