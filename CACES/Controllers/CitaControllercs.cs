using CACES.BLL.Servicios.Citas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Controllers
{
    public class CitaController : Controller
    {
        private readonly ICitaServicio _citaServicio;

        public CitaController(ICitaServicio citaServicio)
        {
            _citaServicio = citaServicio;
        }

        [HttpGet]
        public async Task<IActionResult> GestionCitas()
        {
            var citas = await _citaServicio.GetCitasAsync();
            return View(citas);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarFechaCita(int idCita, DateTime nuevaFecha)
        {
            var resultado = await _citaServicio.ActualizarFechaCitaAsync(idCita, nuevaFecha);

            TempData[resultado ? "Mensaje" : "Error"] =
                resultado ? "La fecha de la cita se actualizó correctamente." : "No se pudo actualizar la fecha de la cita.";

            return RedirectToAction("GestionCitas");
        }

        [HttpPost]
        public async Task<IActionResult> CancelarCitasPorMedicoYFecha(int idMedico, DateTime fechaCita)
        {
            var resultado = await _citaServicio.CancelarCitasPorMedicoYFechaAsync(idMedico, fechaCita);

            TempData[resultado ? "Mensaje" : "Error"] =
                resultado ? "Las citas del médico fueron canceladas correctamente." : "No se encontraron citas activas para cancelar.";

            return RedirectToAction("GestionCitas");
        }
    }
}