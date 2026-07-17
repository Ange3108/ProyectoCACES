using AutoMapper;
using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Cita;
using CACES.BLL.DTOs.Especialidad;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.Citas;


namespace CACES.BLL.Servicios.Citas
{
    public class CitaServicio : ICitaServicio
    {
        private readonly ICitaRepositorio _citaRepositorio;
        

        public CitaServicio(ICitaRepositorio citaRepositorio)
        {
            _citaRepositorio = citaRepositorio;
      
        }

        public async Task<respuestaErrores<List<MostrarCitaDTO>>> GetCitasAsync()
        {
            var respuesta = new respuestaErrores<List<MostrarCitaDTO>>();

            try
            {
                var citas = await _citaRepositorio.ObtenerTodasAsync();

                respuesta.EsCorrecto = true;
                respuesta.codigo = 200;
                respuesta.mensaje = "Citas obtenidas correctamente.";
                respuesta.Dato = citas.Select(MapearCita).ToList();
            }
            catch (Exception ex)
            {
                respuesta.EsCorrecto = false;
                respuesta.codigo = 500;
                respuesta.mensaje = "Error al obtener las citas: " + ex.Message;
                respuesta.Dato = new List<MostrarCitaDTO>();
            }

            return respuesta;
        }

        public async Task<respuestaErrores<MostrarCitaDTO>> RegistrarCitaAsync(
            RegistrarCitaDTO dto,
            int idPaciente)
        {
            try
            {
                if (dto == null)
                    return CrearError("Los datos de la cita son requeridos.", 400);

                if (idPaciente <= 0)
                    return CrearError("El paciente no es válido.", 400);

                if (dto.IdMedico <= 0)
                    return CrearError("Debe seleccionar un médico.", 400);

                if (dto.IdEspecialidad <= 0)
                    return CrearError("Debe seleccionar una especialidad.", 400);

                if (dto.FechaCita == default)
                    return CrearError("Debe seleccionar la fecha de la cita.", 400);

                if (dto.FechaCita.Date < DateTime.Today)
                    return CrearError("No se pueden registrar citas en fechas pasadas.", 400);

                if (dto.Hora == default)
                    return CrearError("Debe seleccionar la hora de la cita.", 400);

                if (string.IsNullOrWhiteSpace(dto.Motivo))
                    return CrearError("Debe ingresar el motivo de la cita.", 400);

                if (dto.Motivo.Trim().Length > 100)
                    return CrearError("El motivo no puede superar los 100 caracteres.", 400);

                var diaSemana = ConvertirDiaSemana(dto.FechaCita.DayOfWeek);

                var tieneHorario = await _citaRepositorio.TieneHorarioActivoAsync(
                    dto.IdMedico,
                    diaSemana
                );

                if (!tieneHorario)
                {
                    return CrearError(
                        "El médico no tiene un horario activo para el día seleccionado.",
                        400
                    );
                }

                var horarios = await _citaRepositorio.ObtenerHorariosAsync(dto.IdMedico);

                var horarioSeleccionado = horarios.FirstOrDefault(h =>
                    h.Id_Horario == dto.IdHorario &&
                    h.Activo
                );

                if (horarioSeleccionado == null)
                    return CrearError("El horario seleccionado no es válido.", 400);

                if (horarioSeleccionado.DiaSemana != diaSemana)
                {
                    return CrearError(
                        "El horario seleccionado no corresponde con el día de la fecha elegida.",
                        400
                    );
                }

                if (dto.Hora < horarioSeleccionado.HoraInicio )
                {
                    return CrearError(
                        "La hora seleccionada está fuera del horario disponible del médico.",
                        400
                    );
                }

                var existeCita = await _citaRepositorio.ExisteCitaAsync(
                    dto.IdMedico,
                    dto.FechaCita.Date,
                    dto.IdHorario
                );

                if (existeCita)
                {
                    return CrearError(
                        "El médico ya tiene una cita registrada para esa fecha y hora.",
                        409
                    );
                }

                var cita = new Cita
                {
                    IdPaciente = idPaciente,
                    IdMedico = dto.IdMedico,
                    IdEspecialidad = dto.IdEspecialidad,
                    IdHorario = dto.IdHorario,

                    Fecha = dto.FechaCita.Date,

                    Motivo = dto.Motivo.Trim(),

                    FechaDeRegistro = DateTime.Now,
                    FechaDeModificacion = null,
                    Estado = 1
                };


                var citaCreada = await _citaRepositorio.RegistrarAsync(cita);

                var citaCompleta = await _citaRepositorio.ObtenerEntidadPorIdAsync(
                    citaCreada.IdCita
                );

                return new respuestaErrores<MostrarCitaDTO>
                {
                    EsCorrecto = true,
                    codigo = 201,
                    mensaje = "Cita registrada correctamente.",
                    Dato = citaCompleta != null
                        ? MapearCita(citaCompleta)
                        : MapearCita(citaCreada)
                };
            }
            catch (Exception ex)
            {
                var detalle = ex.InnerException?.Message ?? ex.Message;

                return CrearError(
                    "Error al registrar la cita: " + detalle,
                    500
                );
            }
        }
        

