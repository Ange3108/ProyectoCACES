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
    }
}
