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
                return Challenge(); // Fuerza el desafío de autenticación si algo falla con la sesión
            }

            int idPacienteLogueado = int.Parse(claimId);

            // El servicio procesa de forma segura y privada los datos de ese paciente único
            var modeloVista = await _procedimientosServicio.ObtenerDetalleCirugiaAsync(idPacienteLogueado);

            return View("~/Views/Procedimiento/Quirurgicos.cshtml",modeloVista);
        }
    }
}
