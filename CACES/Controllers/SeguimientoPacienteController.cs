using CACES.BLL.Servicios.SeguimientoPaciente;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Controllers
{
    public class SeguimientoPacienteController : Controller
    {
        private readonly ISeguimientoPacienteServicio _servicio;

        public SeguimientoPacienteController(ISeguimientoPacienteServicio servicio)
        {
            _servicio = servicio;
        }

        [HttpGet("cirugia/{idCirugia}")]
        public async Task<IActionResult> ObtenerPorCirugia(int idCirugia)
        {
            var respuesta = await _servicio.ObtenerPorCirugia(idCirugia);
            return Ok(respuesta);
        }

        [HttpPost("generar/{idCirugia}")]
        public async Task<IActionResult> GenerarCheckpoints(int idCirugia)
        {
            var respuesta = await _servicio.GenerarCheckpoints(idCirugia);
            return Ok(respuesta);
        }
    }
}
