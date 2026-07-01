using AutoMapper;
using CACES.BLL.DTOs.HorariosDisponibles;
using CACES.BLL.Servicios.Horarios;
using CACES.BLL.Servicios.Medicos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CACES.Controllers
{
    public class HorariosController : Controller
    {
        private readonly IHorarioServicio _horarioServicio;
        private readonly IMedicoServicio _medicoServicio;
        private readonly IMapper _mapper;
        public HorariosController(IHorarioServicio horarioServicio, IMedicoServicio medicoServicio, IMapper mapper) 
        {
            _horarioServicio = horarioServicio;
            _medicoServicio = medicoServicio;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Registrar(CancellationToken cancellationToken)
        {
            var respuestaMedicos = await _medicoServicio.GetMedicosAsync();
            var medicos = respuestaMedicos?.Dato ?? [];

            AsignarHorario modeloVista = new()
            {
                Formulario = new() { Horarios = [] },
                MedicosDisponibles = medicos
            };

            ViewData["MedicosSelectList"] = new SelectList(medicos, "IdMedico", "Usuario.NombreCompleto");

            return View("~/Views/Configuracion/RegistrarHorarios.cshtml", modeloVista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar(AsignarHorario modelo, CancellationToken cancellationToken)
        {
            ModelState.Remove("MedicosDisponibles");
            ModelState.Remove("Formulario.Horarios");
            if (!ModelState.IsValid)
            {
                var respuestaMedicos = await _medicoServicio.GetMedicosAsync();
                var medicos = respuestaMedicos?.Dato ?? [];

                ViewData["MedicosSelectList"] = new SelectList(medicos, "IdMedico", "Usuario.NombreCompleto");
                return View("~/Views/Configuracion/RegistrarHorarios.cshtml", modelo);
            }

            bool operacionExitosa = await _horarioServicio.RegistrarDisponibilidadAsync(modelo.Formulario, cancellationToken);

            if (!operacionExitosa)
            {
                ViewData["Error"] = "No se pudieron guardar los horarios en el sistema.";

                var respuestaMedicos = await _medicoServicio.GetMedicosAsync();
                var medicos = respuestaMedicos?.Dato ?? [];
                ViewData["MedicosSelectList"] = new SelectList(medicos, "IdMedico", "Usuario.NombreCompleto");

                return View("~/Views/Configuracion/RegistrarHorarios.cshtml", modelo);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
