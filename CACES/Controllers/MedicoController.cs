using CACES.BLL.Servicios.Medicos;
using CACES.DAL.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Controllers
{
    public class MedicoController : Controller
    {
        private readonly IMedicoServicio _medicoServicio;


        public MedicoController(IMedicoServicio medicoServicio)
        {
            _medicoServicio = medicoServicio;
        }


        [HttpGet]
        public async Task<IActionResult> Medicos()
        {
            var medicos = await _medicoServicio.GetMedicosAsync();
            return View(medicos);
        }

        [Authorize(Roles = "Paciente")]
        [HttpPost]
        public async Task<IActionResult> CrearMedico(Medico medico)
        {
            if (!ModelState.IsValid)
                return View(medico);
            var resultado = await _medicoServicio.CreateMedicoAsync(medico);
            if (!resultado)
            {
                TempData["Error"] = "No se pudo crear el médico.";
                return View(medico);
            }
            TempData["Mensaje"] = "Médico creado correctamente.";
            return RedirectToAction("Medicos");
        }
        [HttpGet]
        public async Task<IActionResult> EditarMedico(int id)
        {
            var medico = await _medicoServicio.GetMedicoByIdAsync(id);

            if (medico == null)
            {
                TempData["Error"] = "Médico no encontrado.";
                return RedirectToAction("Medicos");
            }

            return View(medico);
        }

        [HttpPut]
        public async Task<IActionResult> EditarMedico(Medico medico)
        {
            if (!ModelState.IsValid)
                return View(medico);

            var resultado = await _medicoServicio.UpdateMedicoAsync(medico);

            if (!resultado)
            {
                TempData["Error"] = "No se pudo actualizar el médico.";
                return View(medico);
            }

            TempData["Mensaje"] = "Médico actualizado correctamente.";
            return RedirectToAction("Medicos");
        }
    }
}