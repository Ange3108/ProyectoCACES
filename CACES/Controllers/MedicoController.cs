using CACES.BLL.DTOs.Medico;
using CACES.BLL.Servicios.Medicos;
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

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Medicos()
        {
            var medicos = await _medicoServicio.GetMedicosAsync();
            return View(medicos);
        }

        [HttpGet]
        public async Task<IActionResult> Especialistas()
        {
            var medicos = await _medicoServicio.GetMedicosAsync();
            return View(medicos);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> CrearMedico(RegistrarMedicoDTO medicoDto)
        {
            if (!ModelState.IsValid)
                return View(medicoDto);

            var resultado = await _medicoServicio.CreateMedicoAsync(medicoDto);

            if (!resultado.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, resultado.mensaje ?? "No se pudo crear el médico.");
                return View(medicoDto);
            }

            TempData["Mensaje"] = resultado.mensaje ?? "Médico creado correctamente.";
            return RedirectToAction("Medicos");
        }

        [Authorize(Roles = "Administrador")]
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

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> EditarMedico(EditarMedicoDTO dto, IFormFile? FotoArchivo)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Debe completar todos los campos obligatorios.";
                return View(dto);
            }

            if (FotoArchivo != null && FotoArchivo.Length > 0)
            {
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes", "medicos");

                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                var nombreArchivo = Guid.NewGuid() + Path.GetExtension(FotoArchivo.FileName);
                var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await FotoArchivo.CopyToAsync(stream);
                }

                dto.Foto = "/imagenes/medicos/" + nombreArchivo;
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
