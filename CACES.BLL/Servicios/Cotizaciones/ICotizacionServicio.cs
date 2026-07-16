using CACES.BLL.DTOs.Cotizacion;
using CACES.DAL.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using CACES.BLL.DTOs.Cotizacion;
using CACES.DAL.Entidades;
using PacienteEntidad = CACES.DAL.Entidades.Paciente;

namespace CACES.BLL.Servicios.Cotizaciones
{
    public interface ICotizacionServicio
    {
        Task<bool> RegistrarCotizacionAsync(
            RegistrarCotizacionDTO dto);

        Task<bool> ActualizarCotizacionAsync(
            EditarCotizacionDTO dto);

        Task<List<MostrarCotizacionDTO>> ObtenerTodasAsync();

        Task<List<MostrarCotizacionDTO>> ObtenerPorPacienteAsync(
            int idPaciente);

        Task<EditarCotizacionDTO?> ObtenerEditarAsync(
            int idCotizacion);

        Task<MostrarCotizacionDTO?> ObtenerDetalleAsync(
            int idCotizacion);

        Task<List<Procedimiento>> ObtenerProcedimientosAsync();

        Task<List<Medico>> ObtenerMedicosAsync();

        Task<List<PacienteEntidad>> ObtenerPacientesAsync();
    }
}