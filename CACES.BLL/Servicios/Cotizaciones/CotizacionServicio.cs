using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<int> RegistrarCotizacionAsync(
            RegistrarCotizacionDTO dto)
        {
            if (dto.IdPaciente <= 0)
                throw new ArgumentException(
                    "El paciente seleccionado no es válido."
                );

            if (dto.IdMedico <= 0)
                throw new ArgumentException(
                    "El médico seleccionado no es válido."
                );

            if (dto.IdProcedimiento <= 0)
                throw new ArgumentException(
                    "El procedimiento seleccionado no es válido."
                );

            if (dto.DiasEstadia <= 0)
                throw new ArgumentException(
                    "Los días de estadía deben ser mayores a cero."
                );

            var procedimiento =
                await _repositorio.ObtenerProcedimientoPorIdAsync(
                    dto.IdProcedimiento
                );

            if (procedimiento == null)
            {
                throw new InvalidOperationException(
                    "El procedimiento seleccionado no existe o está inactivo."
                );
            }

            var precioMedico =
                await _repositorio.ObtenerPrecioMedicoAsync(
                    dto.IdMedico,
                    dto.IdProcedimiento
                );

            if (precioMedico == null)
            {
                throw new InvalidOperationException(
                    "El médico seleccionado no tiene un precio configurado para este procedimiento."
                );
            }

            var configuracion =
                await _repositorio.ObtenerConfiguracionActivaAsync();

            if (configuracion == null)
            {
                throw new InvalidOperationException(
                    "No existe una configuración activa para calcular cotizaciones."
                );
            }

            decimal precioBase =
                Math.Round(
                    procedimiento.PrecioBase,
                    2,
                    MidpointRounding.AwayFromZero
                );

            decimal honorariosMedico =
                Math.Round(
                    precioMedico.Costo,
                    2,
                    MidpointRounding.AwayFromZero
                );

            decimal costoEquipo =
                Math.Round(
                    precioBase *
                    configuracion.PorcentajeEquipo / 100m,
                    2,
                    MidpointRounding.AwayFromZero
                );

            decimal costoEstadia =
                Math.Round(
                    configuracion.CostoEstadiaDiaria *
                    dto.DiasEstadia,
                    2,
                    MidpointRounding.AwayFromZero
                );

            decimal descuento = 0m;

            decimal subtotal =
                precioBase +
                honorariosMedico +
                costoEquipo +
                costoEstadia -
                descuento;

            decimal impuesto =
                Math.Round(
                    subtotal *
                    configuracion.PorcentajeImpuesto / 100m,
                    2,
                    MidpointRounding.AwayFromZero
                );

            decimal total =
                Math.Round(
                    subtotal + impuesto,
                    2,
                    MidpointRounding.AwayFromZero
                );

            var cotizacion = new Cotizacion
            {
                IdPaciente = dto.IdPaciente,
                IdMedico = dto.IdMedico,
                IdProcedimiento = dto.IdProcedimiento,

                PrecioBase = precioBase,
                HonorariosMedico = honorariosMedico,
                CostoEquipo = costoEquipo,
                CostoEstadia = costoEstadia,
                DiasEstadia = dto.DiasEstadia,

                Descuento = descuento,
                Impuesto = impuesto,
                Total = total,

                Observaciones = dto.Observaciones?.Trim(),

                Estado = 1,
                FechaSolicitud = DateTime.Now,
                FechaDeRegistro = DateTime.Now
            };

            var registrada =
                await _repositorio.RegistrarAsync(cotizacion);

            return registrada.IdCotizacion;
        }

        public async Task<bool> ActualizarCotizacionAsync(
            EditarCotizacionDTO dto)
        {
            var cotizacion =
                await _repositorio.ObtenerPorIdAsync(
                    dto.IdCotizacion
                );

            if (cotizacion == null)
                return false;

            cotizacion.PrecioBase = dto.PrecioBase;
            cotizacion.Descuento = dto.Descuento;

            decimal subtotal =
                cotizacion.PrecioBase +
                cotizacion.HonorariosMedico +
                cotizacion.CostoEquipo +
                cotizacion.CostoEstadia -
                cotizacion.Descuento;

            cotizacion.Impuesto = dto.Impuesto;

            cotizacion.Total =
                Math.Round(
                    subtotal + cotizacion.Impuesto,
                    2,
                    MidpointRounding.AwayFromZero
                );

            cotizacion.Observaciones =
                dto.Observaciones?.Trim();

            cotizacion.Estado = dto.Estado;

            await _repositorio.ActualizarAsync(cotizacion);

            return true;
        }

        public async Task<List<MostrarCotizacionDTO>>
            ObtenerTodasAsync()
        {
            var lista =
                await _repositorio.ObtenerTodasAsync();

            return lista
                .Select(MapearMostrarCotizacion)
                .ToList();
        }

        public async Task<List<MostrarCotizacionDTO>>
            ObtenerPorPacienteAsync(int idPaciente)
        {
            var lista =
                await _repositorio.ObtenerPorPacienteAsync(
                    idPaciente
                );

            return lista
                .Select(MapearMostrarCotizacion)
                .ToList();
        }

        public async Task<EditarCotizacionDTO?>
            ObtenerEditarAsync(int idCotizacion)
        {
            var cotizacion =
                await _repositorio.ObtenerPorIdAsync(
                    idCotizacion
                );

            if (cotizacion == null)
                return null;

            return new EditarCotizacionDTO
            {
                IdCotizacion =
                    cotizacion.IdCotizacion,

                PrecioBase =
                    cotizacion.PrecioBase,

                Descuento =
                    cotizacion.Descuento,

                Impuesto =
                    cotizacion.Impuesto,

                Observaciones =
                    cotizacion.Observaciones,

                Estado =
                    cotizacion.Estado
            };
        }

        public async Task<MostrarCotizacionDTO?>
            ObtenerDetalleAsync(int idCotizacion)
        {
            var cotizacion =
                await _repositorio.ObtenerPorIdAsync(
                    idCotizacion
                );

            if (cotizacion == null)
                return null;

            return MapearMostrarCotizacion(cotizacion);
        }

        public async Task<List<Procedimiento>>
            ObtenerProcedimientosAsync()
        {
            return await _repositorio
                .ObtenerProcedimientosAsync();
        }

        public async Task<List<Medico>>
            ObtenerMedicosAsync()
        {
            return await _repositorio
                .ObtenerMedicosAsync();
        }

        public async Task<List<PacienteEntidad>>
            ObtenerPacientesAsync()
        {
            return await _repositorio
                .ObtenerPacientesAsync();
        }

        private static MostrarCotizacionDTO
            MapearMostrarCotizacion(Cotizacion cotizacion)
        {
            return new MostrarCotizacionDTO
            {
                IdCotizacion =
                    cotizacion.IdCotizacion,

                NombrePaciente = ConstruirNombre(
                    cotizacion.Paciente?.Usuario?.Nombres,
                    cotizacion.Paciente?.Usuario?.PrimerApellido,
                    cotizacion.Paciente?.Usuario?.SegundoApellido
                ),

                NombreMedico = ConstruirNombre(
                    cotizacion.Medico?.Usuario?.Nombres,
                    cotizacion.Medico?.Usuario?.PrimerApellido,
                    cotizacion.Medico?.Usuario?.SegundoApellido
                ),

                NombreProcedimiento =
                    cotizacion.Procedimiento?.Nombre
                    ?? "No disponible",

                FechaSolicitud =
                    cotizacion.FechaSolicitud,

                PrecioBase =
                    cotizacion.PrecioBase,

                HonorariosMedico =
                    cotizacion.HonorariosMedico,

                CostoEquipo =
                    cotizacion.CostoEquipo,

                CostoEstadia =
                    cotizacion.CostoEstadia,

                DiasEstadia =
                    cotizacion.DiasEstadia,

                Descuento =
                    cotizacion.Descuento,

                Impuesto =
                    cotizacion.Impuesto,

                Total =
                    cotizacion.Total,

                Observaciones =
                    cotizacion.Observaciones,

                Estado =
                    cotizacion.Estado,

                EstadoTexto =
                    ObtenerEstado(cotizacion.Estado)
            };
        }

        private static string ConstruirNombre(
            params string?[] partes)
        {
            return string.Join(
                " ",
                partes.Where(
                    parte =>
                        !string.IsNullOrWhiteSpace(parte)
                )
            );
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
    }
}