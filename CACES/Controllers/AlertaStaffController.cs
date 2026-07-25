using CACES.BLL.DTOs.SeguimientoPostOperatorio;
using CACES.BLL.Servicios.AlertaStaff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CACES.Controllers
{
    public class AlertaStaffController : Controller
    {
        private readonly IAlertaStaffServicio _servicio;

        public AlertaStaffController(IAlertaStaffServicio servicio)
        {
            _servicio = servicio;
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<ActionResult> ObtenerTodas()
        {
            var resultado = await _servicio.ObtenerTodas();
            return Json(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<ActionResult> ObtenerPendientes()
        {
            var resultado = await _servicio.ObtenerPendientes();
            return Json(resultado);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<ActionResult> AtenderAlerta([FromBody] AtenderAlertaDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // El ID de quien atiende siempre se toma de la sesión autenticada,
            // nunca de lo que mande el cliente.
            var idUsuarioClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idUsuarioClaim, out var idUsuarioAtendio))
            {
                return Unauthorized();
            }
            dto.IdUsuarioAtendio = idUsuarioAtendio;

            var resultado = await _servicio.AtenderAlerta(dto);
            if (!resultado.EsCorrecto)
            {
                ModelState.AddModelError(string.Empty, resultado.mensaje ?? "No se pudo atender la alerta.");
                return BadRequest(resultado);
            }

            return Json(resultado);
        }
    }
}