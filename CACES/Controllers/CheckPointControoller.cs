using CACES.BLL.DTOs.CheckPoint;
using CACES.BLL.Servicios.ConfiguracionCheckPoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Controllers
{
    public class ConfiguracionCheckPointController : Controller
    {
        private readonly IConfiguracionCheckPointsServicio _configuracionCheckPointServicio;

        public ConfiguracionCheckPointController(IConfiguracionCheckPointsServicio configuracionCheckPointServicio)
        {
            _configuracionCheckPointServicio = configuracionCheckPointServicio;
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerConfiguracionesCheckPoints()
        {
            var resultado = await _configuracionCheckPointServicio.ObtenerConfiguracionesCheckPoints();
            return Json(resultado);
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerConfiguracionesCheckPointsActivas()
        {
            var resultado = await _configuracionCheckPointServicio.ObtenerConfiguracionesCheckPointsActivas();
            return Json(resultado);
        }

        [HttpPost]
        public async Task<ActionResult> CrearCheckPoint( RegistrarConfiguracionCheckpointDTO checkpoint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = await _configuracionCheckPointServicio.CrearConfiguracionCheckPoint(checkpoint);
            if (!resultado.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, resultado.mensaje ?? "No se pudo crear el checkpoint.");
                return BadRequest(resultado);
            }

            return Json(resultado);
        }

        [HttpPost]
        public async Task<ActionResult> ActualizarCheckPoint(int id, ConfiguracionCheckPointDTO checkpoint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = await _configuracionCheckPointServicio.ActualizarConfiguracionCheckPoint(checkpoint);
            if (!resultado.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, resultado.mensaje ?? "No se pudo actualizar el checkpoint.");
                return BadRequest(resultado);
            }

            return Json(resultado);
        }

        [HttpPost]
        public async Task<ActionResult> DesactivarCheckPoint(int id)
        {
            var resultado = await _configuracionCheckPointServicio.DesactivarConfiguracionCheckPoint(id);
            if (!resultado.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, resultado.mensaje ?? "No se pudo desactivar el checkpoint.");
                return BadRequest(resultado);
            }

            return Json(resultado);
        }
    }
}