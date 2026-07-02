using CACES.DAL.DBContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CACES.Controllers
{
    public class HomeController : Controller
    {
        private readonly CACESDbContext _context;

        public HomeController(CACESDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var hoy = DateTime.Today;

            ViewBag.Especialidades = await _context.Especialidades
                .Where(e => e.Estado == true)
                .Take(4)
                .ToListAsync();

            ViewBag.Paquetes = await _context.Paquetes
                .Where(p => p.Estado == true)
                .Take(3)
                .ToListAsync();

            ViewBag.TotalCirugias = await _context.Cirugias
                .CountAsync();

            ViewBag.TotalCitas = await _context.Citas
                .Where(c => c.Fecha.Date == hoy)
                .CountAsync();

            ViewBag.TotalPacientes = await _context.Pacientes
                .CountAsync();

            ViewBag.TotalMedicos = await _context.Medicos
                .Include(m => m.Usuario)
                .Where(m => m.Usuario.Estado == true)
                .CountAsync();

            ViewBag.Noticias = await _context.Noticias
            .Where(n => n.Estado == true)
            .OrderByDescending(n => n.FechaDePublicacion)
            .Take(5)
            .ToListAsync();

            return View();

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Reportes()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ResumenCentro()
        {
            var hoy = DateTime.Today;

            var data = new
            {
                cirugias = await _context.Cirugias.CountAsync(),
                citas = await _context.Citas
                    .Where(c => c.Fecha.Date == hoy)
                    .CountAsync(),
                pacientes = await _context.Pacientes.CountAsync(),
                medicos = await _context.Medicos
                    .Include(m => m.Usuario)
                    .Where(m => m.Usuario.Estado == true)
                    .CountAsync(),
                timestamp = DateTime.Now
            };

            return Json(data);
        }
    }
}