        public async Task<respuestaErrores<List<MostrarCitaDTO>>>
            ObtenerCitasPorPacienteAsync(int idPaciente)
        {
            var respuesta = new respuestaErrores<List<MostrarCitaDTO>>();

            try
            {
                if (idPaciente <= 0)
                {
                    respuesta.EsCorrecto = false;
                    respuesta.codigo = 400;
                    respuesta.mensaje = "El paciente no es válido.";
                    respuesta.Dato = new List<MostrarCitaDTO>();

                    return respuesta;
                }

                var citas = await _citaRepositorio.ObtenerPorPacienteAsync(idPaciente);

                respuesta.EsCorrecto = true;
                respuesta.codigo = 200;
                respuesta.mensaje = "Citas del paciente obtenidas correctamente.";
                respuesta.Dato = citas.Select(MapearCita).ToList();
            }
            catch (Exception ex)
            {
                respuesta.EsCorrecto = false;
                respuesta.codigo = 500;
                respuesta.mensaje =
                    "Error al obtener las citas del paciente: " + ex.Message;
                respuesta.Dato = new List<MostrarCitaDTO>();
            }

            return respuesta;
        }

        public async Task<respuestaErrores<List<MostrarCitaDTO>>>
            ObtenerCitasPorMedicoAsync(int idMedico)
        {
            var respuesta = new respuestaErrores<List<MostrarCitaDTO>>();

            try
            {
                if (idMedico <= 0)
                {
                    respuesta.EsCorrecto = false;
                    respuesta.codigo = 400;
                    respuesta.mensaje = "El médico no es válido.";
                    respuesta.Dato = new List<MostrarCitaDTO>();

                    return respuesta;
                }

                var citas = await _citaRepositorio.ObtenerPorMedicoAsync(idMedico);

                respuesta.EsCorrecto = true;
                respuesta.codigo = 200;
                respuesta.mensaje = "Citas del médico obtenidas correctamente.";
                respuesta.Dato = citas.Select(MapearCita).ToList();
            }
            catch (Exception ex)
            {
                respuesta.EsCorrecto = false;
                respuesta.codigo = 500;
                respuesta.mensaje =
                    "Error al obtener las citas del médico: " + ex.Message;
                respuesta.Dato = new List<MostrarCitaDTO>();
            }

            return respuesta;
        }

