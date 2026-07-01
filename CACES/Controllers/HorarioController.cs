using CACES.BLL.DTOs.Horario;
using CACES.BLL.Servicios.Horario;
using CACES.BLL.Servicios.Medicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Controllers
{
    public class HorarioController : Controller
    {
        private readonly IHorarioServicio _horarioServicio;
        private readonly IMedicoServicio _medicoServicio;

        public HorarioController(IHorarioServicio horarioServicio, IMedicoServicio medicoServicio)
        {
            _horarioServicio = horarioServicio;
            _medicoServicio = medicoServicio;
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> GestionHorarios()
        {
            var resultado = await _medicoServicio.GetMedicosAsync();
            ViewBag.Medicos = resultado.Dato;
            return View("~/Views/Horario/GestionHorarios.cshtml");
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerHorariosPorMedico(int idMedico)
        {
            var resultado = await _horarioServicio.ObtenerHorarioPorMedicoIdAsync(idMedico);
            return Json(resultado);
        }

        [HttpPost]
        public async Task<ActionResult> CrearHorario([FromBody] RegistrarHorarioDTO horario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var resultado = await _horarioServicio.CrearHorarioAsync(horario);
            if (!resultado.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, resultado.mensaje ?? "No se pudo crear el horario.");
                return BadRequest(resultado);
            }

            return Json(resultado);
        }
        [HttpPost]
        public async Task<ActionResult> ActualizarHorario(int id, [FromBody] EditarHorarioDTO horario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var resultado = await _horarioServicio.ActualizarHorarioAsync(id, horario);
            if (!resultado.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, resultado.mensaje ?? "No se pudo actualizar el horario.");
                return BadRequest(resultado);
            }
            return Json(resultado);
        }

        [HttpPost]
        public async Task<ActionResult> DesactivarHorario(int id)
        {
           
            var resultado = await _horarioServicio.DesactivarHorarioAsync(id);
            if (!resultado.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, resultado.mensaje ?? "No se pudo desactivar el horario.");
                return BadRequest(resultado);
            }
            return Json(resultado);
        }
    }
}
