using CACES.BLL.DTOs.Precio;
using CACES.BLL.Servicios.Precio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Controllers
{
    public class PrecioController : Controller
    {
        private readonly IPrecioServicio _precioServicio;

        public PrecioController(
            IPrecioServicio precioServicio)
        {
            _precioServicio = precioServicio;
        }

        [HttpGet]
        public IActionResult GestionPrecios()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPrecios()
        {
            var respuesta =
                await _precioServicio.ObtenerTodosAsync();

            return Json(respuesta);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPrecio(
            int idPrecio)
        {
            if (idPrecio <= 0)
            {
                return BadRequest(new
                {
                    esCorrecto = false,
                    mensaje = "El precio seleccionado no es válido."
                });
            }

            var respuesta =
                await _precioServicio.ObtenerEditarAsync(
                    idPrecio
                );

            if (!respuesta.EsCorrecto)
            {
                return BadRequest(respuesta);
            }

            return Json(respuesta);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarPrecio(
            [FromBody] EditarPrecioDTO dto)
        {
            if (dto == null)
            {
                return BadRequest(new
                {
                    esCorrecto = false,
                    mensaje = "No se recibió la información del precio."
                });
            }

            if (!ModelState.IsValid)
            {
                var mensaje = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors)
                    .Select(x => x.ErrorMessage)
                    .FirstOrDefault();

                return BadRequest(new
                {
                    esCorrecto = false,
                    mensaje = mensaje
                        ?? "Revise la información ingresada."
                });
            }

            var respuesta =
                await _precioServicio.ActualizarAsync(dto);

            if (!respuesta.EsCorrecto)
            {
                return BadRequest(respuesta);
            }

            return Json(respuesta);
        }
    }
}