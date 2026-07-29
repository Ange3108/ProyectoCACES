using System.Security.Claims;
using CACES.BLL.Servicios.Notificacion;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Web.Controllers
{
    public class NotificacionUsuarioController : Controller
    {
        private readonly INotificacionUsuarioServicio _notificacionUsuarioServicio;

        public NotificacionUsuarioController(INotificacionUsuarioServicio notificacionUsuarioServicio)
        {
            _notificacionUsuarioServicio = notificacionUsuarioServicio;
        }

        // AJUSTAR: reemplaza esto por la forma real en que tu proyecto obtiene el id del
        // usuario logueado (claim personalizado, sesión, JWT, etc.). Aquí se intenta primero
        // un claim "IdUsuario" y si no existe, el claim estándar de identidad.
        private int ObtenerIdUsuarioActual()
        {
            var claim = User.FindFirst("IdUsuario") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null && int.TryParse(claim.Value, out var id) ? id : 0;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerPendientes()
        {
            var idUsuario = ObtenerIdUsuarioActual();
            var notificaciones = await _notificacionUsuarioServicio.ObtenerPorUsuario(idUsuario);
            var conteo = await _notificacionUsuarioServicio.ContarNoLeidas(idUsuario);

            return Json(new
            {
                esCorrecto = true,
                dato = notificaciones.Dato,
                noLeidas = conteo.Dato
            });
        }

        [HttpPost]
        public async Task<IActionResult> MarcarLeida(int id)
        {
            var resultado = await _notificacionUsuarioServicio.MarcarLeida(id);
            return Json(resultado);
        }

        [HttpPost]
        public async Task<IActionResult> MarcarTodasLeidas()
        {
            var idUsuario = ObtenerIdUsuarioActual();
            var resultado = await _notificacionUsuarioServicio.MarcarTodasLeidas(idUsuario);
            return Json(resultado);
        }
    }
}