        public async Task<respuestaErrores<List<MostrarCitaDTO>>>
            ObtenerCitasPorUsuarioMedicoAsync(int idUsuario)
        {
            var respuesta = new respuestaErrores<List<MostrarCitaDTO>>();

            try
            {
                if (idUsuario <= 0)
                {
                    respuesta.EsCorrecto = false;
                    respuesta.codigo = 400;
                    respuesta.mensaje = "El usuario no es válido.";
                    respuesta.Dato = new List<MostrarCitaDTO>();

                    return respuesta;
                }

                var medicos = await _citaRepositorio.ObtenerMedicosAsync();

                var medico = medicos.FirstOrDefault(m => m.IdUsuario == idUsuario);

                if (medico == null)
                {
                    respuesta.EsCorrecto = false;
                    respuesta.codigo = 404;
                    respuesta.mensaje =
                        "No se encontró un médico asociado al usuario actual.";
                    respuesta.Dato = new List<MostrarCitaDTO>();

                    return respuesta;
                }

                var citas = await _citaRepositorio.ObtenerPorMedicoAsync(
                    medico.IdMedico
                );

                respuesta.EsCorrecto = true;
                respuesta.codigo = 200;
                respuesta.mensaje = "Citas del médico obtenidas correctamente.";
                respuesta.Dato = citas.Select(MapearCita).ToList();
            }
            catch (Exception ex)
            {
                respuesta.EsCorrecto = false;
                respuesta.codigo = 500;
                respuesta.mensaje =
                    "Error al obtener las citas del usuario médico: " + ex.Message;
                respuesta.Dato = new List<MostrarCitaDTO>();
            }

            return respuesta;
        }

        public async Task<respuestaErrores<MostrarCitaDTO>> ObtenerTicketAsync(
            int idCita)
        {
            try
            {
                if (idCita <= 0)
                    return CrearError("La cita no es válida.", 400);

                var cita = await _citaRepositorio.ObtenerEntidadPorIdAsync(idCita);

                if (cita == null)
                    return CrearError("No se encontró la cita solicitada.", 404);

                return new respuestaErrores<MostrarCitaDTO>
                {
                    EsCorrecto = true,
                    codigo = 200,
                    mensaje = "Ticket obtenido correctamente.",
                    Dato = MapearCita(cita)
                };
            }
            catch (Exception ex)
            {
                return CrearError("Error al obtener el ticket: " + ex.Message, 500);
            }
        }

        public async Task<respuestaErrores<MostrarCitaDTO>> CancelarCitaAsync(int idCita)
        {
            try
            {
                var cita = await _citaRepositorio.ObtenerEntidadPorIdAsync(idCita);

                if (cita == null)
                    return CrearError("La cita no existe.", 404);

                cita.Estado = 0;

                var actualizada = await _citaRepositorio.ActualizarAsync(cita);

                return new respuestaErrores<MostrarCitaDTO>
                {
                    EsCorrecto = true,
                    codigo = 200,
                    mensaje = "Cita cancelada correctamente.",
                    Dato = MapearCita(actualizada)
                };
            }
            catch (Exception ex)
            {
                return CrearError(ex.Message, 500);
            }
        }

        public async Task<respuestaErrores<MostrarCitaDTO>> ActualizarFechaCitaAsync(int idCita, DateTime nuevaFecha)
        {
            try
            {
                var cita = await _citaRepositorio.ObtenerEntidadPorIdAsync(idCita);

                if (cita == null)
                    return CrearError("La cita no existe.", 404);

                cita.Fecha = nuevaFecha.Date;

                var actualizada = await _citaRepositorio.ActualizarAsync(cita);

                return new respuestaErrores<MostrarCitaDTO>
                {
                    EsCorrecto = true,
                    codigo = 200,
                    mensaje = "Cita actualizada correctamente.",
                    Dato = MapearCita(actualizada)
                };
            }
            catch (Exception ex)
            {
                return CrearError(ex.Message, 500);
            }
        }

        public async Task<respuestaErrores<MostrarCitaDTO>> CancelarCitasPorMedicoYFechaAsync(int idMedico, DateTime fechaCita)
        {
            var respuesta = new respuestaErrores<MostrarCitaDTO>();

            try
            {
                var citas = await _citaRepositorio.ObtenerPorMedicoAsync(idMedico);

                var lista = citas.Where(x => x.Fecha.Date == fechaCita.Date).ToList();

                foreach (var cita in lista)
                {
                    cita.Estado = 0;
                    await _citaRepositorio.ActualizarAsync(cita);
                }

                respuesta.EsCorrecto = true;
                respuesta.codigo = 200;
                respuesta.mensaje = "Citas canceladas correctamente.";
            }
            catch (Exception ex)
            {
                respuesta.EsCorrecto = false;
                respuesta.codigo = 500;
                respuesta.mensaje = ex.Message;
            }

            return respuesta;
        }

