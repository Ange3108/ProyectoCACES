using System;
using System.Collections.Generic;
using System.Text;
using CACES.DAL.Entidades;

namespace CACES.DAL.Repositorios.Cotizaciones
{
    public interface ICotizacionRepositorio
    {
        Task<Cotizacion> RegistrarAsync(Cotizacion cotizacion);

        Task<Cotizacion> ActualizarAsync(Cotizacion cotizacion);

        Task<Cotizacion?> ObtenerPorIdAsync(int idCotizacion);

        Task<List<Cotizacion>> ObtenerTodasAsync();

        Task<List<Cotizacion>> ObtenerPorPacienteAsync(int idPaciente);

        Task<List<Paciente>> ObtenerPacientesAsync();

        Task<List<Medico>> ObtenerMedicosAsync();

        Task<List<Procedimiento>> ObtenerProcedimientosAsync();
        
    }
}