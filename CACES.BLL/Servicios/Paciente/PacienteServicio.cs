using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Paciente;
using CACES.BLL.DTOs.Usuario;
using CACES.BLL.Mappers;
using CACES.BLL.Servicios.ConfirmacionCorreo;
using CACES.BLL.Servicios.Usuario;
using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using CACES.DAL.Entidades.Roles;
using CACES.DAL.Repositorios.HistorialMedicos;
using CACES.DAL.Repositorios.Pacientes;
using CACES.DAL.Repositorios.Usuario;
using Microsoft.EntityFrameworkCore;

namespace CACES.BLL.Servicios.Paciente
{
    public class PacienteServicio : IPacienteServicio
    {
        private readonly IPacienteRepositorio _pacienteRepositorio;
        private readonly IUsuarioService _usuarioServicio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IEmailServicio _emailServicio;
        private readonly CACESDbContext _context;
        private readonly IHistorialMedicoRepositorio _historialRepositorio;

        public PacienteServicio(
            IPacienteRepositorio pacienteRepositorio,
            IUsuarioService usuarioServicio,
            IUsuarioRepositorio usuarioRepositorio,
            IHistorialMedicoRepositorio historialRepositorio,
            IEmailServicio emailServicio,
            CACESDbContext context)
        {
            _pacienteRepositorio = pacienteRepositorio;
            _usuarioServicio = usuarioServicio;
            _usuarioRepositorio = usuarioRepositorio;
            _historialRepositorio = historialRepositorio;
            _emailServicio = emailServicio;
            _context = context;
        }

        public async Task<respuestaErrores<List<MostrarPacienteDTO>>> GetPacientesAsync()
        {
            var pacientes = await _pacienteRepositorio.GetPacientesAsync();
            var pacientesDTO = pacientes.Select(p => p.ToMostrarPacienteDTO()).ToList();

            return new respuestaErrores<List<MostrarPacienteDTO>>
            {
                EsCorrecto = true,
                mensaje = "Pacientes obtenidos exitosamente.",
                Dato = pacientesDTO,
                codigo = 200
            };
        }

        public async Task<respuestaErrores<MostrarPacienteDTO?>> GetPacienteByIdAsync(int id)
        {
            var paciente = await _pacienteRepositorio.GetPacienteByIdAsync(id);

            if (paciente == null)
            {
                return new respuestaErrores<MostrarPacienteDTO?>
                {
                    EsCorrecto = false,
                    mensaje = "Paciente no encontrado.",
                    codigo = 404
                };
            }

            return new respuestaErrores<MostrarPacienteDTO?>
            {
                EsCorrecto = true,
                mensaje = "Paciente obtenido exitosamente.",
                Dato = paciente.ToMostrarPacienteDTO(),
                codigo = 200
            };
        }

        public async Task<respuestaErrores<MostrarPacienteDTO?>> GetPacienteByDUIAsync(string dui)
        {
            var paciente = await _pacienteRepositorio.GetPacienteByDUIAsync(dui);

            if (paciente == null)
            {
                return new respuestaErrores<MostrarPacienteDTO?>
                {
                    EsCorrecto = false,
                    mensaje = "Paciente no encontrado.",
                    codigo = 404
                };
            }

            return new respuestaErrores<MostrarPacienteDTO?>
            {
                EsCorrecto = true,
                mensaje = "Paciente obtenido exitosamente.",
                Dato = paciente.ToMostrarPacienteDTO(),
                codigo = 200
            };
        }

        public async Task<respuestaErrores<bool>> DesactivarPacienteAsync(int idPaciente)
        {
            var paciente = await _pacienteRepositorio.GetPacienteByIdAsync(idPaciente);

            if (paciente == null || paciente.Usuario == null)
            {
                return new respuestaErrores<bool>
                {
                    EsCorrecto = false,
                    mensaje = "Paciente no encontrado.",
                    codigo = 404
                };
            }

            var resultado = await _usuarioRepositorio.DesactivarUsuarioAsync(paciente.IdUsuario);

            if (!resultado)
            {
                return new respuestaErrores<bool>
                {
                    EsCorrecto = false,
                    mensaje = "No se pudo desactivar al paciente en el sistema.",
                    codigo = 500
                };
            }

            string asunto = "Cuenta desactivada - CACES";

            string cuerpo = $@"
                <h2>Cuenta desactivada</h2>
                <p>Hola {paciente.Usuario.Nombres},</p>
                <p>Su cuenta en el sistema CACES ha sido desactivada.</p>
                <p>Para volver a utilizarla, deberá validar nuevamente su correo electrónico.</p>
                <p>Gracias,<br/>Sistema CACES</p>
            ";

            await _emailServicio.EnviarCorreoAsync(
                paciente.Usuario.CorreoElectronico,
                asunto,
                cuerpo
            );

            return new respuestaErrores<bool>
            {
                EsCorrecto = true,
                mensaje = "Paciente desactivado exitosamente.",
                Dato = true,
                codigo = 200
            };
        }

