using AutoMapper;
using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Procedimientos;
using CACES.BLL.Servicios.Especialidad;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.Especialidades;
using CACES.DAL.Repositorios.Procedimientos;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace CACES.BLL.Servicios.Procedimientos
{
    public class ProcedimientosServicio : IProcedimientosServicio
    {
        private readonly IProcedimientosRepositorio _procedimientosRepositorio;
        private readonly IMapper _mapper;
        public ProcedimientosServicio(IProcedimientosRepositorio procedimientosRepositorio, IMapper mapper)
        {
            _procedimientosRepositorio = procedimientosRepositorio;
            _mapper = mapper;
        }

        public async Task<List<MostrarProcedimientosDTO>> ObtenerDetalleCirugiaAsync(int idPaciente)
        {
            var listaCirugias = await _procedimientosRepositorio.ObtenerDetalleCirugiaAsync(idPaciente);
            return MapearListaCirugiasAMostrarDTO(listaCirugias);
        }

        public async Task<bool> ActualizarProcedimientoAsync(EditarProcedimientosDTO editarProcedimientosDTO, int idPaciente)
        {
            var listaProcedimientos = await _procedimientosRepositorio.ObtenerDetalleCirugiaAsync(idPaciente);
            var cirugia = listaProcedimientos.FirstOrDefault(c => c.Id_Cirugia == editarProcedimientosDTO.Id_Procedimiento);
            if(cirugia == null) return false;
            cirugia.Estado = editarProcedimientosDTO.Estado;
            if(cirugia.Horario != null)
            {
                cirugia.Horario.DiaSemana = (int)editarProcedimientosDTO.Fecha.DayOfWeek;
                cirugia.Horario.HoraInicio = editarProcedimientosDTO.Fecha.TimeOfDay;
                
            }
            if (cirugia.Procedimiento != null)
            {
                cirugia.Procedimiento.Descripcion = editarProcedimientosDTO.Descripcion;
            }
            return await _procedimientosRepositorio.ActualizarProcedimientoAsync(cirugia);
        }


        public async Task<EditarProcedimientosDTO?> ObtenerPorIdParaEditarAsync(int idPaciente, int idCirugia)
        {
            var listaCirugias = await _procedimientosRepositorio.ObtenerDetalleCirugiaAsync(idPaciente);
            var cirugia = listaCirugias.FirstOrDefault(c => c.Id_Cirugia == idCirugia);

            if (cirugia == null) return null;

            DateTime fechaFinalCita = DateTime.Today;
            if (cirugia.Horario != null)
            {
                DayOfWeek diaObjetivo = (DayOfWeek)cirugia.Horario.DiaSemana;
                while (fechaFinalCita.DayOfWeek != diaObjetivo)
                {
                    fechaFinalCita = fechaFinalCita.AddDays(1);
                }
                fechaFinalCita = fechaFinalCita.Add(cirugia.Horario.HoraInicio);
            }

            return new EditarProcedimientosDTO
            {
                Id_Procedimiento = cirugia.Id_Cirugia,
                Nombre = cirugia.Procedimiento?.Nombre ?? "Procedimiento Desconocido",
                NombreMedico = cirugia.Medico?.Usuario != null ? $"Dr. {cirugia.Medico.Usuario.Nombres}" : "Médico No Asignado",
                NombrePaciente = cirugia.Paciente?.Usuario != null ? cirugia.Paciente.Usuario.Nombres : "Paciente Desconocido",
                PrimerApellidoPaciente = cirugia.Paciente?.Usuario?.PrimerApellido ?? "Desconocido",
                SegundoApellidoPaciente = cirugia.Paciente?.Usuario?.SegundoApellido ?? "",
                Fecha = fechaFinalCita,
                Estado = cirugia.Estado,
                Descripcion = cirugia.Procedimiento?.Descripcion ?? "Sin indicaciones particulares."
            };
        }

        public async Task<bool> RegistrarProcedimientoAsync(RegistrarProcedimientosDto registrarProcedimientosDto)
        {
            int diaSemanaNet = (int)registrarProcedimientosDto.FechaProgramada.DayOfWeek;
            int diaSemanaSql = diaSemanaNet == 0 ? 6 : diaSemanaNet - 1;

            TimeSpan horaElegida = registrarProcedimientosDto.FechaProgramada.TimeOfDay;

            var horarioDisponible = await _procedimientosRepositorio.ObtenerHorarioPorRangoAsync(
                registrarProcedimientosDto.Id_Medico,
                diaSemanaSql,
                horaElegida
            );

            if (horarioDisponible == null)
            {
                return false;
            }

            var nuevaCirugia = new Cirugias
            {
                Id_Paciente = registrarProcedimientosDto.Id_Paciente,
                Id_Medico = registrarProcedimientosDto.Id_Medico,
                Id_Procedimiento = registrarProcedimientosDto.Id_Procedimiento,
                Id_Horario = horarioDisponible.Id_Horario, 
                Estado = true
            };

            return await _procedimientosRepositorio.RegistrarProcedimientosAsync(nuevaCirugia);
        }

        public async Task<List<Procedimiento>> ObtenerProcedimientosFijosAsync()
        {
            return await _procedimientosRepositorio.ObtenerProcedimientosFijosAsync();
        }

        public async Task<List<MostrarProcedimientosDTO>> ObtenerCirugiasPorMedicoAsync(int idMedico)
        {
            var listaCirugias = await _procedimientosRepositorio.ObtenerCirugiasPorMedicoAsync(idMedico);

            return MapearListaCirugiasAMostrarDTO(listaCirugias);
        }

        public async Task<List<MostrarProcedimientosDTO>> ObtenerTodasLasCirugiasAsync()
        {
            var listaCirugias = await _procedimientosRepositorio.ObtenerTodasLasCirugiasAsync();
            return MapearListaCirugiasAMostrarDTO(listaCirugias);
        }

        private List<MostrarProcedimientosDTO> MapearListaCirugiasAMostrarDTO(List<Cirugias> listaCirugias)
        {
            var listaDtos = new List<MostrarProcedimientosDTO>();

            foreach (var cirugia in listaCirugias)
            {
                DateTime fechaFinalCita = DateTime.Today;

                if (cirugia.Horario != null)
                {
                    DayOfWeek diaObjetivo = (DayOfWeek)cirugia.Horario.DiaSemana;
                    while (fechaFinalCita.DayOfWeek != diaObjetivo)
                    {
                        fechaFinalCita = fechaFinalCita.AddDays(1);
                    }
                    fechaFinalCita = fechaFinalCita.Add(cirugia.Horario.HoraInicio);
                }

                string nombreDelMedico = cirugia.Medico?.Usuario != null
                    ? $"Dr. {cirugia.Medico.Usuario.Nombres}"
                    : "Médico No Asignado";

                string nombreDelPaciente = "Paciente Desconocido";
                if (cirugia.Paciente?.Usuario != null)
                {
                    var u = cirugia.Paciente.Usuario;
                    nombreDelPaciente = $"{u.Nombres} {u.PrimerApellido} {u.SegundoApellido}".Trim();
                }

                listaDtos.Add(new MostrarProcedimientosDTO
                {
                    Id_Cirugia = cirugia.Id_Cirugia,
                    Id_Paciente = cirugia.Id_Paciente,
                    Nombre = cirugia.Procedimiento?.Nombre ?? "Procedimiento Desconocido",
                    NombreMedico = nombreDelMedico,
                    NombrePaciente = nombreDelPaciente,
                    PrimerApellidoPaciente = cirugia.Paciente?.Usuario?.PrimerApellido ?? "Desconocido",
                    SegundoApellidoPaciente = cirugia.Paciente?.Usuario?.SegundoApellido ?? "",
                    Fecha = fechaFinalCita,
                    Estado = cirugia.Estado,
                    Descripcion = cirugia.Procedimiento?.Descripcion ?? "Sin indicaciones particulares."
                });
            }

            return listaDtos;
        }

        public async Task<respuestaErrores<MostrarProcedimientosDTO>> ObtenerDatosReporteAsync(int idCirugia)
        {
            var respuesta = new respuestaErrores<MostrarProcedimientosDTO>();

            var listaCirugias = await _procedimientosRepositorio.ObtenerTodasLasCirugiasAsync();
            var cirugiaFisica = listaCirugias.FirstOrDefault(c => c.Id_Cirugia == idCirugia);

            if (cirugiaFisica == null)
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "El registro de la cirugía o procedimiento no fue encontrado en el sistema.";
                respuesta.codigo = 404;
                return respuesta;
            }

            var listaDtoMapeada = MapearListaCirugiasAMostrarDTO(new List<Cirugias> { cirugiaFisica });
            var dto = listaDtoMapeada.First();

            respuesta.EsCorrecto = true;
            respuesta.mensaje = "Información del procedimiento cargada con éxito.";
            respuesta.Dato = dto;

            return respuesta;
        }

        public async Task<List<InsertarProcedimientosDto>> ListarProcedimientosAsync()
        {
            var entidades = await _procedimientosRepositorio.ObtenerTodosLosProcedimientosAsync();
            return _mapper.Map<List<InsertarProcedimientosDto>>(entidades);
        }

        public async Task<bool> GuardarProcedimientoAsync(InsertarProcedimientosDto dto)
        {
            var entidad = _mapper.Map<Procedimiento>(dto);

            entidad.Estado = dto.Estado;

            return await _procedimientosRepositorio.InsertarProcedimientoAsync(entidad);
        }

        public async Task<InsertarProcedimientosDto> ObtenerPorIdAsync(int id)
        {
            var entidad = await _procedimientosRepositorio.ObtenerProcedimientoPorIdAsync(id);
            return _mapper.Map<InsertarProcedimientosDto>(entidad);
        }
        public async Task<bool> EditarProcedimientoAdminAsync(InsertarProcedimientosDto dto)
        {
            try
            {
                var entidad = _mapper.Map<Procedimiento>(dto);

                entidad.Estado = dto.Estado;

                return await _procedimientosRepositorio.ActualizarProcedimientoAdminAsync(entidad);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CambiarEstadoProcedimientoAsync(int id)
        {
            try
            {
                return await _procedimientosRepositorio.CambiarEstadoProcedimientoAsync(id);
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}

