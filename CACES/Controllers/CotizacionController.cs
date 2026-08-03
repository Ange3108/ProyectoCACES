using CACES.BLL.DTOs.Cotizacion;
using CACES.BLL.Servicios.Cotizaciones;
using CACES.BLL.Servicios.Paciente;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CACES.Controllers
{
    [Authorize]
    public class CotizacionController : Controller
    {
        private readonly ICotizacionServicio _cotizacionServicio;
        private readonly IPacienteServicio _pacienteServicio;

        public CotizacionController(
            ICotizacionServicio cotizacionServicio,
            IPacienteServicio pacienteServicio)
        {
            _cotizacionServicio = cotizacionServicio;
            _pacienteServicio = pacienteServicio;
        }

        // =====================================================
        // VISTAS
        // =====================================================

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public IActionResult GestionCotizaciones()
        {
            return View();
        }

        [Authorize(Roles = "Paciente")]
        [HttpGet]
        public IActionResult MisCotizaciones()
        {
            return View();
        }

        [Authorize(Roles = "Paciente")]
        [HttpGet]
        public IActionResult SolicitarCotizacion()
        {
            return View();
        }

        // =====================================================
        // REGISTRAR SOLICITUD
        // =====================================================

        [Authorize(Roles = "Paciente")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarCotizacion(
     [FromBody] RegistrarCotizacionDTO dto)
        {
            if (dto == null)
            {
                return BadRequest(new
                {
                    esCorrecto = false,
                    mensaje = "No se recibió la información de la solicitud."
                });
            }

            var idUsuario = ObtenerIdUsuarioActual();

            if (idUsuario == null)
            {
                return Unauthorized(new
                {
                    esCorrecto = false,
                    mensaje = "No se pudo identificar al usuario autenticado."
                });
            }

            var pacienteResultado =
                await _pacienteServicio.GetPacienteByUsuarioIdAsync(
                    idUsuario.Value
                );

            if (!pacienteResultado.EsCorrecto || pacienteResultado.Dato == null)
            {
                return NotFound(new
                {
                    esCorrecto = false,
                    mensaje = "No se encontró el perfil de paciente."
                });
            }

            dto.IdPaciente = pacienteResultado.Dato.IdPaciente;

            ModelState.Remove(nameof(dto.IdPaciente));

            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values
                    .SelectMany(valor => valor.Errors)
                    .Select(error => error.ErrorMessage)
                    .Where(mensaje => !string.IsNullOrWhiteSpace(mensaje))
                    .ToList();

                return BadRequest(new
                {
                    esCorrecto = false,
                    mensaje = errores.Count > 0
                        ? string.Join(" ", errores)
                        : "La información proporcionada no es válida."
                });
            }

            try
            {
                var idCotizacion =
    await _cotizacionServicio.RegistrarCotizacionAsync(dto);

                return Json(new
                {
                    esCorrecto = true,
                    idCotizacion = idCotizacion,
                    redirectUrl = Url.Action(
                        "Detalle",
                        "Cotizacion",
                        new { id = idCotizacion }),
                    mensaje = "Cotización generada correctamente."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    esCorrecto = false,
                    mensaje = $"No fue posible registrar la solicitud: {ex.Message}"
                });
            }
        }

        [Authorize(Roles = "Paciente")]
        [HttpGet]
        public async Task<IActionResult> ObtenerMisCotizaciones()
        {
            var idUsuario = ObtenerIdUsuarioActual();

            if (idUsuario == null)
            {
                return Unauthorized(new
                {
                    esCorrecto = false,
                    mensaje = "No se pudo identificar al usuario autenticado."
                });
            }

            var pacienteResultado =
                await _pacienteServicio.GetPacienteByUsuarioIdAsync(
                    idUsuario.Value
                );

            if (!pacienteResultado.EsCorrecto || pacienteResultado.Dato == null)
            {
                return NotFound(new
                {
                    esCorrecto = false,
                    mensaje = "No se encontró el perfil de paciente."
                });
            }

            try
            {
                var lista =
                    await _cotizacionServicio.ObtenerPorPacienteAsync(
                        pacienteResultado.Dato.IdPaciente
                    );

                return Json(new
                {
                    esCorrecto = true,
                    dato = lista
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    esCorrecto = false,
                    mensaje = $"No fue posible cargar las cotizaciones: {ex.Message}"
                });
            }
        }

        // =====================================================
        // GESTIÓN DEL ADMINISTRADOR
        // =====================================================

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> ObtenerCotizaciones()
        {
            try
            {
                var lista =
                    await _cotizacionServicio.ObtenerTodasAsync();

                return Json(new
                {
                    esCorrecto = true,
                    dato = lista
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    esCorrecto = false,
                    mensaje = $"No fue posible cargar las cotizaciones: {ex.Message}"
                });
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> ObtenerCotizacion(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new
                {
                    esCorrecto = false,
                    mensaje = "El identificador de la cotización no es válido."
                });
            }

            var cotizacion =
                await _cotizacionServicio.ObtenerEditarAsync(id);

            if (cotizacion == null)
            {
                return NotFound(new
                {
                    esCorrecto = false,
                    mensaje = "No existe la cotización solicitada."
                });
            }

            return Json(new
            {
                esCorrecto = true,
                dato = cotizacion
            });
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarCotizacion(
            EditarCotizacionDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    esCorrecto = false,
                    mensaje = "La información de la cotización no es válida."
                });
            }

            try
            {
                var resultado =
                    await _cotizacionServicio.ActualizarCotizacionAsync(dto);

                return Json(new
                {
                    esCorrecto = resultado,
                    mensaje = resultado
                        ? "Cotización actualizada correctamente."
                        : "No fue posible actualizar la cotización."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    esCorrecto = false,
                    mensaje = $"No fue posible actualizar la cotización: {ex.Message}"
                });
            }
        }

        // =====================================================
        // COMBOS
        // =====================================================

        [Authorize(Roles = "Paciente,Administrador")]
        [HttpGet]
        public async Task<IActionResult> ObtenerProcedimientos()
        {
            try
            {
                var procedimientos =
                    await _cotizacionServicio.ObtenerProcedimientosAsync();

                var resultado = procedimientos
                    .Select(procedimiento => new
                    {
                        idProcedimiento =
                            procedimiento.Id_Procedimiento,

                        nombre =
                            procedimiento.Nombre,

                        precioBase =
                            procedimiento.PrecioBase
                    })
                    .ToList();

                return Json(new
                {
                    esCorrecto = true,
                    dato = resultado
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    esCorrecto = false,
                    mensaje = $"No fue posible cargar los procedimientos: {ex.Message}"
                });
            }
        }

        [Authorize(Roles = "Paciente,Administrador")]
        [HttpGet]
        public async Task<IActionResult> ObtenerMedicoPorProcedimiento(
    int idProcedimiento)
        {
            if (idProcedimiento <= 0)
            {
                return BadRequest(new
                {
                    esCorrecto = false,
                    mensaje = "El procedimiento seleccionado no es válido."
                });
            }

            try
            {
                var precio =
                    await _cotizacionServicio
                        .ObtenerMedicoPorProcedimientoAsync(
                            idProcedimiento
                        );

                if (precio == null)
                {
                    return NotFound(new
                    {
                        esCorrecto = false,
                        mensaje =
                            "No existe un médico asociado a este procedimiento."
                    });
                }

                var medico = precio.Medico;

                var nombreCompleto = string.Join(
                    " ",
                    new[]
                    {
                medico.Usuario?.Nombres,
                medico.Usuario?.PrimerApellido,
                medico.Usuario?.SegundoApellido
                    }
                    .Where(valor =>
                        !string.IsNullOrWhiteSpace(valor))
                );

                return Json(new
                {
                    esCorrecto = true,
                    dato = new
                    {
                        idMedico = precio.Id_Medico,
                        nombreCompleto = nombreCompleto,
                        especialidad =
                            medico.Especialidad?.Nombre
                            ?? "Sin especialidad",
                        honorarios = precio.Costo
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    esCorrecto = false,
                    mensaje =
                        $"No fue posible obtener el médico: {ex.Message}"
                });
            }
        }

        [Authorize(Roles = "Paciente,Administrador")]
        [HttpGet]
        public async Task<IActionResult> ObtenerMedicos()
        {
            try
            {
                var medicos =
                    await _cotizacionServicio.ObtenerMedicosAsync();

                var resultado = medicos
                    .Select(medico => new
                    {
                        idMedico = medico.IdMedico,

                        nombreCompleto = string.Join(
                            " ",
                            new[]
                            {
                                medico.Usuario?.Nombres,
                                medico.Usuario?.PrimerApellido,
                                medico.Usuario?.SegundoApellido
                            }
                            .Where(valor =>
                                !string.IsNullOrWhiteSpace(valor))
                        ),

                        nombreEspecialidad =
                            medico.Especialidad?.Nombre
                            ?? "Sin especialidad"
                    })
                    .ToList();

                return Json(new
                {
                    esCorrecto = true,
                    dato = resultado
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    esCorrecto = false,
                    mensaje = $"No fue posible cargar los médicos: {ex.Message}"
                });
            }
        }

        [Authorize(Roles = "Paciente,Administrador")]
        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            if (id <= 0)
                return BadRequest();

            var cotizacion =
                await _cotizacionServicio.ObtenerDetalleAsync(id);

            if (cotizacion == null)
                return NotFound();

            return View(cotizacion);
        }
        // =====================================================
        // USUARIO ACTUAL
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