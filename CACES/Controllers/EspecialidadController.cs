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
        //Registro
        [HttpPost]
        public async Task<IActionResult> RegistroEspecialidad(especialidadDTO registrarEspecialidadDTO)
        {
            var EspecialidadCreado = await _especialidadServicio.CrearEspecialidadAsync(registrarEspecialidadDTO);
           
                ModelState.AddModelError(string.Empty, EspecialidadCreado.mensaje);
                return Json(EspecialidadCreado);
        }


        [Authorize(Roles = "Administrador")]
        //Actualizar
        [HttpPost]
        public async Task<IActionResult> ActualizarEspecialidad(int id, especialidadDTO actualizarEspecialidadDTO)
        {

            var especialidadActualizado = await _especialidadServicio.ActualizarEspecialidadAsync(id, actualizarEspecialidadDTO);

            return Json(especialidadActualizado);

        }


        public IActionResult Especialidades()
        {
            return View("~/Views/Especialidades/Especialidades.cshtml");
        }


        public async Task<IActionResult> ObtenerEspecialidades()
        {
            var especialidades = await _especialidadServicio.GetEspecialidadesAsync();
            return Json(especialidades);
        }


        public async Task <IActionResult> DesactivarEspecialidadAsync(int id)
        {
            var especialidad = await _especialidadServicio.DesactivarEspecialidadAsync(id);
            return Json(especialidad);
    }
}
}
