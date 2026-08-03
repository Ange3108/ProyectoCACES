using CACES.BLL.DTOs.Cita;
using CACES.BLL.DTOs.Procedimientos;
using CACES.BLL.Servicios.Citas;
using CACES.BLL.Servicios.Paciente;
using CACES.DAL.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CACES.Controllers
{
    [Authorize]
    public class ProcedimientoAgendarController : Controller
    {
        private readonly ICitaServicio _citaServicio;
        private readonly IPacienteServicio _pacienteServicio;

        public ProcedimientoAgendarController(
            ICitaServicio citaServicio,
            IPacienteServicio pacienteServicio)
        {
            _citaServicio = citaServicio;
            _pacienteServicio = pacienteServicio;
        }


        [HttpGet]
        public IActionResult MisProcedimientos()
        {
            return View("~/Views/AgendarMisProcedimientos/MisProcedimientos.cshtml");
        }

        [HttpGet]
        public IActionResult AgendarProcedimiento()
        {
            return View("~/Views/AgendarMisProcedimientos/AgendarProcedimiento.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProcedimientosFijos(int? idEspecialidad)
        {
            var resultado = await _citaServicio.ObtenerProcedimientosFijosAsync(idEspecialidad);

            if (resultado.EsCorrecto)
            {
                return Json(resultado.Dato);
            }

            return Json(new List<ProcedimientoDTO>());
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerMisProcedimientos()
        {
            int idPaciente = await ObtenerIdPacienteActualAsync();

            if (idPaciente <= 0)
                return Json(new { esCorrecto = false, mensaje = "Sesión no válida o usuario no identificado." });

            var respuesta = await _citaServicio.ObtenerCitasPorPacienteAsync(idPaciente);

            if (respuesta.EsCorrecto && respuesta.Dato != null)
            {
                // Filtrar únicamente citas que tienen un procedimiento asociado
                var procedimientos = respuesta.Dato
                    .Where(c => c.IdProcedimiento.HasValue && c.IdProcedimiento.Value > 0)
                    .OrderByDescending(c => c.FechaCita)
                    .ToList();

                return Json(new { esCorrecto = true, dato = procedimientos });
            }

            return Json(new { esCorrecto = false, mensaje = respuesta.mensaje });
        }

        [HttpPost]
        public async Task<IActionResult> AgendarProcedimientoJson([FromBody] RegistrarCitaDTO dto)
        {
            if (dto == null)
                return Json(new { esCorrecto = false, mensaje = "Datos del formulario incompletos." });

            dto.IdPaciente = await ObtenerIdPacienteActualAsync();

            if (dto.IdPaciente <= 0)
                return Json(new { esCorrecto = false, mensaje = "Usuario no autenticado o sesión expirada." });

            if (string.IsNullOrWhiteSpace(dto.Motivo))
            {
                dto.Motivo = "Agendamiento de Procedimiento Médico";
            }

            ModelState.Clear();
            TryValidateModel(dto);

            if (!ModelState.IsValid)
            {
                var primerError = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();

                return Json(new { esCorrecto = false, mensaje = primerError ?? "Verifique los datos ingresados." });
            }

            // Invocación al servicio de Citas
            var resultado = await _citaServicio.RegistrarCitaAsync(dto, dto.IdPaciente);

            return Json(new
            {
                esCorrecto = resultado.EsCorrecto,
                mensaje = resultado.mensaje
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Ticket(int id)
        {
            var resultado = await _citaServicio.ObtenerTicketAsync(id);

            if (!resultado.EsCorrecto || resultado.Dato == null)
                return NotFound();

            return View("~/Views/Cita/Ticket.cshtml",resultado.Dato);
        }

        [Authorize(Roles = "Paciente,Medico,Administrador")]
        [HttpPost]
        public async Task<IActionResult> CancelarCita(int idCita)
        {
            var resultado = await _citaServicio.CancelarCitaAsync(idCita);

            return Json(new
            {
                esCorrecto = resultado.EsCorrecto,
                mensaje = resultado.mensaje
            });
        }


        private async Task<int> ObtenerIdPacienteActualAsync()
        {
            var claimPaciente = User.FindFirst("IdPaciente");
            if (claimPaciente != null && int.TryParse(claimPaciente.Value, out int idPacienteClaim))
            {
                return idPacienteClaim;
            }

            var claimUsuario = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claimUsuario != null && int.TryParse(claimUsuario.Value, out int idUsuario))
            {
                var pacienteResultado = await _pacienteServicio.GetPacienteByUsuarioIdAsync(idUsuario);
                if (pacienteResultado.EsCorrecto && pacienteResultado.Dato != null)
                {
                    return pacienteResultado.Dato.IdPaciente;
                }
            }

            return 0;
        }

    }

}