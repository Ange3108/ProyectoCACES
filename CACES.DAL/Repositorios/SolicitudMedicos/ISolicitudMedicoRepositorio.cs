using System;
using System.Collections.Generic;
using System.Text;
using CACES.DAL.Entidades;

namespace CACES.DAL.Repositorios.SolicitudMedicos
{
    public interface ISolicitudMedicoRepositorio
    {
        Task<SolicitudMedico> RegistrarAsync(
            SolicitudMedico solicitud
        );

        Task<List<SolicitudMedico>> ObtenerTodasAsync();

        Task<SolicitudMedico?> ObtenerPorIdAsync(
            int idSolicitud
        );

        Task<SolicitudMedico?> ObtenerPendientePorCorreoAsync(
            string correoElectronico
        );

        Task<SolicitudMedico> ActualizarAsync(
            SolicitudMedico solicitud
        );

        Task<List<Especialidad>> ObtenerEspecialidadesActivasAsync();
    }
}