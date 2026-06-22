using CACES.BLL.DTOs.Usuario;
using CACES.BLL.Servicios.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Controllers
{

    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioServicio;

        public UsuarioController(IUsuarioService usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }


        //Mostrar vista administrador
        [Authorize(Roles = "Administrador")]
        public IActionResult Usuarios()
        {
            return View("~/Views/Usuarios/Usuarios.cshtml");
        }

        //Mostrar vista administrador
        [Authorize(Roles = "Administrador")]

        public async Task<IActionResult> ObtenerUsuarios()
        {
            var usuarios = await _usuarioServicio.GetUsuariosAsync();
            return Json(usuarios);
        }

        [HttpGet]
        public IActionResult RegistroUsuario()
        {
            return View("~/Views/Usuarios/RegistroUsuario.cshtml");
        }

        //Registro
        [HttpPost]
        public async Task<IActionResult> RegistroUsuario(RegistrarUsuarioDTO registrarUsuarioDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var UsuarioCreado = await _usuarioServicio.CrearUsuarioAsync(registrarUsuarioDTO);
            if (!UsuarioCreado.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, UsuarioCreado.mensaje);
                return BadRequest(UsuarioCreado);
            }

            // Verificar si el usuario que está operando en la sesión es Administrador
            if (User.Identity.IsAuthenticated && User.IsInRole("Administrador"))
            {
                // Redirige al área o controlador de administración
                return RedirectToAction("Usuarios", new { area = "Administrador" });
            }

            // Redirección para usuarios comunes (público general)
            return RedirectToAction("Index", "Home");

        }


        //Actualizar
        [HttpPost]
        public async Task<IActionResult> ActualizarUsuario(int id, ActualizarUsuarioDTO actualizarUsuarioDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var usuarioActualizado = await _usuarioServicio.ActualizarUsuarioAsync(id, actualizarUsuarioDTO);

            return Json(usuarioActualizado);

        }

        //Metodo Desactivar
        [HttpPost]
        public async Task<IActionResult> DesactivarUsuario(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var resultado = await _usuarioServicio.DesactivarUsuarioAsync(id);
            return Json(resultado);

        }


    }
}
