using System;
using System.Collections.Generic;
using System.Text;
using CACES.BLL.DTOs.Cotizacion;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.Cotizaciones;
using PacienteEntidad = CACES.DAL.Entidades.Paciente;

namespace CACES.BLL.Servicios.Cotizaciones
{
    public class CotizacionServicio : ICotizacionServicio
    {
        private readonly ICotizacionRepositorio _repositorio;

        public CotizacionServicio(ICotizacionRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<bool> RegistrarCotizacionAsync(RegistrarCotizacionDTO dto)
        {
            var cotizacion = new Cotizacion
            {
                IdPaciente = dto.IdPaciente,
                IdMedico = dto.IdMedico,
                IdProcedimiento = dto.IdProcedimiento,

                PrecioBase = 0,
                Descuento = 0,
                Impuesto = 0,
                Total = 0,

                Observaciones = dto.Observaciones,

                Estado = 1
            };

            await _repositorio.RegistrarAsync(cotizacion);

            return true;
        }

        public async Task<bool> ActualizarCotizacionAsync(EditarCotizacionDTO dto)
        {
            var cotizacion =
                await _repositorio.ObtenerPorIdAsync(dto.IdCotizacion);

            if (cotizacion == null)
                return false;

            cotizacion.PrecioBase = dto.PrecioBase;
            cotizacion.Descuento = dto.Descuento;
            cotizacion.Impuesto = dto.Impuesto;

            cotizacion.Total =
                dto.PrecioBase -
                dto.Descuento +
                dto.Impuesto;

            cotizacion.Observaciones = dto.Observaciones;
            cotizacion.Estado = dto.Estado;

            await _repositorio.ActualizarAsync(cotizacion);

            return true;
        }

        public async Task<List<MostrarCotizacionDTO>> ObtenerTodasAsync()
        {
            var lista = await _repositorio.ObtenerTodasAsync();

            return lista.Select(c => new MostrarCotizacionDTO
            {
                IdCotizacion = c.IdCotizacion,

                NombrePaciente =
                    $"{c.Paciente.Usuario.Nombres} {c.Paciente.Usuario.PrimerApellido}",

                NombreMedico =
                    $"{c.Medico.Usuario.Nombres} {c.Medico.Usuario.PrimerApellido}",

                NombreProcedimiento = c.Procedimiento.Nombre,

                FechaSolicitud = c.FechaSolicitud,

                PrecioBase = c.PrecioBase,

                Descuento = c.Descuento,

                Impuesto = c.Impuesto,

                Total = c.Total,

                Observaciones = c.Observaciones,

                Estado = c.Estado,

                EstadoTexto = ObtenerEstado(c.Estado)

            }).ToList();
        }

        public async Task<List<MostrarCotizacionDTO>> ObtenerPorPacienteAsync(int idPaciente)
        {
            var lista =
                await _repositorio.ObtenerPorPacienteAsync(idPaciente);

            return lista.Select(c => new MostrarCotizacionDTO
            {
                IdCotizacion = c.IdCotizacion,

                NombrePaciente = "",

                NombreMedico =
                    $"{c.Medico.Usuario.Nombres} {c.Medico.Usuario.PrimerApellido}",

                NombreProcedimiento = c.Procedimiento.Nombre,

                FechaSolicitud = c.FechaSolicitud,

                PrecioBase = c.PrecioBase,

                Descuento = c.Descuento,

                Impuesto = c.Impuesto,

                Total = c.Total,

                Observaciones = c.Observaciones,

                Estado = c.Estado,

                EstadoTexto = ObtenerEstado(c.Estado)

            }).ToList();
        }

        public async Task<EditarCotizacionDTO?> ObtenerEditarAsync(int idCotizacion)
        {
            var c =
                await _repositorio.ObtenerPorIdAsync(idCotizacion);

            if (c == null)
                return null;

            return new EditarCotizacionDTO
            {
                IdCotizacion = c.IdCotizacion,

                PrecioBase = c.PrecioBase,

                Descuento = c.Descuento,

                Impuesto = c.Impuesto,

                Observaciones = c.Observaciones,

                Estado = c.Estado
            };
        }

        private static string ObtenerEstado(byte estado)
        {
            return estado switch
            {
                1 => "Pendiente",
                2 => "Enviada",
                3 => "Aceptada",
                4 => "Rechazada",
                _ => "Desconocido"
            };
        }

        public async Task<List<Procedimiento>> ObtenerProcedimientosAsync()
        {
            return await _repositorio.ObtenerProcedimientosAsync();
        }

        public async Task<List<Medico>> ObtenerMedicosAsync()
        {
            return await _repositorio.ObtenerMedicosAsync();
        }

        public async Task<List<PacienteEntidad>> ObtenerPacientesAsync()
        {
            return await _repositorio.ObtenerPacientesAsync();
        }
        public async Task<MostrarCotizacionDTO?> ObtenerDetalleAsync(
    int idCotizacion)
        {
            var cotizacion =
                await _repositorio.ObtenerPorIdAsync(idCotizacion);

            if (cotizacion == null)
                return null;

            return new MostrarCotizacionDTO
            {
                IdCotizacion = cotizacion.IdCotizacion,

                NombrePaciente = string.Join(
                    " ",
                    new[]
                    {
                cotizacion.Paciente?.Usuario?.Nombres,
                cotizacion.Paciente?.Usuario?.PrimerApellido,
                cotizacion.Paciente?.Usuario?.SegundoApellido
                    }
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                ),

                NombreMedico = string.Join(
                    " ",
                    new[]
                    {
                cotizacion.Medico?.Usuario?.Nombres,
                cotizacion.Medico?.Usuario?.PrimerApellido,
                cotizacion.Medico?.Usuario?.SegundoApellido
                    }
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                ),

                NombreProcedimiento =
                    cotizacion.Procedimiento?.Nombre
                    ?? "No disponible",

                FechaSolicitud = cotizacion.FechaSolicitud,

                PrecioBase = cotizacion.PrecioBase,

                Descuento = cotizacion.Descuento,

                Impuesto = cotizacion.Impuesto,

                Total = cotizacion.Total,

                Observaciones = cotizacion.Observaciones,

                Estado = cotizacion.Estado,

                EstadoTexto = ObtenerEstado(cotizacion.Estado)
            };
        }
    }
}