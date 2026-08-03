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

        [HttpGet]
        public async Task<IActionResult> VerPerfil()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    if (IsAjaxRequest())
                    {
                        return Unauthorized(new { esCorrecto = false, mensaje = "Sesión no válida o expirada." });
                    }
                    return RedirectToAction("Login", "Login_Logout");
                }

                int idUsuario = int.Parse(userIdClaim.Value);
                var resultado = await _perfilServicio.GetPerfilUsuarioPorIdAsync(idUsuario);

                if (resultado == null || !resultado.EsCorrecto || resultado.Dato == null)
                {
                    string mensajeError = resultado?.mensaje ?? "No se pudo cargar el perfil.";
                    if (IsAjaxRequest())
                    {
                        return BadRequest(new { esCorrecto = false, mensaje = mensajeError });
                    }
                    TempData["Error"] = mensajeError;
                    return RedirectToAction("Index", "Home");
                }

                if (IsAjaxRequest())
                {
                    return Json(resultado.Dato);
                }

                return View("~/Views/Perfil/perfil.cshtml", resultado.Dato);
            }
            catch (Exception)
            {
                if (IsAjaxRequest())
                {
                    return StatusCode(500, new { esCorrecto = false, mensaje = "Ocurrió un error interno en el servidor." });
                }
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ActualizarPerfilObt()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    if (IsAjaxRequest())
                    {
                        return Unauthorized(new { esCorrecto = false, mensaje = "Sesión no válida o expirada." });
                    }
                    return RedirectToAction("Login", "Usuario");
                }

                int idUsuario = int.Parse(userIdClaim.Value);
                var resultado = await _perfilServicio.GetPerfilParaActualizarPorIdAsync(idUsuario);

                if (resultado == null || !resultado.EsCorrecto || resultado.Dato == null)
                {
                    string mensajeError = resultado?.mensaje ?? "No se pudieron cargar los datos para editar.";
                    if (IsAjaxRequest())
                    {
                        return BadRequest(new { esCorrecto = false, mensaje = mensajeError });
                    }
                    TempData["Error"] = mensajeError;
                    return RedirectToAction("VerPerfil");
                }

                if (IsAjaxRequest())
                {
                    return Json(resultado.Dato);
                }

                return View("~/Views/Perfil/actualizarPerfil.cshtml", resultado.Dato);
            }
            catch (Exception)
            {
                if (IsAjaxRequest())
                {
                    return StatusCode(500, new { esCorrecto = false, mensaje = "Ocurrió un error interno en el servidor." });
                }
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarPerfil(ActualizarPerfilDTO perfilDto)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                if (IsAjaxRequest())
                {
                    return BadRequest(new
                    {
                        esCorrecto = false,
                        mensaje = "Por favor completa los campos requeridos.",
                        errores = errores
                    });
                }

                return View(perfilDto);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                if (IsAjaxRequest())
                {
                    return Unauthorized(new { esCorrecto = false, mensaje = "Sesión expirada." });
                }
                return RedirectToAction("Login", "Usuario");
            }

            int idUsuario = int.Parse(userIdClaim.Value);

            if (perfilDto.IdUsuario != idUsuario)
            {
                string msjNoAuth = "Acción no autorizada.";
                if (IsAjaxRequest())
                {
                    return BadRequest(new { esCorrecto = false, mensaje = msjNoAuth });
                }
                TempData["Error"] = msjNoAuth;
                return RedirectToAction("VerPerfil");
            }

            var resultado = await _perfilServicio.ActualizarPerfilUsuarioAsync(idUsuario, perfilDto);

            if (!resultado.EsCorrecto)
            {
                string msjError = resultado.mensaje ?? "Ocurrió un error al guardar los cambios.";
                if (IsAjaxRequest())
                {
                    return BadRequest(new { esCorrecto = false, mensaje = msjError });
                }

                ModelState.AddModelError(string.Empty, msjError);
                return View(perfilDto);
            }

            if (IsAjaxRequest())
            {
                return Json(new { esCorrecto = true, mensaje = "¡Tu perfil ha sido actualizado correctamente!" });
            }

            TempData["Exito"] = "¡Tu perfil ha sido actualizado correctamente!";
            return RedirectToAction("VerPerfil");
        }

        // Función auxiliar para detectar AJAX de forma limpia
        private bool IsAjaxRequest()
        {
            return Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}