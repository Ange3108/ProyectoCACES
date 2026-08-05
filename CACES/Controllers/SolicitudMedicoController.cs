using Microsoft.AspNetCore.Mvc;
using CACES.BLL.DTOs.SolicitudMedico;
using CACES.BLL.Servicios.SolicitudMedico;
using Microsoft.AspNetCore.Authorization;

namespace CACES.Controllers
{
    public class SolicitudMedicoController : Controller
    {
        private readonly ISolicitudMedicoServicio _servicio;
        private readonly IWebHostEnvironment _environment;

        public SolicitudMedicoController(
            ISolicitudMedicoServicio servicio,
            IWebHostEnvironment environment)
        {
            _servicio = servicio;
            _environment = environment;
        }

        // =====================================================
        // VISTAS
        // =====================================================

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SolicitarIngreso()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult GestionSolicitudes()
        {
            return View();
        }

        // =====================================================
        // ESPECIALIDADES
        // =====================================================

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerEspecialidades()
        {
            var respuesta =
                await _servicio.ObtenerEspecialidadesAsync();

            return Json(respuesta);
        }

        // =====================================================
        // REGISTRAR SOLICITUD
        // =====================================================

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarSolicitud(
            [FromForm] RegistrarSolicitudFormularioDTO formulario)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value!.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                return BadRequest(new
                {
                    esCorrecto = false,
                    mensaje = errores.FirstOrDefault()
                        ?? "Revise la información ingresada."
                });
            }

            string? curriculumGuardado = null;
            string? fotoGuardada = null;

            try
            {
                curriculumGuardado =
                    await GuardarCurriculumAsync(
                        formulario.Curriculum
                    );

                if (formulario.Foto != null &&
                    formulario.Foto.Length > 0)
                {
                    fotoGuardada =
                        await GuardarFotoAsync(
                            formulario.Foto
                        );
                }

                var dto = new RegistrarSolicitudMedicoDTO
                {
                    Nombres = formulario.Nombres,
                    PrimerApellido = formulario.PrimerApellido,
                    SegundoApellido = formulario.SegundoApellido,
                    CorreoElectronico = formulario.CorreoElectronico,
                    Telefono = formulario.Telefono,
                    IdEspecialidad = formulario.IdEspecialidad,
                    AniosExperiencia = formulario.AniosExperiencia,
                    Certificaciones = formulario.Certificaciones,
                    Motivo = formulario.Motivo,
                    Curriculum = curriculumGuardado,
                    Foto = fotoGuardada
                };

                var respuesta =
                    await _servicio.RegistrarAsync(dto);

                if (!respuesta.EsCorrecto)
                {
                    EliminarArchivo(curriculumGuardado);
                    EliminarArchivo(fotoGuardada);

                    return BadRequest(respuesta);
                }

                return Json(respuesta);
            }
            catch (InvalidOperationException ex)
            {
                EliminarArchivo(curriculumGuardado);
                EliminarArchivo(fotoGuardada);

                return BadRequest(new
                {
                    esCorrecto = false,
                    mensaje = ex.Message
                });
            }
            catch (Exception ex)
            {
                EliminarArchivo(curriculumGuardado);
                EliminarArchivo(fotoGuardada);

                return StatusCode(500, new
                {
                    esCorrecto = false,
                    mensaje =
                        "No fue posible enviar la solicitud: " +
                        (ex.InnerException?.Message ?? ex.Message)
                });
            }
        }

        // =====================================================
        // ADMINISTRACIÓN
        // =====================================================

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ObtenerSolicitudes()
        {
            var respuesta =
                await _servicio.ObtenerTodasAsync();

            return Json(respuesta);
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ObtenerSolicitud(
            int idSolicitud)
        {
            var respuesta =
                await _servicio.ObtenerPorIdAsync(idSolicitud);

            return Json(respuesta);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ResponderSolicitud(
            [FromBody] ResponderSolicitudMedicoDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    esCorrecto = false,
                    mensaje =
                        "Revise la información de la respuesta."
                });
            }

            var respuesta =
                await _servicio.ResponderAsync(dto);

            return Json(respuesta);
        }

        // =====================================================
        // ARCHIVOS
        // =====================================================

        private async Task<string> GuardarCurriculumAsync(
            IFormFile curriculum)
        {
            if (curriculum == null ||
                curriculum.Length == 0)
            {
                throw new InvalidOperationException(
                    "Debe adjuntar el currículum."
                );
            }

            const long maximoBytes = 5 * 1024 * 1024;

            if (curriculum.Length > maximoBytes)
            {
                throw new InvalidOperationException(
                    "El currículum no puede superar los 5 MB."
                );
            }

            var extension =
                Path.GetExtension(curriculum.FileName)
                    .ToLowerInvariant();

            if (extension != ".pdf")
            {
                throw new InvalidOperationException(
                    "El currículum debe estar en formato PDF."
                );
            }

            var carpeta = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                "solicitudes-medicos",
                "curriculums"
            );

            Directory.CreateDirectory(carpeta);

            var nombreArchivo =
                $"{Guid.NewGuid():N}.pdf";

            var rutaFisica =
                Path.Combine(carpeta, nombreArchivo);

            await using var stream =
                new FileStream(
                    rutaFisica,
                    FileMode.Create
                );

            await curriculum.CopyToAsync(stream);

            return
                $"/uploads/solicitudes-medicos/curriculums/{nombreArchivo}";
        }

        private async Task<string> GuardarFotoAsync(
            IFormFile foto)
        {
            const long maximoBytes = 5 * 1024 * 1024;

            if (foto.Length > maximoBytes)
            {
                throw new InvalidOperationException(
                    "La fotografía no puede superar los 5 MB."
                );
            }

            var extensionesPermitidas = new[]
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".webp"
            };

            var extension =
                Path.GetExtension(foto.FileName)
                    .ToLowerInvariant();

            if (!extensionesPermitidas.Contains(extension))
            {
                throw new InvalidOperationException(
                    "La fotografía debe ser JPG, PNG o WEBP."
                );
            }

            var carpeta = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                "solicitudes-medicos",
                "fotos"
            );

            Directory.CreateDirectory(carpeta);

            var nombreArchivo =
                $"{Guid.NewGuid():N}{extension}";

            var rutaFisica =
                Path.Combine(carpeta, nombreArchivo);

            await using var stream =
                new FileStream(
                    rutaFisica,
                    FileMode.Create
                );

            await foto.CopyToAsync(stream);

            return
                $"/uploads/solicitudes-medicos/fotos/{nombreArchivo}";
        }

        private void EliminarArchivo(string? rutaRelativa)
        {
            if (string.IsNullOrWhiteSpace(rutaRelativa))
            {
                return;
            }

            var rutaLimpia =
                rutaRelativa.TrimStart('/')
                    .Replace('/', Path.DirectorySeparatorChar);

            var rutaFisica =
                Path.Combine(
                    _environment.WebRootPath,
                    rutaLimpia
                );

            if (System.IO.File.Exists(rutaFisica))
            {
                System.IO.File.Delete(rutaFisica);
            }
        }
    }
}