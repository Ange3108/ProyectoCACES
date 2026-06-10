using CACES.BLL.DTOs.Auth;
using CACES.BLL.Servicios.Auth;
using CACES.BLL.Servicios.Usuario;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CACES.Controllers
{
    public class Login_LogoutController : Controller
    {
        private readonly IAuthServicio _authServicio;

        public Login_LogoutController(IAuthServicio authServicio)
        {
            _authServicio = authServicio;
        }

     
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Usuarios/Login.cshtml");
            }

            var usuario = await _authServicio.AutenticarAsync(dto);

            if (usuario != null)
            {
                var claims = new List<Claim> {
            new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new Claim(ClaimTypes.Email, usuario.CorreoElectronico),
            new Claim(ClaimTypes.Name, $"{usuario.Nombres} {usuario.PrimerApellido}")
        };

                foreach (var UsuarioRoles in usuario.UsuarioRoles)
                {
                    claims.Add(
                        new Claim(
                            ClaimTypes.Role,
                            UsuarioRoles.Rol.Name
                        )
                    );
                }
                    var claimsIdentity = new ClaimsIdentity(claims, Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity)
                );
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
            return View("~/Views/Usuarios/Login.cshtml");


        }

        [HttpGet]
        public IActionResult OlvidoContrasena()
        {
            return View("~/Views/Usuarios/OlvidoContrasena.cshtml");
        }

        // 1. Paso uno: Verifica el correo y genera el token internamente
        [HttpPost]
        public async Task<IActionResult> VerificarCorreoDirecto(string correo)
        {
            if (string.IsNullOrEmpty(correo))
                return Json(new { exito = false, mensaje = "El correo es obligatorio." });

            var dto = new OlvidoContrasenaDTO { CorreoElectronico = correo };
            var resultado = await _authServicio.GenerarTokenRecuperacionAsync(dto);

            // Tu servicio devuelve Exito = true si el usuario existe y guarda el token en SecurityStamp
            if (resultado.Exito && !string.IsNullOrEmpty(resultado.Token))
            {
                return Json(new { exito = true, token = resultado.Token, correo = correo });
            }

            return Json(new { exito = false, mensaje = "El correo ingresado no pertenece a una cuenta activa." });
        }

        // 2. Paso dos: Cambia la contraseña usando el token obtenido en el paso 1
        [HttpPost]
        public async Task<IActionResult> RestablecerDirecto(RestablecerContrasenaDTO modelo)
        {
            if (!ModelState.IsValid)
                return Json(new { exito = false, mensaje = "Datos inválidos." });

            var resultado = await _authServicio.RestablecerContraseñaAsync(modelo);

            if (resultado.Exito)
                return Json(new { exito = true, mensaje = "Tu contraseña ha sido cambiada con éxito." });

            return Json(new { exito = false, mensaje = resultado.Mensaje });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
