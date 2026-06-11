using CACES.BLL.DTOs.Paciente;
using CACES.BLL.Servicios.Paciente;
using CACES.BLL.Servicios.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CACES.BLL.Servicios.Usuario;
using Microsoft.AspNetCore.Authentication;

namespace CACES.Controllers
{
 
    public class PacienteController : Controller
    {
        private readonly IPacienteServicio _pacienteServicio;
        private readonly IUsuarioService _usuarioService;


        public PacienteController(IPacienteServicio pacienteServicio,IUsuarioService usuarioService)
        {
            _pacienteServicio = pacienteServicio;
            _usuarioService = usuarioService;

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
        public IActionResult RegistrarPaciente()
        {
            return View("~/Views/Pacientes/RegistrarPaciente.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarPaciente(RegistrarPacienteDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Pacientes/RegistrarPaciente.cshtml", dto);
            }

            try
            {
                var resultado = await _pacienteServicio.RegistrarPacienteAsync(dto);

                if (!resultado)
                {
                    TempData["Error"] = "El servicio devolvió FALSE.";
                    return View("~/Views/Pacientes/RegistrarPaciente.cshtml", dto);
                }

                TempData["Mensaje"] = "Paciente registrado correctamente.";
                return RedirectToAction("Login", "Auth");
            }
            catch (Exception ex)
            {
                var error = ex;

                while (error.InnerException != null)
                {
                    error = error.InnerException;
                }

                TempData["Error"] = error.Message;

                return View("~/Views/Pacientes/RegistrarPaciente.cshtml", dto);
            }
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


            if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
            {
                return Json(new { exito = false, mensaje = "No se pudo identificar tu sesión activa." });
            }

            var resultadoService = await _usuarioService.EliminarUsuarioAsync(idUsuario);

            if (resultadoService.EsCorrecto)
            {
                await HttpContext.SignOutAsync();

                return Json(new
                {
                    exito = true,
                    mensaje = resultadoService.mensaje ?? "Tu cuenta ha sido eliminada correctamente del sistema CACES."
                });
            }

            return Json(new
            {
                exito = false,
                mensaje = resultadoService.mensaje
            });

            var resultado = await _usuarioService.EliminarUsuarioAsync(idUsuario);
            return Json(resultado);

        }
    }
}