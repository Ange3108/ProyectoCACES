using CACES.BLL.DTOs.Paquete;
using CACES.BLL.Servicios.Paquete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Controllers
{
    public class PaqueteController : Controller
    {
        public readonly IPaqueteServicio _paqueteServicio;

        public PaqueteController(IPaqueteServicio paqueteServicio)
        {
            _paqueteServicio = paqueteServicio;
        }

        [HttpGet]
        public IActionResult CrearPaquete()
        {
            return View(new PaqueteDTO());
        }

        [HttpPost]
        public async Task<IActionResult> CrearPaquete([FromBody] PaqueteDTO paqueteDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { esCorrecto = false, mensaje = "Datos del formulario no válidos." });
            }

            var resultado = await _paqueteServicio.CreatePaqueteAsync(paqueteDTO);

            if (!resultado.EsCorrecto)
            {
                return BadRequest(resultado);
            }

            return Json(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> ObtenerPaquetes()
        {
            try
            {
                var resultado = await _paqueteServicio.GetPaquetesAsync() ?? new List<PaqueteDTO>();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(resultado);
                }

                return View("Turismo", resultado);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ObtenerPaquetesSoloActivos()
        {
            try
            {
                var resultado = await _paqueteServicio.GetPaquetesSoloActivosAsync() ?? new List<PaqueteDTO>();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(resultado);
                }

                return View("Turismo", resultado);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> ActualizarPaquete(int id)
        {
            var respuesta = await _paqueteServicio.GetPaquetePorIdAsync(id);

            if (respuesta == null || !respuesta.EsCorrecto || respuesta.Dato == null)
            {
                return View("Error");
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(respuesta.Dato);
            }

            return View(respuesta.Dato);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> ActualizarPaquete(int id, [FromBody] PaqueteDTO paqueteDTO)
        {
            paqueteDTO.IdPaquete = id;

            if (!ModelState.IsValid)
            {
                return BadRequest(new { esCorrecto = false, mensaje = "Datos no válidos." });
            }

            var resultado = await _paqueteServicio.UpdatePaqueteAsync(id, paqueteDTO);

            if (!resultado.EsCorrecto)
            {
                return BadRequest(resultado);
            }

            return Json(resultado);
        }
    }
}