using CACES.BLL.DTOs.Paciente;
using CACES.BLL.Servicios.Paciente;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Controllers
{
    public class PacienteController : Controller
    {
        private readonly IPacienteServicio _pacienteServicio;

        public PacienteController(IPacienteServicio pacienteServicio)
        {
            _pacienteServicio = pacienteServicio;
        }

        [Authorize(Roles = "Paciente")]
        public async Task<IActionResult> Pacientes()
        {
            var pacientes = await _pacienteServicio.GetPacientesAsync();
            return View("~/Views/Pacientes/Pacientes.cshtml", pacientes);
        }

        [HttpGet]
        public IActionResult RegistroPaciente()
        {
            return View("~/Views/Pacientes/RegistroPaciente.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> RegistroPaciente(RegistrarPacienteDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Pacientes/RegistroPaciente.cshtml", dto);
            }

            var resultado = await _pacienteServicio.RegistrarPacienteAsync(dto);

            if (!resultado)
            {
                TempData["Error"] = "No se pudo registrar el paciente.";
                return View("~/Views/Pacientes/RegistroPaciente.cshtml", dto);
            }

            TempData["Mensaje"] = "Paciente registrado correctamente.";
            return RedirectToAction("Login", "Auth");
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> DesactivarPaciente(int id)
        {
            var resultado = await _pacienteServicio.DesactivarPacienteAsync(id);

            if (!resultado)
            {
                TempData["Error"] = "No se pudo desactivar la cuenta del paciente.";
            }
            else
            {
                TempData["Mensaje"] = "La cuenta del paciente fue desactivada correctamente.";
            }

            return RedirectToAction("Pacientes");
        }

        [HttpPost]
        public async Task<IActionResult> EliminarCuentaDirecta()
        {
          
            var claimId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
            {
                return Json(new { exito = false, mensaje = "No se pudo identificar tu sesión activa." });
            }

            var resultadoService = await _usuarioService.EliminarUsuarioAsync(idUsuario);

            if (resultadoService.EsCorrecto)
            {

                if (HttpContext.RequestServices.GetService(typeof(Microsoft.AspNetCore.Authentication.IAuthenticationService)) != null)
                {
                    await Microsoft.AspNetCore.Authentication.HttpContextAuthenticationExtensions.SignOutAsync(HttpContext);
                }

                return Json(new { exito = true, mensaje = resultadoService.mensaje ?? "Tu cuenta ha sido eliminada correctamente del sistema CACES." });
            }

            return Json(new { exito = false, mensaje = resultadoService.mensaje });
        }
    }
}