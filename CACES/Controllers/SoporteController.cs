using CACES.BLL.Servicios.Soportes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CACES.Controllers
{
    public class SoporteController : Controller
    {
        private readonly ISoporteServicio _soporteServicio;

        public SoporteController(ISoporteServicio soporteServicio)
        {
            _soporteServicio = soporteServicio;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Contacto()
        {
            return View("~/Views/Soporte/Contacto.cshtml");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Contacto(string asunto, string mensaje)
        {
            var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
                return Unauthorized();

            var resultado = await _soporteServicio.CrearConsultaAsync(idUsuario, asunto, mensaje);

            if (!resultado)
            {
                TempData["Error"] = "No se pudo enviar la consulta. Verifique la información.";
                return RedirectToAction("Contacto");
            }

            TempData["Mensaje"] = "Consulta enviada correctamente.";
            return RedirectToAction("Contacto");
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> GestionConsultas()
        {
            var consultas = await _soporteServicio.GetConsultasAsync();

            return View("~/Views/Soporte/GestionConsultas.cshtml", consultas);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> DetalleConsulta(int id)
        {
            var consulta = await _soporteServicio.GetConsultaByIdAsync(id);

            if (consulta == null)
                return NotFound();

            return View("~/Views/Soporte/DetalleConsulta.cshtml", consulta);
        }
    }
}