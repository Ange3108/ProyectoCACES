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


        [AllowAnonymous]
        public IActionResult Especialistas()
        {
            return View("~/Views/Medico/Especialistas.cshtml");
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult Medicos()
        {
            return View("~/Views/Medico/Medicos.cshtml");
        }
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult RegistrarEspecialista()
        {
            return View("~/Views/Medico/RegistrarEspecialista.cshtml");
        }

        // Acción para mostrar la vista de especialistas activos
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ObtenerEspecialistas()
        {
            var medicos = await _medicoServicio.GetEspecialistasActivosAsync();
            return Json(medicos);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> GetMedicos()
        {
            var resultado = await _medicoServicio.GetMedicosAsync();
            return Json(resultado);

        }


        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> CrearMedico(RegistrarMedicoDTO medicoDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
        
        var resultado = await _medicoServicio.CreateMedicoAsync(medicoDto);

            if (!resultado.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, resultado.mensaje ?? "No se pudo crear el médico.");
                return BadRequest(resultado);
            }

            return Json(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> EditarMedico(int id)
        {
            var resultado = await _medicoServicio.GetMedicoParaEditarAsync(id);

            if (!resultado.EsCorrecto)
            {
                TempData["Error"] = resultado.mensaje ?? "Médico no encontrado.";
                return RedirectToAction("Medicos");
            }

            return View("~/Views/Medico/EditarMedico.cshtml", resultado.Dato);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> EditarMedico(int id,EditarMedicoDTO dto, IFormFile? FotoArchivo)
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

                using var stream = new FileStream(rutaCompleta, FileMode.Create);
                await FotoArchivo.CopyToAsync(stream);
                dto.Foto = "/imagenes/medicos/" + nombreArchivo;
            }

            var resultado = await _medicoServicio.UpdateMedicoConUsuarioAsync(dto);

            if (!resultado.EsCorrecto)
            {
                return BadRequest(resultado);   
            }

            TempData["Exito"] = resultado.mensaje;
            return RedirectToAction(nameof(Medicos));
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> DesactivarMedico(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
        
        var resultado = await _medicoServicio.DesactivarMedicoAsync(id);
            if(!resultado.EsCorrecto) 
            {
                return NotFound(resultado);
             }
            return Json(resultado);


        }
    }
}
