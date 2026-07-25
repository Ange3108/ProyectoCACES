using CACES.BLL.DTOs;
using CACES.BLL.DTOs.SeguimientoPostOperatorio;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.AlertaStaff
{
    public interface IAlertaStaffServicio
    {
        Task<respuestaErrores<List<AlertaStaffDTO>>> ObtenerTodas();
        Task<respuestaErrores<List<AlertaStaffDTO>>> ObtenerPendientes();
        Task<respuestaErrores<int>> ContarPendientes();
        Task<respuestaErrores<bool>> AtenderAlerta(AtenderAlertaDTO dto);
    }
}
