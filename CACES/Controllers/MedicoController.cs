using CACES.BLL.DTOs.Medico;
using CACES.BLL.Servicios.Medicos;
<<<<<<< HEAD
=======
using CACES.DAL.Entidades;
using Microsoft.AspNetCore.Authorization;
>>>>>>> 78e43479a88e6319ed1a30dc30587c1a8375219d
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
            var medico = await _medicoServicio.GetMedicoParaEditarAsync(id);

            if (medico == null)
            {
                TempData["Error"] = "Médico no encontrado.";
                return RedirectToAction("Medicos");
            }

            return View(medico);
        }

<<<<<<< HEAD
        [HttpPost]
        public async Task<IActionResult> EditarMedico(EditarMedicoDTO dto)
=======
        [HttpPut]
        public async Task<IActionResult> EditarMedico(Medico medico)
>>>>>>> 78e43479a88e6319ed1a30dc30587c1a8375219d
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Debe completar todos los campos obligatorios.";
                return View(dto);
            }

            var resultado = await _medicoServicio.UpdateMedicoConUsuarioAsync(dto);

            if (!resultado)
            {
                TempData["Error"] = "No se pudo actualizar la información del médico.";
                return View(dto);
            }

            TempData["Mensaje"] = "La información del médico se actualizó correctamente.";
            return RedirectToAction("Medicos");
        }
    }
}