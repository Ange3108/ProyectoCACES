using CACES.BLL.Servicios.Paciente;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CACES.BLL.DTOs.Paciente;
using CACES.BLL.Servicios.Paciente;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
                return View("~/Views/Pacientes/RegistroPaciente.cshtml", dto);

            var resultado = await _pacienteServicio.RegistrarPacienteAsync(dto);

            if (!resultado)
            {
                TempData["Error"] = "No se pudo registrar el paciente.";
                return View("~/Views/Pacientes/RegistroPaciente.cshtml", dto);
            }

            TempData["Mensaje"] = "Paciente registrado correctamente. Revise su correo para verificar la cuenta.";
            return RedirectToAction("Login", "Auth");
        }
    }
}