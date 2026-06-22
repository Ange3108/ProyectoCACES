using CACES.BLL.DTOs.Especialidad;
using CACES.BLL.Servicios.Especialidad;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Controllers
{
    public class EspecialidadController : Controller
    {
        private readonly IEspecialidadServicio _especialidadServicio;

        public EspecialidadController(IEspecialidadServicio especialidadServicio)
        {
            _especialidadServicio = especialidadServicio;
        }

        [HttpGet]
        public IActionResult RegistroEspecialidad()
        {
            return View("~/Views/Especialidades/RegistroEspecialidad.cshtml");
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> RegistroEspecialidad(especialidadDTO registrarEspecialidadDTO)
        {
            var especialidadCreada = await _especialidadServicio.CrearEspecialidadAsync(registrarEspecialidadDTO);

            if (!especialidadCreada.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, especialidadCreada.mensaje);
            }

            return Json(especialidadCreada);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> ActualizarEspecialidad(int id, especialidadDTO actualizarEspecialidadDTO)
        {
            var especialidadActualizada =
                await _especialidadServicio.ActualizarEspecialidadAsync(id, actualizarEspecialidadDTO);

            return Json(especialidadActualizada);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> EliminarEspecialidad(int id)
        {
            var resultado = await _especialidadServicio.DesactivarEspecialidadAsync(id);

            return Json(resultado);
        }

        [HttpGet]
        public IActionResult Especialidades()
        {
            return View("~/Views/Especialidades/Especialidades.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerEspecialidades()
        {
            var especialidades = await _especialidadServicio.GetEspecialidadsAsync();

            return Json(especialidades);
        }

        [HttpPost]
        public async Task<IActionResult> DesactivarEspecialidad(int id)
        {
            var especialidad = await _especialidadServicio.DesactivarEspecialidadAsync(id);

            return Json(especialidad);
        }
    }
}