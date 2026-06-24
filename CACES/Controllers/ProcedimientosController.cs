using CACES.BLL.DTOs.Procedimientos;
using CACES.BLL.Servicios.Medicos;
using CACES.BLL.Servicios.Paciente;
using CACES.BLL.Servicios.Procedimientos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CACES.Controllers
{
    public class ProcedimientosController : Controller
    {
        private readonly IProcedimientosServicio _procedimientosServicio;
        private readonly IMedicoServicio _medicoServicio;
        private readonly IPacienteServicio _pacienteServicio;
        public ProcedimientosController(IProcedimientosServicio procedimientosServicio, IMedicoServicio medicoServicio, IPacienteServicio pacienteServicio)
        {
            _procedimientosServicio = procedimientosServicio;
            _medicoServicio = medicoServicio;
            _pacienteServicio = pacienteServicio;
        }



        [HttpGet]
        [Authorize(Roles = "Administrador,Medico,Paciente")]
        public async Task<IActionResult> ObtenerProcedimientos(int? idPaciente)
        {
            if (User.IsInRole("Paciente"))
            {
                var idUsuarioClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(idUsuarioClaim))
                {
                    return View("~/Views/Procedimiento/Quirurgicos.cshtml", new List<MostrarProcedimientosDTO>());
                }

                int idUsuarioLogueado = int.Parse(idUsuarioClaim);

                int idPacienteReal = await _pacienteServicio.ObtenerIdPacientePorUsuarioIdAsync(idUsuarioLogueado);

                if (idPacienteReal == 0) 
                {
                    ModelState.AddModelError("", "No se encontró un perfil de paciente asociado a este usuario.");
                    return View("~/Views/Procedimiento/Quirurgicos.cshtml", new List<MostrarProcedimientosDTO>());
                }

                var procedimientosPaciente = await _procedimientosServicio.ObtenerDetalleCirugiaAsync(idPacienteReal);
                return View("~/Views/Procedimiento/Quirurgicos.cshtml", procedimientosPaciente);
            }

            if (idPaciente == null || idPaciente <= 0)
            {
                var todosLosProcedimientos = await _procedimientosServicio.ObtenerTodasLasCirugiasAsync();
                return View("~/Views/Procedimiento/Quirurgicos.cshtml", todosLosProcedimientos);
            }

            var procedimientosEspecificos = await _procedimientosServicio.ObtenerDetalleCirugiaAsync(idPaciente.Value);
            return View("~/Views/Procedimiento/Quirurgicos.cshtml", procedimientosEspecificos);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int idPaciente, int id)
        {
            var modelo = await _procedimientosServicio.ObtenerPorIdParaEditarAsync(idPaciente, id);

            if (modelo == null)
            {
                return NotFound();
            }

            ViewData["IdPaciente"] = idPaciente;

            return View("~/Views/Procedimiento/EditarProcedimientos.cshtml",modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(EditarProcedimientosDTO dto, int idPaciente)
        {
            bool exito = false;

            if (ModelState.IsValid)
            {
                exito = await _procedimientosServicio.ActualizarProcedimientoAsync(dto, idPaciente);
                if (exito)
                {
                    return RedirectToAction("ObtenerProcedimientos");
                }
            }
            var datosOriginales = await _procedimientosServicio.ObtenerPorIdParaEditarAsync(idPaciente, dto.Id_Procedimiento);

            if (datosOriginales != null)
            {
                dto.Nombre = datosOriginales.Nombre;
                dto.NombreMedico = datosOriginales.NombreMedico;
                dto.NombrePaciente = datosOriginales.NombrePaciente;
                dto.PrimerApellidoPaciente = datosOriginales.PrimerApellidoPaciente;
                dto.SegundoApellidoPaciente = datosOriginales.SegundoApellidoPaciente;
            }

            ViewData["IdPaciente"] = idPaciente;

            ModelState.AddModelError("", "No se pudieron guardar los cambios. Verifique los datos ingresados.");
            return View("~/Views/Procedimiento/EditarProcedimientos.cshtml", dto);
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")] 
        public async Task<IActionResult> Registrar(int idPaciente)
        {
            if (idPaciente <= 0)
            {
                return BadRequest("El ID del paciente no es válido.");
            }

            var dto = new RegistrarProcedimientosDto
            {
                Id_Paciente = idPaciente
            };

            var medicos = await _medicoServicio.GetEspecialistasActivosAsync();
            var procedimientos = await _procedimientosServicio.ObtenerProcedimientosFijosAsync();

            ViewBag.Medicos = medicos.Dato;
            ViewBag.Procedimientos = procedimientos;

            return View("~/Views/Procedimiento/RegistrarProcedimiento.cshtml", dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Registrar(RegistrarProcedimientosDto dto)
        {
            if (!ModelState.IsValid)
            {
                var medicos = await _medicoServicio.GetEspecialistasActivosAsync();

                ViewBag.Medicos = medicos.Dato;

                ViewBag.Procedimientos = await _procedimientosServicio.ObtenerProcedimientosFijosAsync();

                return View("~/Views/Procedimiento/RegistrarProcedimiento.cshtml", dto);
            }

            bool guardadoExitoso = await _procedimientosServicio.RegistrarProcedimientoAsync(dto);

            if (guardadoExitoso)
            {
                return RedirectToAction("ObtenerProcedimientos", new { idPaciente = dto.Id_Paciente });
            }

            ModelState.AddModelError("", "No se pudo agendar el procedimiento quirúrgico. Verifique la disponibilidad del médico.");

            var medicosFallo = await _medicoServicio.GetEspecialistasActivosAsync();

            ViewBag.Medicos = medicosFallo.Dato;
            ViewBag.Procedimientos = await _procedimientosServicio.ObtenerProcedimientosFijosAsync();

            return View("~/Views/Procedimiento/RegistrarProcedimiento.cshtml", dto);
        }
    }
}
