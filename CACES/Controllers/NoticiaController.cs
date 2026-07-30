using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CACES.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class NoticiaController : Controller
    {
        private readonly CACESDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public NoticiaController(
            CACESDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpPost]
 
        public async Task<IActionResult> Crear(
            string titulo,
            string contenido,
            IFormFile? imagen)
        {
            if (string.IsNullOrWhiteSpace(titulo))
            {
                return BadRequest(new
                {
                    esCorrecto = false,
                    mensaje = "El título es obligatorio."
                });
            }

            if (string.IsNullOrWhiteSpace(contenido))
            {
                return BadRequest(new
                {
                    esCorrecto = false,
                    mensaje = "El contenido es obligatorio."
                });
            }

            if (imagen == null || imagen.Length == 0)
            {
                return BadRequest(new
                {
                    esCorrecto = false,
                    mensaje = "Debe seleccionar una imagen."
                });
            }

            string? nombreArchivo = null;
            string? rutaFisica = null;

            try
            {
                string[] extensionesPermitidas =
                {
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".webp"
                };

                string extension = Path
                    .GetExtension(imagen.FileName)
                    .ToLowerInvariant();

                if (!extensionesPermitidas.Contains(extension))
                {
                    return BadRequest(new
                    {
                        esCorrecto = false,
                        mensaje = "La imagen debe ser JPG, JPEG, PNG o WEBP."
                    });
                }

                const long tamanoMaximo = 5 * 1024 * 1024;

                if (imagen.Length > tamanoMaximo)
                {
                    return BadRequest(new
                    {
                        esCorrecto = false,
                        mensaje = "La imagen no puede superar los 5 MB."
                    });
                }

                string carpetaImagenes = Path.Combine(
                    _environment.WebRootPath,
                    "img"
                );

                if (!Directory.Exists(carpetaImagenes))
                {
                    Directory.CreateDirectory(carpetaImagenes);
                }

                nombreArchivo = $"{Guid.NewGuid():N}{extension}";

                rutaFisica = Path.Combine(
                    carpetaImagenes,
                    nombreArchivo
                );

                await using (var stream = new FileStream(
                    rutaFisica,
                    FileMode.Create))
                {
                    await imagen.CopyToAsync(stream);
                }

                var noticia = new Noticia
                {
                    Titulo = titulo.Trim(),
                    Contenido = contenido.Trim(),
                    FechaDePublicacion = DateTime.Now,
                    FechaDeModificacion = null,

                    // Solo se guarda el nombre porque la vista utiliza /img/
                    Imagen = nombreArchivo,

                    Estado = true
                };

                await _context.Noticias.AddAsync(noticia);

                int filasGuardadas =
                    await _context.SaveChangesAsync();

                if (filasGuardadas <= 0)
                {
                    if (System.IO.File.Exists(rutaFisica))
                    {
                        System.IO.File.Delete(rutaFisica);
                    }

                    return StatusCode(500, new
                    {
                        esCorrecto = false,
                        mensaje = "No se pudo guardar la noticia."
                    });
                }

                return Json(new
                {
                    esCorrecto = true,
                    idNoticia = noticia.IdNoticia,
                    mensaje = "Noticia publicada correctamente."
                });
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrWhiteSpace(rutaFisica) &&
                    System.IO.File.Exists(rutaFisica))
                {
                    System.IO.File.Delete(rutaFisica);
                }

                return StatusCode(500, new
                {
                    esCorrecto = false,
                    mensaje = $"Error al publicar la noticia: {ex.Message}"
                });
            }
        }
    }
}