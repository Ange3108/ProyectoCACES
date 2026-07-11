using CACES.BLL.DTOs.Icono;
using CACES.BLL.Servicios.Icono;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Controllers
{
    [Authorize]
    public class IconoController : Controller
    {
        private readonly IIconoServicio _iconoServicio;

        public IconoController(IIconoServicio iconoServicio)
        {
            _iconoServicio = iconoServicio;
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult ListadoIcono()
        {
            return View("~/Views/Icono/ListadoIcono.cshtml");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ObtenerIconos()
        {
            var iconos = await _iconoServicio.GetListadoIconosAsync();
            return Json(iconos);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> RegistroIcono(IconoDTO iconoDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultado = await _iconoServicio.CrearIconoAsync(iconoDTO);
            return Json(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> ActualizarIcono(int id, IconoDTO iconoDTO)
        {
            var resultado = await _iconoServicio.ActualizarIconoAsync(id, iconoDTO);
            return Json(resultado);
        }
    }
}
