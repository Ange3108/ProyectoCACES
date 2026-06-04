using CACES.BLL.DTOs.Auth;
using CACES.BLL.Servicios.Auth;
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

        [HttpPost] 
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
