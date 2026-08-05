using CACES.BLL.DTOs;
using CACES.BLL.DTOs.SolicitudMedico;
using CACES.DAL.Repositorios.SolicitudMedicos;
using SolicitudMedicoEntidad = CACES.DAL.Entidades.SolicitudMedico;

namespace CACES.BLL.Servicios.SolicitudMedico
{
    public class SolicitudMedicoServicio : ISolicitudMedicoServicio
    {
        private readonly ISolicitudMedicoRepositorio _repositorio;

        public SolicitudMedicoServicio(
            ISolicitudMedicoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<respuestaErrores<int>> RegistrarAsync(
            RegistrarSolicitudMedicoDTO dto)
        {
            var respuesta = new respuestaErrores<int>();

            try
            {
                if (dto == null)
                {
                    respuesta.EsCorrecto = false;
                    respuesta.mensaje =
                        "No se recibió la información de la solicitud.";

                    return respuesta;
                }

                var correo = dto.CorreoElectronico
                    .Trim()
                    .ToLowerInvariant();

                var solicitudExistente =
                    await _repositorio
                        .ObtenerPendientePorCorreoAsync(correo);

                if (solicitudExistente != null)
                {
                    respuesta.EsCorrecto = false;
                    respuesta.mensaje =
                        "Ya existe una solicitud pendiente o en revisión para este correo.";

                    return respuesta;
                }

                if (string.IsNullOrWhiteSpace(dto.Curriculum))
                {
                    respuesta.EsCorrecto = false;
                    respuesta.mensaje =
                        "Debe adjuntar el currículum.";

                    return respuesta;
                }

                var solicitud = new SolicitudMedicoEntidad
                {
                    Nombres = dto.Nombres.Trim(),
                    PrimerApellido = dto.PrimerApellido.Trim(),
                    SegundoApellido = dto.SegundoApellido?.Trim(),
                    CorreoElectronico = correo,
                    Telefono = dto.Telefono.Trim(),
                    IdEspecialidad = dto.IdEspecialidad,
                    AniosExperiencia = dto.AniosExperiencia,
                    Certificaciones = dto.Certificaciones?.Trim(),
                    Motivo = dto.Motivo.Trim(),
                    Curriculum = dto.Curriculum.Trim(),
                    Foto = string.IsNullOrWhiteSpace(dto.Foto)
                        ? null
                        : dto.Foto.Trim(),
                    Estado = 1,
                    FechaSolicitud = DateTime.UtcNow
                };

                var registrada =
                    await _repositorio.RegistrarAsync(solicitud);

                respuesta.EsCorrecto = true;
                respuesta.Dato = registrada.IdSolicitud;
                respuesta.mensaje =
                    "La solicitud fue enviada correctamente. Le notificaremos cuando sea revisada.";
            }
            catch (Exception ex)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje =
                    "No fue posible registrar la solicitud: " +
                    (ex.InnerException?.Message ?? ex.Message);
            }

            return respuesta;
        }

        public async Task<respuestaErrores<List<MostrarSolicitudMedicoDTO>>>
            ObtenerTodasAsync()
        {
            var respuesta =
                new respuestaErrores<List<MostrarSolicitudMedicoDTO>>();

            try
            {
                var solicitudes =
                    await _repositorio.ObtenerTodasAsync();

                respuesta.EsCorrecto = true;
                respuesta.Dato = solicitudes
                    .Select(MapearSolicitud)
                    .ToList();
            }
            catch (Exception ex)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje =
                    "No fue posible cargar las solicitudes: " +
                    (ex.InnerException?.Message ?? ex.Message);
            }

            return respuesta;
        }

        public async Task<respuestaErrores<MostrarSolicitudMedicoDTO>>
            ObtenerPorIdAsync(int idSolicitud)
        {
            var respuesta =
                new respuestaErrores<MostrarSolicitudMedicoDTO>();

            try
            {
                if (idSolicitud <= 0)
                {
                    respuesta.EsCorrecto = false;
                    respuesta.mensaje =
                        "La solicitud seleccionada no es válida.";

                    return respuesta;
                }

                var solicitud =
                    await _repositorio.ObtenerPorIdAsync(idSolicitud);

                if (solicitud == null)
                {
                    respuesta.EsCorrecto = false;
                    respuesta.mensaje =
                        "No se encontró la solicitud.";

                    return respuesta;
                }

                respuesta.EsCorrecto = true;
                respuesta.Dato = MapearSolicitud(solicitud);
            }
            catch (Exception ex)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje =
                    "No fue posible consultar la solicitud: " +
                    (ex.InnerException?.Message ?? ex.Message);
            }

