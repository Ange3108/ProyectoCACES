using CACES.BLL.DTOs.Paciente;
using CACES.BLL.Servicios.Paciente;
using CACES.BLL.Servicios.Usuario;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Controllers
{
    public class PacienteController : Controller
    {
        private readonly IPacienteServicio _pacienteServicio;
        private readonly IUsuarioService _usuarioService;

        public PacienteController(
            IPacienteServicio pacienteServicio,
            IUsuarioService usuarioService)
        {
            _pacienteServicio = pacienteServicio;
            _usuarioService = usuarioService;
        }

        // Solo renderiza la vista vacía; la tabla se llena con ObtenerPacientes vía AJAX
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult Pacientes()
        {
            return View("~/Views/Pacientes/Pacientes.cshtml");
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> ObtenerPacientes()
        {
            var resultado = await _pacienteServicio.GetPacientesAsync();
            return Json(resultado);
        }

        [HttpGet]
        public IActionResult RegistrarPaciente()
        {
            return View("~/Views/Pacientes/RegistroPaciente.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarPaciente(RegistrarPacienteDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Pacientes/RegistroPaciente.cshtml", dto);
            }

            var resultado = await _pacienteServicio.RegistrarPacienteAsync(dto);

            if (!resultado.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, resultado.mensaje); 
                return View("~/Views/Pacientes/RegistroPaciente.cshtml", dto);
            }

            return RedirectToAction("Login", "Login_Logout");
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> DesactivarPaciente(int id)
        {
            var resultado = await _pacienteServicio.DesactivarPacienteAsync(id);

            if (!resultado.EsCorrecto)
            {
                return NotFound(resultado);
            }

            return Json(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> ActivarPaciente(int id)
        {
            var resultado = await _pacienteServicio.ActivarPacienteAsync(id);

            if (!resultado.EsCorrecto)
            {
                return NotFound(resultado);
            }

            return Json(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarCuentaDirecta()
        {
            var claimId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
            {
                return Json(new
                {
                    EsCorrecto = false,
                    mensaje = "No se pudo identificar tu sesión activa.",
                    codigo = 400
                });
            }

            var resultado = await _usuarioService.DesactivarUsuarioAsync(idUsuario);

            if (resultado.EsCorrecto)
            {
                await HttpContext.SignOutAsync();
            }

            return Json(resultado);
        }
    }
}