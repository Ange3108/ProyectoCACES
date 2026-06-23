using CACES.BLL.DTOs.Procedimientos;
using CACES.BLL.Servicios.Procedimientos;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CACES.Controllers
{
    public class ProcedimientosController : Controller
    {
        private readonly IProcedimientosServicio _procedimientosServicio;

        public ProcedimientosController(IProcedimientosServicio procedimientosServicio)
        {
            _procedimientosServicio = procedimientosServicio;
        }


        [HttpGet]
        public async Task<IActionResult> ObtenerProcedimientos()
        {
            var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (claimId == null)
            {
                return Challenge(); 
            }

            int idPacienteLogueado = int.Parse(claimId);

            var modeloVista = await _procedimientosServicio.ObtenerDetalleCirugiaAsync(idPacienteLogueado);

            return View("~/Views/Procedimiento/Quirurgicos.cshtml",modeloVista);

        }

        [HttpGet]
        public async Task<IActionResult> Editar(int idPaciente, int id)
        {
            var modelo = await _procedimientosServicio.ObtenerPorIdParaEditarAsync(idPaciente, id);

            if (modelo == null)
            {
                return NotFound();
            }

            ViewData["IdPaciente"] = idPaciente;

            return View("~/Views/Procedimiento/EditarProcedimientos.cshtml",modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(EditarProcedimientosDTO dto, int idPaciente)
        {
            bool exito = false;

            if (ModelState.IsValid)
            {
                exito = await _procedimientosServicio.ActualizarProcedimientoAsync(dto, idPaciente);
                if (exito)
                {
                    return RedirectToAction("ObtenerProcedimientos");
                }
            }
            var datosOriginales = await _procedimientosServicio.ObtenerPorIdParaEditarAsync(idPaciente, dto.Id_Procedimiento);

            if (datosOriginales != null)
            {
                dto.Nombre = datosOriginales.Nombre;
                dto.NombreMedico = datosOriginales.NombreMedico;
                dto.NombrePaciente = datosOriginales.NombrePaciente;
                dto.PrimerApellidoPaciente = datosOriginales.PrimerApellidoPaciente;
                dto.SegundoApellidoPaciente = datosOriginales.SegundoApellidoPaciente;
            }

            ViewData["IdPaciente"] = idPaciente;

            ModelState.AddModelError("", "No se pudieron guardar los cambios. Verifique los datos ingresados.");
            return View("~/Views/Procedimiento/EditarProcedimientos.cshtml", dto);
        }
    }
}
