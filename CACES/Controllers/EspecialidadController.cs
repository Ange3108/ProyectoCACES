using CACES.BLL.DTOs.Especialidad;
using CACES.BLL.Servicios.Especialidad;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Controllers
{
    [Authorize]
    public class EspecialidadController : Controller
    {
        private readonly IEspecialidadServicio _especialidadServicio;

        public EspecialidadController(IEspecialidadServicio especialidadServicio)
        {
            _especialidadServicio = especialidadServicio;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Especialidades()
        {
            return View("~/Views/Especialidad/Especialidades.cshtml");
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult ListadoEspecialidad()
        {
            return View("~/Views/Especialidad/ListadoEspecialidad.cshtml");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Detalles(int id)
        {
            var respuesta = await _especialidadServicio.GetDetalleEspecialidadAsync(id);

            if (!respuesta.EsCorrecto || respuesta.Dato == null)
            {
                return NotFound();
            }

            return View(respuesta.Dato);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult RegistroEspecialidad()
        {
            return View("~/Views/Especialidad/RegistroEspecialidad.cshtml");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ObtenerEspecialidades()
        {
            var especialidades = await _especialidadServicio.GetEspecialidadesActivasAsync();
            return Json(especialidades);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> ObtenerListadoEspecialidades()
        {
            var especialidad = await _especialidadServicio.GetListadoEspecialidadesAsync();
            return Json(especialidad);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> RegistroEspecialidad(especialidadDTO registrarEspecialidadDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var especialidadCreada = await _especialidadServicio.CrearEspecialidadAsync(registrarEspecialidadDTO);

            if (!especialidadCreada.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, especialidadCreada.mensaje);
                return Json(especialidadCreada);
            }

            return Json(especialidadCreada);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> ActualizarEspecialidad(int id, especialidadDTO actualizarEspecialidadDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _especialidadServicio.ActualizarEspecialidadAsync(id, actualizarEspecialidadDTO);
            return Json(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> DesactivarEspecialidad(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _especialidadServicio.DesactivarEspecialidadAsync(id);
            return Json(resultado);
        }

       
    }
}