using CACES.BLL.DTOs.Perfil;
using CACES.BLL.DTOs.Usuario;
using CACES.BLL.Servicios.Perfil;
using CACES.BLL.Servicios.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace CACES.Controllers
{
  
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioServicio;

        public UsuarioController(IUsuarioService usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
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
            var UsuarioCreado = await _usuarioServicio.CrearUsuarioAsync(registrarUsuarioDTO);
            if (!UsuarioCreado.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, UsuarioCreado.mensaje);
                return View("~/Views/Usuarios/RegistroUsuario.cshtml", registrarUsuarioDTO);
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

     /* [HttpGet]
        public IActionResult ActualizarUsuario(int id)
        {
            return View();
        }
     */
        //Actualizar
        [HttpPost]
        public async Task<IActionResult> ActualizarUsuario(int id, ActualizarUsuarioDTO actualizarUsuarioDTO)
        {
            var usuarioActualizado = await _usuarioServicio.ActualizarUsuarioAsync(id, actualizarUsuarioDTO);
            if(!usuarioActualizado.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, usuarioActualizado.mensaje);
                return View(actualizarUsuarioDTO);
            }
            return RedirectToAction("Usuarios");
         
        }

        //Metodo Eliminar
        //[Authorize(Roles = "Paciente")]
        [HttpPost]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var resultado = await _usuarioServicio.EliminarUsuarioAsync(id);
            if (resultado.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, resultado.mensaje);
                return RedirectToAction("Usuarios");
            }
            return RedirectToAction("Index", "Home");
        }

        //Mostrar vista administrador
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Usuarios()
        {
            var usuarios = await _usuarioServicio.GetUsuariosAsync();
            return View("~/Views/Usuarios/Usuarios.cshtml", usuarios.Dato);
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
        public async Task<IActionResult> ActualizarPerfil()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Usuario");
            }

            int idUsuario = int.Parse(userIdClaim.Value);

            // Busca los datos actuales del usuario
            var resultado = await _perfilServicio.GetPerfilUsuarioPorIdAsync(idUsuario);

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

            var resultado = await _perfilServicio.ActualizarPerfilUsuarioAsync(idUsuario, ActualizarperfilDto);

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
