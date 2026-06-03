using CACES.BLL.DTOs.Auth;
using CACES.BLL.DTOs.Usuario;
using CACES.BLL.Servicios.Auth;
using CACES.BLL.Servicios.Usuario;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CACES.Controllers
{
    public class Login_LogoutController : Controller
    {
        private readonly IAuthServicio _authServicio;


        public Login_LogoutController (IAuthServicio authServicio)
        {
            _authServicio = authServicio;
        }

        public async Task< IActionResult> Login(LoginDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Usuarios/Login.cshtml");
            }

            // Autenticamos y obtenemos el objeto usuario completo
            var usuario = await _authServicio.AutenticarAsync(dto);

            if (usuario != null)
            {
                // Crear los Claims de inicio de sesión
                var claims = new List<Claim> {
            new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
            new Claim(ClaimTypes.Email, usuario.CorreoElectronico),
            new Claim(ClaimTypes.Name, $"{usuario.Nombres} {usuario.PrimerApellido}")
        };

                // Puedes agregar aquí la lógica de los roles si tu contexto está accesible,
                // o si no la necesitas de inmediato, crea la identidad directamente:
                var claimsIdentity = new ClaimsIdentity(claims, Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity)
                );
                return RedirectToAction("Index", "Home");
            }

            // Si las credenciales fallan, vuelve a mostrar el login con un error
            ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
            return View("~/Views/Usuarios/Login.cshtml");


        }

        public IActionResult Logout()
        {
            // Aquí puedes agregar la lógica para cerrar la sesión del usuario, como limpiar cookies o tokens de autenticación.
            // Por ejemplo, si estás usando cookies para la autenticación, puedes eliminar la cookie de autenticación:
            Response.Cookies.Delete("NombreDeTuCookieDeAutenticacion");
            // Redirige al usuario a la página de inicio después de cerrar sesión
            return RedirectToAction("Index", "Home");
        }
    }
}
