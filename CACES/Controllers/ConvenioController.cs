using CACES.BLL.DTOs.Convenios;
using CACES.BLL.Servicios.Convenios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Controllers
{
    public class ConvenioController : Controller
    {
        private readonly IConvenioServicio _convenioServicio;

        public ConvenioController(IConvenioServicio convenioServicio)
        {
            _convenioServicio = convenioServicio;
        }

        [HttpGet]
        public IActionResult CrearConvenio()
        {
            return View(new CrearModificarConvenio());
        }

        [HttpPost]
        public async Task<IActionResult> CrearConvenio([FromBody] CrearModificarConvenio dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { esCorrecto = false, mensaje = "Datos del formulario no válidos." });
            }

            var resultado = await _convenioServicio.CreateConvenioAsync(dto);

            if (!resultado)
            {
                return BadRequest(new { esCorrecto = false, mensaje = "Ocurrió un error al registrar el convenio." });
            }

            return Json(new { esCorrecto = true, mensaje = "Convenio creado exitosamente." });
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> ObtenerConvenios()
        {
            try
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var datos = await _convenioServicio.GetConveniosAsync() ?? new List<MostrarConvenios>();
                    return Json(datos);
                }

                return View("GestionConvenios");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ObtenerConveniosSoloActivos()
        {
            try
            {
                var resultado = await _convenioServicio.GetConveniosSoloActivosAsync() ?? new List<MostrarConvenios>();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(resultado);
                }

                return View("Convenios", resultado);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> ActualizarConvenio(int id)
        {
            var respuesta = await _convenioServicio.GetConvenioByIdAsync(id);

            if (respuesta == null)
            {
                return View("Error");
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(respuesta);
            }
            ViewBag.IdConvenio = id;
            return View("EditarConvenio", respuesta);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> ActualizarConvenio(int id, [FromBody] CrearModificarConvenio dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { esCorrecto = false, mensaje = "Datos no válidos." });
            }

            var resultado = await _convenioServicio.UpdateConvenioAsync(id, dto);

            if (!resultado)
            {
                return BadRequest(new { esCorrecto = false, mensaje = "No se pudo actualizar el convenio." });
            }

            return Json(new { esCorrecto = true, mensaje = "Convenio actualizado correctamente." });
        }
    }
}
