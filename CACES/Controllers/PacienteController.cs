using AutoMapper;
using CACES.BLL.DTOs.Paciente;
using CACES.BLL.DTOs.Usuario;
using CACES.BLL.Servicios.Paciente;
using CACES.BLL.Servicios.Paciente;
using CACES.BLL.Servicios.Usuario;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.HistorialMedicos;
using CACES.DAL.Repositorios.Pacientes;
using CACES.DAL.Repositorios.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CACES.Controllers
{
    public class PacienteController : Controller
    {
        private readonly IPacienteServicio _pacienteServicio;
        private readonly IPacienteRepositorio _pacienteRepositorio;
        private readonly IUsuarioService _usuarioServicio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IHistorialMedicoRepositorio _historialRepositorio;
        private readonly IMapper _mapper;


        public PacienteController(IPacienteServicio pacienteServicio, 
            IPacienteRepositorio pacienteRepositorio,
            IUsuarioService usuarioServicio,
            IUsuarioRepositorio usuarioRepositorio, 
            IHistorialMedicoRepositorio historialRepositorio,
            IMapper mapper)
        {
            _pacienteServicio = pacienteServicio;
        }

        [Authorize(Roles = "Paciente")]
        public async Task<IActionResult> Pacientes()
        {
            var pacientes = await _pacienteServicio.GetPacientesAsync();
            return View("~/Views/Pacientes/Pacientes.cshtml", pacientes);
        }

        [HttpGet]
        public IActionResult RegistroPaciente()
        {
            return View("~/Views/Pacientes/RegistroPaciente.cshtml");
        }

        [HttpPost]
        public async Task<MostrarUsuarioDTO> RegistroPaciente(RegistrarPacienteDTO dto)
        {
            //Delega la creación del usuario al servicio correspondiente.
            //Este método se encargará de encriptar la contraseña y guardar el usuario.
            var usuario = await _usuarioServicio.CrearUsuarioAsync(dto.Usuario);

            if (usuario?.Dato == null)
            {
                return null;
            }
            var usuarioCreado = usuario.Dato;
            // Crear Paciente
            var paciente = _mapper.Map<DAL.Entidades.Paciente>(dto.Usuario);
            // Crear Historial Médico
            var nuevoHistorial = _mapper.Map<HistorialMedico>(dto.Historial);

            //Asigna los IDs correspondientes para establecer las relaciones entre las entidades
            paciente.IdUsuario = usuarioCreado.idUsuario;
            paciente.HistorialMedico = nuevoHistorial;

            //crear el paciente y el historial médico en la base de datos. El repositorio se encargará de guardar ambas entidades en una sola transacción para garantizar la integridad de los datos.
            bool pacienteCreado = await _pacienteRepositorio.CreatePacienteAsync(paciente);

            if (pacienteCreado)
            {
                return usuarioCreado;
            }
            else
            {
                return null;
            }

        }

        [HttpPost]
        public async Task<IActionResult> EliminarCuentaDirecta()
        {
          
            var claimId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
            {
                return Json(new { exito = false, mensaje = "No se pudo identificar tu sesión activa." });
            }

            var resultadoService = await _usuarioService.EliminarUsuarioAsync(idUsuario);

            if (resultadoService.EsCorrecto)
            {

                if (HttpContext.RequestServices.GetService(typeof(Microsoft.AspNetCore.Authentication.IAuthenticationService)) != null)
                {
                    await Microsoft.AspNetCore.Authentication.HttpContextAuthenticationExtensions.SignOutAsync(HttpContext);
                }

                return Json(new { exito = true, mensaje = resultadoService.mensaje ?? "Tu cuenta ha sido eliminada correctamente del sistema CACES." });
            }

            return Json(new { exito = false, mensaje = resultadoService.mensaje });
        }
    }
}