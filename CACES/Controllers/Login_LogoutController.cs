using CACES.BLL.DTOs.Auth;
using CACES.BLL.Servicios.Auth;
using CACES.DAL.DBContext;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CACES.Controllers
{
    public class Login_LogoutController : Controller
    {
        private readonly IAuthServicio _authServicio;
        private readonly CACESDbContext _context;

        public Login_LogoutController(
            IAuthServicio authServicio,
            CACESDbContext context)
        {
            _authServicio = authServicio;
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Usuarios/Login.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Usuarios/Login.cshtml", dto);
            }

            var usuario = await _authServicio.AutenticarAsync(dto);

            if (usuario != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Email, usuario.CorreoElectronico),
                    new Claim(ClaimTypes.Name, $"{usuario.Nombres} {usuario.PrimerApellido}")
                };

                var aspUser = await _context.AspNetUsers
                    .FirstOrDefaultAsync(x => x.Email == usuario.CorreoElectronico);

                if (aspUser != null)
                {
                    var roles = await (
                        from ur in _context.AspNetUserRoles
                        join r in _context.AspNetRoles
                            on ur.RoleId equals r.Id
                        where ur.UserId == aspUser.Id
                        select r.Name
                    ).ToListAsync();

                    foreach (var rol in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, rol));
                    }
                }

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                );

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity)
                );

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
            return View("~/Views/Usuarios/Login.cshtml", dto);
        }

        [HttpGet]
        public IActionResult OlvidoContrasena()
        {
            return View("~/Views/Usuarios/OlvidoContrasena.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> VerificarCorreoDirecto(string correo)
        {
            if (string.IsNullOrEmpty(correo))
                return Json(new { exito = false, mensaje = "El correo es obligatorio." });

            var dto = new OlvidoContrasenaDTO { CorreoElectronico = correo };
            var resultado = await _authServicio.GenerarTokenRecuperacionAsync(dto);

            if (resultado.Exito && !string.IsNullOrEmpty(resultado.Token))
            {
                return Json(new { exito = true, token = resultado.Token, correo = correo });
            }

            return Json(new { exito = false, mensaje = "El correo ingresado no pertenece a una cuenta activa." });
        }

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