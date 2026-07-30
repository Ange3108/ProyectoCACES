using CACES.BLL.DTOs.Configuracion;
using CACES.BLL.Servicios.Configuracion;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Web.Controllers
{
    public class ConfiguracionController : Controller
    {
        private readonly IConfiguracionServicio _configuracionServicio;

        public ConfiguracionController(IConfiguracionServicio configuracionServicio)
        {
            _configuracionServicio = configuracionServicio;
        }

        public IActionResult Correo()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var resultado = await _configuracionServicio.ObtenerTodos();
            return Json(resultado);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorCategoria(string categoria)
        {
            var resultado = await _configuracionServicio.ObtenerPorCategoria(categoria);
            return Json(resultado);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var resultado = await _configuracionServicio.ObtenerPorId(id);
            return Json(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> CrearConfiguracion([FromBody] ConfiguracionDTO dto)
        {
            var resultado = await _configuracionServicio.Crear(dto);
            return Json(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarConfiguracion([FromBody] ConfiguracionDTO dto)
        {
            var resultado = await _configuracionServicio.Actualizar(dto);
            return Json(resultado);
        }
    }
}
