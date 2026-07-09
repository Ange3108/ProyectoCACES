using CACES.BLL.Servicios.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Controllers
{
    public class RolController : Controller
    {
        private readonly IRolServicio _rolServicio;

        public RolController(IRolServicio rolServicio)
        {
            _rolServicio = rolServicio;
        }

        [HttpGet]
        public async Task<IActionResult> GestionRoles()
        {
            var usuarios = await _rolServicio.GetUsuariosConRolesAsync();
            var roles = await _rolServicio.GetRolesAsync();

            ViewBag.Roles = roles;

            return View(usuarios);
        }

        [HttpPost]
        public async Task<IActionResult> CambiarRol(string userId, string roleId)
        {
            var resultado = await _rolServicio.CambiarRolAsync(userId, roleId);

            if (resultado)
                TempData["Mensaje"] = "El rol del usuario se actualizó correctamente.";
            else
                TempData["Error"] = "No se pudo actualizar el rol del usuario.";

            return RedirectToAction("GestionRoles");
        }

        [HttpPost]
        public async Task<IActionResult> DesactivarMedico(string userId)
        {
            var resultado = await _rolServicio.DesactivarUsuarioPorRolAsync(userId, "Medico");

            if (resultado)
                TempData["Mensaje"] = "El usuario médico fue desactivado correctamente.";
            else
                TempData["Error"] = "No se pudo desactivar el usuario médico.";

            return RedirectToAction("GestionRoles");
        }

        [HttpPost]
        public async Task<IActionResult> DesactivarAdministrador(string userId)
        {
            var resultado = await _rolServicio.DesactivarUsuarioPorRolAsync(userId, "Administrador");

            if (resultado)
                TempData["Mensaje"] = "El usuario administrador fue desactivado correctamente.";
            else
                TempData["Error"] = "No se pudo desactivar el usuario administrador.";

            return RedirectToAction("GestionRoles");
        }
    }
}