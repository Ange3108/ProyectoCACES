using CACES.BLL.Servicios.HistorialMedicos;
using CACES.BLL.Servicios.Paciente;
using CACES.DAL.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CACES.BLL.Servicios.ArchivosHistorial;

namespace CACES.Controllers
{
    [Authorize]
    public class HistorialMedicoController : Controller
    {
        private readonly IHistorialMedicoServicio _historialServicio;
        private readonly IPacienteServicio _pacienteServicio;
        private readonly IArchivoHistorialServicio _archivoServicio;

        public HistorialMedicoController(IHistorialMedicoServicio historialServicio, IPacienteServicio pacienteServicio, IArchivoHistorialServicio archivoServicio)
        {
            _historialServicio = historialServicio;
            _pacienteServicio = pacienteServicio;
            _archivoServicio = archivoServicio;
        }

        // Consultar historial médico
        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var historial = await _historialServicio.GetHistorialByIdAsync(id);

            if (historial == null)
                return NotFound();

            var archivos = await _archivoServicio.GetArchivosByHistorialAsync(id);
            ViewBag.Archivos = archivos;

            return View("~/Views/HistorialMedico/Detalle.cshtml", historial);
        }
        // Mostrar formulario para editar
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var historial = await _historialServicio.GetHistorialByIdAsync(id);

            if (historial == null)
                return NotFound();

            return View("~/Views/HistorialMedico/Editar.cshtml", historial);
        }

        // Guardar cambios
        [HttpPost]
        public async Task<IActionResult> Editar(HistorialMedico historial)
        {
            if (!ModelState.IsValid)
                return View("~/Views/HistorialMedico/Editar.cshtml", historial);

            bool resultado = await _historialServicio.UpdateHistorialAsync(historial);

            if (!resultado)
            {
                TempData["Error"] = "No se pudo actualizar el historial médico.";
                return View("~/Views/HistorialMedico/Editar.cshtml", historial);
            }

            TempData["Mensaje"] = "Cambios guardados correctamente.";

            return RedirectToAction(nameof(Detalle), new { id = historial.IdHistorial });
        }

        [HttpGet]
        [Authorize(Roles = "Paciente")]
        public async Task<IActionResult> MiHistorial()
        {
            var claimId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(claimId) || !int.TryParse(claimId, out int idUsuario))
                return Unauthorized();

            var paciente = await _pacienteServicio.GetPacienteByUsuarioIdAsync(idUsuario);

            if (paciente == null)
                return NotFound();

            return RedirectToAction("Detalle", new { id = paciente.IdHistorial });
        }

        [HttpGet]
        [Authorize(Roles = "Medico")]
        public IActionResult SubirImagen(int idHistorial)
        {
            ViewBag.IdHistorial = idHistorial;
            return View("~/Views/HistorialMedico/SubirImagen.cshtml");
        }

        [HttpPost]
        [Authorize(Roles = "Medico")]
        public async Task<IActionResult> SubirImagen(int idHistorial, IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
            {
                TempData["Error"] = "Debe seleccionar una imagen o archivo.";
                return RedirectToAction("Detalle", new { id = idHistorial });
            }

            var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "historial");

            if (!Directory.Exists(carpeta))
            {
                Directory.CreateDirectory(carpeta);
            }

            var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(archivo.FileName);
            var rutaFisica = Path.Combine(carpeta, nombreArchivo);

            using (var stream = new FileStream(rutaFisica, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }

            var archivoHistorial = new ArchivoHistorial
            {
                IdHistorial = idHistorial,
                NombreArchivo = archivo.FileName,
                RutaArchivo = "/uploads/historial/" + nombreArchivo,
                TipoArchivo = archivo.ContentType,
                FechaDeSubida = DateTime.Now
            };

            var resultado = await _archivoServicio.CrearArchivoAsync(archivoHistorial);

            TempData[resultado ? "Mensaje" : "Error"] =
                resultado
                    ? "Imagen médica subida correctamente."
                    : "No se pudo subir la imagen médica.";

            return RedirectToAction("Detalle", new { id = idHistorial });
        }
    }
}