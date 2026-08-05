using CACES.BLL.DTOs.Precio;
using CACES.BLL.Servicios.Precio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class PrecioController : Controller
    {
        private readonly IPrecioServicio _precioServicio;

        public PrecioController(IPrecioServicio precioServicio)
        {
            _precioServicio = precioServicio;
        }

        public IActionResult GestionPrecios()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPrecios()
        {
            var respuesta = await _precioServicio.ObtenerTodosAsync();
            return Json(respuesta);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPrecio(int idPrecio)
        {
            var respuesta = await _precioServicio.ObtenerEditarAsync(idPrecio);
            return Json(respuesta);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarPrecio([FromBody] EditarPrecioDTO dto)
        {
            var respuesta = await _precioServicio.ActualizarAsync(dto);
            return Json(respuesta);
        }
    }
}