            return respuesta;
        }

        public async Task<respuestaErrores<MostrarSolicitudMedicoDTO>>
            ResponderAsync(ResponderSolicitudMedicoDTO dto)
        {
            var respuesta =
                new respuestaErrores<MostrarSolicitudMedicoDTO>();

            try
            {
                if (dto == null)
                {
                    respuesta.EsCorrecto = false;
                    respuesta.mensaje =
                        "No se recibió la respuesta de la solicitud.";

                    return respuesta;
                }

                if (dto.Estado != 3 && dto.Estado != 4)
                {
                    respuesta.EsCorrecto = false;
                    respuesta.mensaje =
                        "Debe aprobar o rechazar la solicitud.";

                    return respuesta;
                }

                var solicitud =
                    await _repositorio.ObtenerPorIdAsync(dto.IdSolicitud);

                if (solicitud == null)
                {
                    respuesta.EsCorrecto = false;
                    respuesta.mensaje =
                        "La solicitud no existe.";

                    return respuesta;
                }

                if (solicitud.Estado == 3 ||
                    solicitud.Estado == 4)
                {
                    respuesta.EsCorrecto = false;
                    respuesta.mensaje =
                        "La solicitud ya fue respondida.";

                    return respuesta;
                }

                solicitud.Estado = dto.Estado;
                solicitud.ObservacionAdministrador =
                    dto.ObservacionAdministrador?.Trim();
                solicitud.FechaRespuesta = DateTime.UtcNow;

                var actualizada =
                    await _repositorio.ActualizarAsync(solicitud);

                respuesta.EsCorrecto = true;
                respuesta.Dato = MapearSolicitud(actualizada);
                respuesta.mensaje = dto.Estado == 3
                    ? "La solicitud fue aprobada correctamente."
                    : "La solicitud fue rechazada correctamente.";
            }
            catch (Exception ex)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje =
                    "No fue posible responder la solicitud: " +
                    (ex.InnerException?.Message ?? ex.Message);
            }

            return respuesta;
        }

        public async Task<respuestaErrores<List<EspecialidadSolicitudDTO>>>
            ObtenerEspecialidadesAsync()
        {
            var respuesta =
                new respuestaErrores<List<EspecialidadSolicitudDTO>>();

            try
            {
                var especialidades =
                    await _repositorio
                        .ObtenerEspecialidadesActivasAsync();

                respuesta.EsCorrecto = true;
                respuesta.Dato = especialidades
                    .Select(e => new EspecialidadSolicitudDTO
                    {
                        IdEspecialidad = e.IdEspecialidad,
                        Nombre = e.Nombre
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje =
                    "No fue posible cargar las especialidades: " +
                    (ex.InnerException?.Message ?? ex.Message);
            }

            return respuesta;
        }

        private static MostrarSolicitudMedicoDTO MapearSolicitud(
            SolicitudMedicoEntidad solicitud)
        {
            return new MostrarSolicitudMedicoDTO
            {
                IdSolicitud = solicitud.IdSolicitud,

                NombreCompleto = string.Join(
                    " ",
                    new[]
                    {
                        solicitud.Nombres,
                        solicitud.PrimerApellido,
                        solicitud.SegundoApellido
                    }
                    .Where(parte =>
                        !string.IsNullOrWhiteSpace(parte))
                ),

                CorreoElectronico = solicitud.CorreoElectronico,
                Telefono = solicitud.Telefono,
                IdEspecialidad = solicitud.IdEspecialidad,
                NombreEspecialidad =
                    solicitud.Especialidad?.Nombre
                    ?? "Sin especialidad",
                AniosExperiencia = solicitud.AniosExperiencia,
                Certificaciones = solicitud.Certificaciones,
                Motivo = solicitud.Motivo,
                Curriculum = solicitud.Curriculum,
                Foto = solicitud.Foto,
                Estado = solicitud.Estado,
                EstadoTexto =
                    ObtenerEstadoTexto(solicitud.Estado),
                ObservacionAdministrador =
                    solicitud.ObservacionAdministrador,
                FechaSolicitud = solicitud.FechaSolicitud,
                FechaRespuesta = solicitud.FechaRespuesta
            };
        }

        private static string ObtenerEstadoTexto(byte estado)
        {
            return estado switch
            {
                1 => "Pendiente",
                2 => "En revisión",
                3 => "Aprobada",
                4 => "Rechazada",
                _ => "Desconocido"
            };
        }
    }
}