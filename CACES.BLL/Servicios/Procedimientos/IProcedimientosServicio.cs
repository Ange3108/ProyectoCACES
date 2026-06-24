using CACES.BLL.DTOs.Procedimientos;
using CACES.DAL.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Procedimientos
{
    public interface IProcedimientosServicio
    {
        Task<List<MostrarProcedimientosDTO?>> ObtenerDetalleCirugiaAsync(int idPaciente);
        Task<bool> ActualizarProcedimientoAsync(EditarProcedimientosDTO editarProcedimientosDTO, int idPaciente);
        Task<EditarProcedimientosDTO?> ObtenerPorIdParaEditarAsync(int idPaciente, int idCirugia);
        Task<bool> RegistrarProcedimientoAsync(RegistrarProcedimientosDto registrarProcedimientosDto);
        Task<List<Procedimiento>> ObtenerProcedimientosFijosAsync();
        Task<List<MostrarProcedimientosDTO>> ObtenerCirugiasPorMedicoAsync(int idMedico);
        Task<List<MostrarProcedimientosDTO>> ObtenerTodasLasCirugiasAsync();
    }
}
