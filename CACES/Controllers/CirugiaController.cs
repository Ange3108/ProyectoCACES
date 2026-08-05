using CACES.BLL.DTOs.Cita;
using CACES.BLL.Servicios.Cirugia;
using CACES.BLL.Servicios.Citas;
using CACES.BLL.Servicios.Paciente;
using CACES.BLL.Servicios.SeguimientoPaciente;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CACES.Controllers
{
    public class CirugiaController : Controller
    {
        private readonly ICirugiaServicio _cirugiaServicio;
        private readonly ICitaServicio _citaServicio;
        private readonly IPacienteServicio _pacienteServicio;
        private readonly ISeguimientoPacienteServicio _seguimientoServicio;

        public CirugiaController(
            ICirugiaServicio cirugiaServicio,
            ICitaServicio citaServicio,
            IPacienteServicio pacienteServicio,
            ISeguimientoPacienteServicio seguimientoServicio)
        {
            _cirugiaServicio = cirugiaServicio;
            _citaServicio = citaServicio;
            _pacienteServicio = pacienteServicio;
            _seguimientoServicio = seguimientoServicio;
        }

        // GET Views
        [Authorize(Roles = "Paciente")]
        [HttpGet]
        public IActionResult AgendarCirugia()
        {
            return View();
        }

        [Authorize(Roles = "Paciente")]
        [HttpGet]
        public IActionResult MisCirugias()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult GestionCirugias()
        {
            return View();
        }

        // JSON API Endpoints
        [Authorize(Roles = "Paciente")]
        [HttpPost]
        public async Task<IActionResult> AgendarCirugiaJson([FromBody] RegistrarCitaDTO dto)
        {
            var idUsuario = ObtenerIdUsuarioActual();

            if (idUsuario == null)
                return Unauthorized();

            var pacienteResultado = await _pacienteServicio.GetPacienteByUsuarioIdAsync(idUsuario.Value);

            if (!pacienteResultado.EsCorrecto || pacienteResultado.Dato == null)
                return NotFound(pacienteResultado);

            // Registrar la cita que automáticamente crea la cirugía
            var resultado = await _citaServicio.RegistrarCitaAsync(
                dto,
                pacienteResultado.Dato.IdPaciente
            );

            return Json(resultado);
        }

        [Authorize(Roles = "Paciente")]
        [HttpGet]
        public async Task<IActionResult> ObtenerMisCirugias()
        {
            var idUsuario = ObtenerIdUsuarioActual();

            if (idUsuario == null)
                return Unauthorized();

            var pacienteResultado = await _pacienteServicio.GetPacienteByUsuarioIdAsync(idUsuario.Value);

            if (!pacienteResultado.EsCorrecto || pacienteResultado.Dato == null)
                return NotFound(pacienteResultado);

            var resultado = await _cirugiaServicio.ObtenerCirugiaPorPacienteAsync(
                pacienteResultado.Dato.IdPaciente
            );

            return Json(resultado);
        }

        [Authorize(Roles = "Administrador,Medico")]
        [HttpGet]
        public async Task<IActionResult> ObtenerListadoCirugias()
        {
            return Json(await _cirugiaServicio.GetAllCirugiaAsync());
        }

        [Authorize(Roles = "Administrador,Medico")]
        [HttpGet]
        public async Task<IActionResult> ObtenerCirugia(int id)
        {
            var resultado = await _cirugiaServicio.GetCirugiaByIdAsync(id);
            return Json(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> CompletarCirugia(int id)
        {
            var resultado = await _cirugiaServicio.CompletarCirugia(id);
            return Json(resultado);
        }

        [Authorize(Roles = "Paciente,Medico,Administrador")]
        [HttpPost]
        public async Task<IActionResult> CancelarCirugia(int idCirugia)
        {
            return Json(await _cirugiaServicio.CancelarCirugiaAsync(idCirugia));
        }

        // Helper methods
        [Authorize(Roles = "Paciente,Administrador")]
        [HttpGet]
        public async Task<IActionResult> ObtenerEspecialidadesActivas()
        {
            return Json(await _citaServicio.ObtenerEspecialidadesActivasAsync());
        }

        [Authorize(Roles = "Paciente,Administrador")]
        [HttpGet]
        public async Task<IActionResult> ObtenerCirugiasFijos(int? idEspecialidad)
        {
            var resultado = await _citaServicio.ObtenerProcedimientosFijosAsync(idEspecialidad);

            if (resultado.EsCorrecto)
            {
                return Json(resultado.Dato);
            }

            return Json(new { error = resultado.mensaje });
        }

        [Authorize(Roles = "Paciente,Administrador")]
        [HttpGet]
        public async Task<IActionResult> ObtenerMedicos(int? idEspecialidad)
        {
            return Json(await _citaServicio.ObtenerMedicosAsync(idEspecialidad));
        }

        [Authorize(Roles = "Paciente,Administrador")]
        [HttpGet]
        public async Task<IActionResult> ObtenerHorariosPorMedico(int idMedico)
        {
            return Json(
                await _citaServicio.ObtenerHorariosPorMedicoAsync(
                    idMedico
                )
            );
        }

        private int? ObtenerIdUsuarioActual()
        {
            var claimId = User
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            return int.TryParse(claimId, out var idUsuario)
                ? idUsuario
                : null;
        }
        [Authorize(Roles = "Administrador,Medico")]
        [HttpGet]
        public async Task<IActionResult> ObtenerSeguimientoPorCirugia(int idCirugia)
        {
            var resultado = await _seguimientoServicio.ObtenerPorCirugia(idCirugia);
            return Json(resultado);
        }

        //endpoint de prueba para enviar recordatorios de cirugias del dia
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> TestRecordatorios()
        {
            var resultado = await _seguimientoServicio.EnviarRecordatoriosDelDiaAsync();
            return Json(resultado);
        }
    }
}