        public async Task<respuestaErrores<MostrarUsuarioDTO>> CreatePacienteAsync(RegistrarPacienteDTO dto)
        {
            var usuario = await _usuarioServicio.CrearUsuarioAsync(dto.Usuario);

            if (usuario?.Dato == null)
            {
                return new respuestaErrores<MostrarUsuarioDTO>
                {
                    EsCorrecto = false,
                    mensaje = usuario?.mensaje ?? "No se pudo crear el usuario.",
                    codigo = 500
                };
            }

            var usuarioCreado = usuario.Dato;

            var usuarioEntidad = await _usuarioRepositorio
                .GetUsuarioByEmailAsync(dto.Usuario.CorreoElectronico);

            if (usuarioEntidad == null)
            {
                return new respuestaErrores<MostrarUsuarioDTO>
                {
                    EsCorrecto = false,
                    mensaje = "El usuario se creó, pero no se pudo recuperar desde la base de datos.",
                    codigo = 500
                };
            }

            var rolPaciente = await _context.AspNetRoles
                .FirstOrDefaultAsync(r => r.Name == "Paciente");

            if (rolPaciente == null)
            {
                return new respuestaErrores<MostrarUsuarioDTO>
                {
                    EsCorrecto = false,
                    mensaje = "No existe el rol Paciente.",
                    codigo = 500
                };
            }

            var yaTieneRolUsuarioRoles = await _context.UsuarioRoles
                .AnyAsync(x => x.IdUsuario == usuarioEntidad.IdUsuario &&
                               x.RoleId == rolPaciente.Id);

            if (!yaTieneRolUsuarioRoles)
            {
                await _context.UsuarioRoles.AddAsync(new UsuarioRoles
                {
                    IdUsuario = usuarioEntidad.IdUsuario,
                    RoleId = rolPaciente.Id
                });
            }

            await _context.SaveChangesAsync();

            var nuevoHistorial = new HistorialMedico
            {
                TipoSangre = dto.Historial.TipoSangre,
                Medicamentos = dto.Historial.Medicamentos,
                Alergias = dto.Historial.Alergias,
                EnfermedadesCronicas = dto.Historial.EnfermedadesCronicas,
                Antecedentes = dto.Historial.Antecedentes,
                Detalles = dto.Historial.Detalles,
                FechaDeCreacion = DateTime.Now
            };

            var historialCreado = await _historialRepositorio.CreateHistorialAsync(nuevoHistorial);

            var paciente = new DAL.Entidades.Paciente
            {
                IdUsuario = usuarioEntidad.IdUsuario,
                IdHistorial = historialCreado.IdHistorial
            };

            bool pacienteCreado = await _pacienteRepositorio.CreatePacienteAsync(paciente);

            if (!pacienteCreado)
            {
                return new respuestaErrores<MostrarUsuarioDTO>
                {
                    EsCorrecto = false,
                    mensaje = "No se pudo crear el paciente con su historial médico.",
                    codigo = 500
                };
            }

            return new respuestaErrores<MostrarUsuarioDTO>
            {
                EsCorrecto = true,
                mensaje = "Paciente registrado exitosamente.",
                Dato = usuarioCreado,
                codigo = 200
            };
        }

        public async Task<respuestaErrores<bool>> RegistrarPacienteAsync(RegistrarPacienteDTO pacienteDto)
        {
            var resultado = await CreatePacienteAsync(pacienteDto);

            return new respuestaErrores<bool>
            {
                EsCorrecto = resultado.EsCorrecto,
                mensaje = resultado.mensaje,
                Dato = resultado.EsCorrecto,
                codigo = resultado.codigo
            };
        }

        public async Task<respuestaErrores<MostrarPacienteDTO>> GetPacienteByUsuarioIdAsync(int idUsuario)
        {
            var paciente = await _pacienteRepositorio.GetPacienteByUsuarioIdAsync(idUsuario);

            if (paciente == null)
            {
                return new respuestaErrores<MostrarPacienteDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Paciente no encontrado.",
                    codigo = 404
                };
            }

            return new respuestaErrores<MostrarPacienteDTO>
            {
                EsCorrecto = true,
                mensaje = "Paciente obtenido exitosamente.",
                Dato = paciente.ToMostrarPacienteDTO(),
                codigo = 200
            };
        }

        public async Task<int> ObtenerIdPacientePorUsuarioIdAsync(int idUsuario)
        {
            var paciente = await _pacienteRepositorio.ObtenerPorUsuarioIdAsync(idUsuario);
            return paciente != null ? paciente.IdPaciente : 0;
        }

        public async Task<respuestaErrores<IEnumerable<MostrarPacienteDTO>>> ObtenerPacientesActivosAsync()
        {
            var pacientes = await _pacienteRepositorio.ObtenerPacientesActivosAsync();
            var pacientesDTO = pacientes.Select(p => p.ToMostrarPacienteDTO());

            return new respuestaErrores<IEnumerable<MostrarPacienteDTO>>
            {
                EsCorrecto = true,
                mensaje = "Pacientes activos obtenidos exitosamente.",
                Dato = pacientesDTO,
                codigo = 200
            };
        }

        public async Task<respuestaErrores<bool>> ActivarPacienteAsync(int idPaciente)
        {
            var paciente = await _pacienteRepositorio.GetPacienteByIdAsync(idPaciente);

            if (paciente == null || paciente.Usuario == null)
            {
                return new respuestaErrores<bool>
                {
                    EsCorrecto = false,
                    mensaje = "Paciente no encontrado.",
                    Dato = false,
                    codigo = 404
                };
            }

            paciente.Usuario.Estado = true;

            _context.Usuarios.Update(paciente.Usuario);

            await _context.SaveChangesAsync();

            return new respuestaErrores<bool>
            {
                EsCorrecto = true,
                mensaje = "Paciente activado exitosamente.",
                Dato = true,
                codigo = 200
            };
        }
    }
}