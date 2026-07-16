using CACES.BLL.DTOs.Receta;
using CACES.BLL.Servicios.Paciente;
using CACES.BLL.Servicios.Recetas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CACES.Controllers
{
    public class RecetaController : Controller
    {
        private readonly IRecetaServicio _recetaServicio;
        private readonly IPacienteServicio _pacienteServicio;

        public RecetaController(
            IRecetaServicio recetaServicio,
            IPacienteServicio pacienteServicio)
        {
            _recetaServicio = recetaServicio;
            _pacienteServicio = pacienteServicio;
        }

        // =====================================================
        // VISTAS
        // =====================================================

        [Authorize(Roles = "Paciente")]
        [HttpGet]
        public IActionResult MisRecetas()
        {
            return View();
        }

        [Authorize(Roles = "Medico")]
        [HttpGet]
        public IActionResult GestionRecetas()
        {
            return View();
        }

        [Authorize(Roles = "Medico")]
        [HttpGet]
        public IActionResult Registrar(int idCita)
        {
            if (idCita <= 0)
                return BadRequest();

            ViewBag.IdCita = idCita;

            return View();
        }

        [Authorize(Roles = "Paciente,Medico,Admin")]
        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            if (id <= 0)
                return BadRequest();

            var resultado = await _recetaServicio.ObtenerPorIdAsync(id);

            if (!resultado.EsCorrecto || resultado.Dato == null)
                return NotFound();

            return View(resultado.Dato);
        }

        // =====================================================
        // JSON PARA REGISTRAR Y ACTUALIZAR
        // =====================================================

        [Authorize(Roles = "Medico")]
        [HttpPost]
        public async Task<IActionResult> RegistrarJson(
            [FromBody] RegistrarRecetaDTO dto)
        {
            if (dto == null)
            {
                return BadRequest(new
                {
                    esCorrecto = false,
                    mensaje = "Los datos de la receta son requeridos."
                });
            }

            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values
                    .SelectMany(valor => valor.Errors)
                    .Select(error => error.ErrorMessage)
                    .ToList();

                return BadRequest(new
                {
                    esCorrecto = false,
                    mensaje = errores.FirstOrDefault()
                              ?? "Los datos ingresados no son válidos.",
                    errores
                });
            }

            var resultado = await _recetaServicio.RegistrarAsync(dto);

            return Json(resultado);
        }

        [Authorize(Roles = "Medico")]
        [HttpPost]
        public async Task<IActionResult> ActualizarJson(
            [FromBody] RegistrarRecetaDTO dto)
        {
            if (dto == null)
            {
                return BadRequest(new
                {
                    esCorrecto = false,
                    mensaje = "Los datos de la receta son requeridos."
                });
            }

            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values
                    .SelectMany(valor => valor.Errors)
                    .Select(error => error.ErrorMessage)
                    .ToList();

                return BadRequest(new
                {
                    esCorrecto = false,
                    mensaje = errores.FirstOrDefault()
                              ?? "Los datos ingresados no son válidos.",
                    errores
                });
            }

            var resultado = await _recetaServicio.ActualizarAsync(dto);

            return Json(resultado);
        }

        // =====================================================
        // CONSULTAS JSON
        // =====================================================

        [Authorize(Roles = "Paciente,Medico,Admin")]
        [HttpGet]
        public async Task<IActionResult> ObtenerPorId(int idReceta)
        {
            if (idReceta <= 0)
            {
                return BadRequest(new
                {
                    esCorrecto = false,
                    mensaje = "La receta no es válida."
                });
            }

            var resultado =
                await _recetaServicio.ObtenerPorIdAsync(idReceta);

            return Json(resultado);
        }

        [Authorize(Roles = "Paciente,Medico,Admin")]
        [HttpGet]
        public async Task<IActionResult> ObtenerPorCita(int idCita)
        {
            if (idCita <= 0)
            {
                return BadRequest(new
                {
                    esCorrecto = false,
                    mensaje = "La cita no es válida."
                });
            }

            var resultado =
                await _recetaServicio.ObtenerPorCitaAsync(idCita);

            return Json(resultado);
        }

        [Authorize(Roles = "Paciente")]
        [HttpGet]
        public async Task<IActionResult> ObtenerMisRecetas()
        {
            var idUsuario = ObtenerIdUsuarioActual();

            if (idUsuario == null)
            {
                return Unauthorized(new
                {
                    esCorrecto = false,
                    mensaje = "No se pudo identificar al usuario."
                });
            }

            var paciente =
                await _pacienteServicio.GetPacienteByUsuarioIdAsync(
                    idUsuario.Value
                );

            if (paciente == null)
            {
                return NotFound(new
                {
                    esCorrecto = false,
                    mensaje = "No se encontró el paciente asociado."
                });
            }

            var resultado =
                await _recetaServicio.ObtenerPorPacienteAsync(
                    paciente.IdPaciente
                );

            return Json(resultado);
        }

        // =====================================================
        // MÉTODOS PRIVADOS
        // =====================================================

        private int? ObtenerIdUsuarioActual()
        {
            var claimId = User
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            return int.TryParse(claimId, out var idUsuario)
                ? idUsuario
                : null;
        }
    }
}