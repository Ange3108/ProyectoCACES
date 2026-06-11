using CACES.BLL.DTOs.Paciente;
using CACES.BLL.Servicios.Paciente;
using CACES.BLL.Servicios.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Controllers
{
 
    public class PacienteController : Controller
    {
        private readonly IPacienteServicio _pacienteServicio;
        private readonly IUsuarioService _usuarioService;

        public PacienteController(IPacienteServicio pacienteServicio, IUsuarioService usuarioServicio)
        {
            _pacienteServicio = pacienteServicio;
            _usuarioService = usuarioServicio;
        }

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
            int.TryParse(claimId, out int idUsuario);

            var resultado = await _usuarioService.EliminarUsuarioAsync(idUsuario);
            return Json(resultado);
        }
    }
}