        public async Task<respuestaErrores<List<CitaComboDTO>>> ObtenerMedicosAsync()
        {
            var respuesta = new respuestaErrores<List<CitaComboDTO>>();

            var medicos = await _citaRepositorio.ObtenerMedicosAsync();

            respuesta.Dato = medicos.Select(x => new CitaComboDTO
            {
                Id = x.IdMedico,
                Nombre = $"{x.Usuario.Nombres} {x.Usuario.PrimerApellido}"
            }).ToList();

            return respuesta;
        }

        public async Task<respuestaErrores<List<CitaComboDTO>>> ObtenerEspecialidadesActivasAsync()
        {
            var respuesta = new respuestaErrores<List<CitaComboDTO>>();

            var especialidades = await _citaRepositorio.ObtenerEspecialidadesAsync();

            respuesta.Dato = especialidades.Select(x => new CitaComboDTO
            {
                Id = x.IdEspecialidad,
                Nombre = x.Nombre
            }).ToList();

            return respuesta;
        }

        public async Task<respuestaErrores<List<CitaHorarioDTO>>> ObtenerHorariosPorMedicoAsync(int idMedico)
        {
            var respuesta = new respuestaErrores<List<CitaHorarioDTO>>();

            var horarios = await _citaRepositorio.ObtenerHorariosAsync(idMedico);

            respuesta.Dato = horarios.Select(h => new CitaHorarioDTO
            {
                IdHorario = h.Id_Horario,
                DiaSemana = h.DiaSemana,
                HoraInicio = h.HoraInicio,
                DiaTexto = ObtenerNombreDia(h.DiaSemana),
                HorarioTexto = $"{ObtenerNombreDia(h.DiaSemana)} {h.HoraInicio:hh\\:mm}"
            }).ToList();

            return respuesta;
        }

        private MostrarCitaDTO MapearCita(Cita cita)
        {
            return new MostrarCitaDTO
            {
                IdCita = cita.IdCita,
                IdPaciente = cita.IdPaciente,
                IdMedico = cita.IdMedico,
                IdEspecialidad = cita.IdEspecialidad,
                IdHorario = cita.IdHorario,

                IdReceta = cita.Receta?.IdReceta,

                FechaCita = cita.Fecha,

                Hora = cita.Horario != null
                    ? cita.Horario.HoraInicio
                    : TimeSpan.Zero,

                Motivo = cita.Motivo,
                Estado = cita.Estado,

                NombrePaciente = cita.Paciente?.Usuario != null
                    ? $"{cita.Paciente.Usuario.Nombres} {cita.Paciente.Usuario.PrimerApellido}"
                    : string.Empty,

                NombreMedico = cita.Medico?.Usuario != null
                    ? $"{cita.Medico.Usuario.Nombres} {cita.Medico.Usuario.PrimerApellido}"
                    : string.Empty,

                NombreEspecialidad = cita.Especialidad?.Nombre ?? string.Empty
            };
        }

        private respuestaErrores<MostrarCitaDTO> CrearError(string mensaje, int codigo)
        {
            return new respuestaErrores<MostrarCitaDTO>
            {
                EsCorrecto = false,
                codigo = codigo,
                mensaje = mensaje
            };
        }

        private int ConvertirDiaSemana(DayOfWeek dia)
        {
            return dia switch
            {
                DayOfWeek.Monday => 1,
                DayOfWeek.Tuesday => 2,
                DayOfWeek.Wednesday => 3,
                DayOfWeek.Thursday => 4,
                DayOfWeek.Friday => 5,
                DayOfWeek.Saturday => 6,
                DayOfWeek.Sunday => 7,
                _ => 0
            };
        }

        private string ObtenerNombreDia(int dia)
        {
            return dia switch
            {
                1 => "Lunes",
                2 => "Martes",
                3 => "Miércoles",
                4 => "Jueves",
                5 => "Viernes",
                6 => "Sábado",
                7 => "Domingo",
                _ => ""
            };
        }
    }
}