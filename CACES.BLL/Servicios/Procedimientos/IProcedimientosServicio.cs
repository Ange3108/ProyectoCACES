using CACES.BLL.DTOs;
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
        Task<List<MostrarProcedimientosDTO>> ObtenerCirugiasPorMedicoAsync(int idMedico);
        Task<List<MostrarProcedimientosDTO>> ObtenerTodasLasCirugiasAsync();
        Task<respuestaErrores<MostrarProcedimientosDTO>> ObtenerDatosReporteAsync(int idCirugia);
        Task<List<InsertarProcedimientosDto>> ListarProcedimientosAsync();
        Task<bool> GuardarProcedimientoAsync(InsertarProcedimientosDto dto);
        Task<InsertarProcedimientosDto> ObtenerPorIdAsync(int id);
        Task<bool> EditarProcedimientoAdminAsync(InsertarProcedimientosDto dto);
        Task<bool> CambiarEstadoProcedimientoAsync(int id);
    }
}
