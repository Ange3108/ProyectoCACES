using CACES.BLL.DTOs.Notificacion;
using CACES.BLL.Servicios.Notificacion;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Web.Controllers
{
    public class NotificacionController : Controller
    {
        private readonly INotificacionServicio _notificacionServicio;

        public NotificacionController(INotificacionServicio notificacionServicio)
        {
            _notificacionServicio = notificacionServicio;
        }

        public IActionResult Notificacion()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var resultado = await _notificacionServicio.ObtenerTodos();
            return Json(resultado);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var resultado = await _notificacionServicio.ObtenerPorId(id);
            return Json(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> CrearNotificacion([FromBody] NotificacionDTO dto)
        {
            var resultado = await _notificacionServicio.Crear(dto);
            return Json(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarNotificacion([FromBody] NotificacionDTO dto)
        {
            var resultado = await _notificacionServicio.Actualizar(dto);
            return Json(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var resultado = await _notificacionServicio.CambiarEstado(id);
            return Json(resultado);
        }
    }
}
