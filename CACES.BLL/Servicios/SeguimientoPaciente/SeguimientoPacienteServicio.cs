using CACES.BLL.DTOs;
using CACES.BLL.DTOs.SeguimientoPostOperatorio;
using CACES.BLL.Mappers;
using CACES.BLL.Servicios.Notificacion;
using CACES.DAL.Entidades.SeguimientoPostOperatorio;
using CACES.DAL.Repositorios.Base;
using CACES.DAL.Repositorios.SeguimientoPaciente;


namespace CACES.BLL.Servicios.SeguimientoPaciente
{
    public class SeguimientoPacienteServicio : ISeguimientoPacienteServicio

    {

        private readonly IRepositorioGenerico<ConfiguracionCheckpoints> _repositorioCheckpoints;
        private readonly ISeguimientoPacienteRepositorio _seguimientoPacienteRepositorio;
        private readonly INotificadorServicio _notificadorServicio;

        public SeguimientoPacienteServicio(IRepositorioGenerico<ConfiguracionCheckpoints> repositorioCheckpoints, ISeguimientoPacienteRepositorio seguimientoPacienteRepositorio, INotificadorServicio notificadorServicio)
        {
            _repositorioCheckpoints = repositorioCheckpoints;
            _seguimientoPacienteRepositorio = seguimientoPacienteRepositorio;
            _notificadorServicio = notificadorServicio;
        }
        public async Task<respuestaErrores<List<MostrarSeguimientoPacienteDTO>>> ObtenerTodos()
        {
            var respuesta = new respuestaErrores<List<MostrarSeguimientoPacienteDTO>>();
            var lista = await _seguimientoPacienteRepositorio.ObtenerTodos();
            var dtos = lista.Select(s => s.ToMostrarSeguimientoPacienteDTO()!).ToList();

            respuesta.EsCorrecto = true;
            respuesta.Dato = dtos;
            respuesta.codigo = 200;
            respuesta.mensaje = "Seguimientos obtenidos correctamente";
            return respuesta;
        }
        public async Task<respuestaErrores<bool>> GenerarCheckpoints(int idCirugia)
        {
            var respuesta = new respuestaErrores<bool>();
            var cirugia = await _seguimientoPacienteRepositorio.ObtenerCirugiaConFecha(idCirugia);

            if (cirugia?.Cita?.Horario == null)
            { 
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "No se encontró la cirugía o la cita asociada";
            return respuesta;
             }

            // Evitar duplicar checkpoints si ya se generaron antes
            var existentes = await _seguimientoPacienteRepositorio.ObtenerPorCirugia(idCirugia);
            if (existentes.Any())
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "Los checkpoints ya fueron generados para esta cirugía";
                return respuesta;
            }

            var fechaCirugia = cirugia.Cita.Fecha; 
            var checkpointsActivos = await _repositorioCheckpoints.ObtenerActivos();

            if (!checkpointsActivos.Any())
            {
                respuesta.EsCorrecto = false;
                respuesta.mensaje = "No hay checkpoints activos para generar";
                return respuesta;
            }

            var nuevos = checkpointsActivos.Select(c => new DAL.Entidades.SeguimientoPostOperatorio.SeguimientoPaciente
            {
                Id_Cirugia = idCirugia,
                DiaCheckpoint = c.DiaCheckPoint,
                FechaProgramada = fechaCirugia.AddDays(c.DiaCheckPoint),
                Estado = EstadoSeguimiento.Pendiente,
                FechaRegistro = null
            }).ToList();

            await _seguimientoPacienteRepositorio.AgregarRango(nuevos);
            respuesta.EsCorrecto = true;
            respuesta.mensaje = "Checkpoints generados correctamente";
            return respuesta;
        }

        public async Task<respuestaErrores<List<MostrarSeguimientoPacienteDTO>>> ObtenerPorCirugia(int idCirugia)
        {
            var respuesta = new respuestaErrores<List<MostrarSeguimientoPacienteDTO>>();
            var lista = await _seguimientoPacienteRepositorio.ObtenerPorCirugia(idCirugia);
            if(lista.Count == 0)
            {
                respuesta.EsCorrecto = false;
                respuesta.Dato = null;
                respuesta.codigo = 404;
                respuesta.mensaje = "No se encontraron datos para la cirugía especificada";
                return respuesta;
            }
            var dtos = lista.Select(s => s.ToMostrarSeguimientoPacienteDTO()!).ToList();

            respuesta.EsCorrecto = true;
            respuesta.Dato = dtos;
            respuesta.codigo = 200;
            respuesta.mensaje = "Datos obtenidos correctamente";

            return respuesta;
        }

        public async Task<respuestaErrores<int>> EnviarRecordatoriosDelDiaAsync()
        {
            var respuesta = new respuestaErrores<int>();

            var pendientesHoy = await _seguimientoPacienteRepositorio.ObtenerProgramadosParaHoy();

            int procesados = 0;
            foreach (var seguimiento in pendientesHoy)
            {
                var idUsuario = seguimiento.Cirugia.Paciente.IdUsuario; 

                await _notificadorServicio.NotificarAsync(
                    evento: "RecordatorioCheckpoint",
                    idUsuario: idUsuario,
                    titulo: "Recordatorio de seguimiento post-operatorio",
                    mensaje: $"Es momento de completar tu encuesta de seguimiento (día {seguimiento.DiaCheckpoint})."
                );

                procesados++;
            }

            respuesta.EsCorrecto = true;
            respuesta.Dato = procesados;
            respuesta.mensaje = $"{procesados} recordatorios procesados";
            return respuesta;
        }
    }
}
