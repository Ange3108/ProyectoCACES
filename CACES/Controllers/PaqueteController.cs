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
            return View("~/Views/Paquete/CrearPaquete.cshtml", new PaqueteDTO());
        }


        [HttpPost]
        public async Task<IActionResult> CrearPaquete(PaqueteDTO paqueteDTO)
        {
            if (!ModelState.IsValid)
            {
                // Si hay errores de validación, recarga la misma vista manteniendo los datos digitados
                return View("~/Views/Paquete/CrearPaquete.cshtml", paqueteDTO);
            }

            var resultado = await _paqueteServicio.CreatePaqueteAsync(paqueteDTO);

            if (!resultado.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, resultado.mensaje ?? "No se pudo crear el paquete.");
                // Si falla la lógica de negocio, recarga la vista con el mensaje de error superior
                return View("~/Views/Paquete/CrearPaquete.cshtml", paqueteDTO);
            }

            TempData["Mensaje"] = resultado.mensaje ?? "Paquete creado correctamente.";
            return RedirectToAction("ObtenerPaquetes");
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> ObtenerPaquetes()
        {
            try
            {
                var resultado = await _paqueteServicio.GetPaquetesAsync();

                if (resultado == null)
                {
                    resultado = new List<PaqueteDTO>();
                }

                return View("~/Views/Paquete/Turismo.cshtml", resultado);
            }
            catch (Exception ex)
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
                var resultado = await _paqueteServicio.GetPaquetesSoloActivosAsync();

                if (resultado == null)
                {
                    resultado = new List<PaqueteDTO>();
                }

                return View("~/Views/Paquete/Turismo.cshtml", resultado);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }


        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> ActualizarPaquete(int id)
        {
            var respuesta = await _paqueteServicio.GetPaquetePorIdAsync(id);

            if (respuesta == null)
            {
                return View("Error");
            }

            if (!respuesta.EsCorrecto)
            {
                return View("Error"); 
            }


            PaqueteDTO paqueteDto = respuesta.Dato;

            if (paqueteDto == null)
            {
                return NotFound();
            }

            return View("~/Views/Paquete/ActualizarPaquete.cshtml", paqueteDto);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> ActualizarPaquete(int id, PaqueteDTO paqueteDTO)
        {
            paqueteDTO.IdPaquete = id;

            if (!ModelState.IsValid)
            {
                return View("~/Views/Paquete/ActualizarPaquete.cshtml", paqueteDTO);
            }

            var resultado = await _paqueteServicio.UpdatePaqueteAsync(id, paqueteDTO);

            if (!resultado.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, resultado.mensaje ?? "No se pudo actualizar el paquete.");
                return View("~/Views/Paquete/ActualizarPaquete.cshtml", paqueteDTO);
            }

            TempData["Mensaje"] = resultado.mensaje ?? "Paquete actualizado correctamente.";
            return RedirectToAction("ObtenerPaquetes");
        }
    }
}
