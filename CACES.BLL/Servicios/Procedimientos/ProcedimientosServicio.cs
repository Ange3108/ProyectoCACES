using AutoMapper;
using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Procedimientos;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.Procedimientos;
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

            // 2. Extraer la hora exacta elegida
            TimeSpan horaElegida = registrarProcedimientosDto.FechaProgramada.TimeOfDay;

            // 3. Consultar al repositorio (aquí se ejecuta el LINQ de forma aislada)
            var horarioDisponible = await _procedimientosRepositorio.ObtenerHorarioPorRangoAsync(
                registrarProcedimientosDto.Id_Medico,
                diaSemanaSql,
                horaElegida
            );

            // Si el repositorio no encuentra ningún bloque de horario activo que calce, se rechaza
            if (horarioDisponible == null)
            {
                return false;
            }

            // 4. Mapear y enlazar el ID del horario encontrado a la nueva cirugía
            var nuevaCirugia = new Cirugias
            {
                Id_Paciente = registrarProcedimientosDto.Id_Paciente,
                Id_Medico = registrarProcedimientosDto.Id_Medico,
                Id_Procedimiento = registrarProcedimientosDto.Id_Procedimiento,
                Id_Horario = horarioDisponible.Id_Horario, // Seteamos la FK existente
                Estado = true
            };

            // 5. Mandar a guardar la cirugía
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


    }
}

