using System;
using System.Collections.Generic;
using System.Text;
using CACES.BLL.DTOs;
using CACES.BLL.DTOs.SolicitudMedico;

namespace CACES.BLL.Servicios.SolicitudMedico
{
    public interface ISolicitudMedicoServicio
    {
        Task<respuestaErrores<int>> RegistrarAsync(
            RegistrarSolicitudMedicoDTO dto
        );

        Task<respuestaErrores<List<MostrarSolicitudMedicoDTO>>>
            ObtenerTodasAsync();

        Task<respuestaErrores<MostrarSolicitudMedicoDTO>>
            ObtenerPorIdAsync(int idSolicitud);

        Task<respuestaErrores<MostrarSolicitudMedicoDTO>>
            ResponderAsync(ResponderSolicitudMedicoDTO dto);

        Task<respuestaErrores<List<EspecialidadSolicitudDTO>>>
            ObtenerEspecialidadesAsync();
    }
}
