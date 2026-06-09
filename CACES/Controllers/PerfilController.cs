using AutoMapper;
using CACES.BLL.DTOs.Perfil;
using CACES.BLL.Servicios.Perfil;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CACES.Controllers
{
    public class PerfilController : Controller
    {
        private readonly IPerfilServicio _perfilServicio;
        public PerfilController(IPerfilServicio perfilServicio)
        {
            _perfilServicio = perfilServicio;
        }


        //Perfil de usuario
        [HttpGet]
        public async Task<IActionResult> VerPerfil()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Login_Logout");
            }

            int idUsuario = int.Parse(userIdClaim.Value);

            var resultado = await _perfilServicio.GetPerfilUsuarioPorIdAsync(idUsuario);

            if (!resultado.EsCorrecto || resultado.Dato == null)
            {
                TempData["Error"] = resultado.mensaje ?? "No se pudo cargar el perfil.";
                return RedirectToAction("Index", "Home");
            }

            return View("~/Views/Perfil/perfil.cshtml", resultado.Dato);
        }

        [HttpGet]
        public async Task<IActionResult> ActualizarPerfilObt()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            int idUsuario = int.Parse(userIdClaim.Value);

            // Busca los datos actuales del usuario
            var resultado = await _perfilServicio.GetPerfilParaActualizarPorIdAsync(idUsuario);

            if (!resultado.EsCorrecto || resultado.Dato == null)
            {
                TempData["Error"] = resultado.mensaje ?? "No se pudieron cargar los datos para editar.";
                return RedirectToAction("VerPerfil");
            }

            // Envia el PerfilUsuarioDTO a la vista para rellenar los inputs del HTML
            return View("~/Views/Perfil/actualizarPerfil.cshtml", resultado.Dato);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarPerfil(ActualizarPerfilDTO perfilDto)
        {
            if (!ModelState.IsValid)
            {
                return View(perfilDto);
            }

            // 2. Extraemos el ID del usuario en sesión
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            int idUsuario = int.Parse(userIdClaim.Value);


            if (perfilDto.IdUsuario != idUsuario)
            {
                TempData["Error"] = "Acción no autorizada.";
                return RedirectToAction("VerPerfil");
            }

            var resultado = await _perfilServicio.ActualizarPerfilUsuarioAsync(idUsuario, perfilDto);

            if (resultado.EsCorrecto)
            {
                TempData["Exito"] = "¡Tu perfil ha sido actualizado correctamente!";
                return RedirectToAction("VerPerfil");
            }

            ModelState.AddModelError(string.Empty, resultado.mensaje ?? "Ocurrió un error al guardar los cambios.");

            return View(perfilDto);
        }

    }
}
