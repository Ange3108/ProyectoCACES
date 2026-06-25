using CACES.BLL.DTOs.Auth;
using CACES.BLL.Servicios.Auth;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthServicio _authServicio;

        public AuthController(IAuthServicio authServicio)
        {
            _authServicio = authServicio;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Auth/Login.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Auth/Login.cshtml", dto);

            var resultado = await _authServicio.LoginAsync(dto);

            if (!resultado)
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
                return View("~/Views/Auth/Login.cshtml", dto);
            }

            HttpContext.Session.SetString("UsuarioCorreo", dto.CorreoElectronico);

            TempData["Mensaje"] = "Inicio de sesión exitoso.";
            return RedirectToAction("Index", "Home");
        }

    }
    }