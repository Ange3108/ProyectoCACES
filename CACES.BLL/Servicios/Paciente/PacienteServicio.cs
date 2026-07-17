using AutoMapper;
using CACES.BLL.DTOs.Paciente;
using CACES.BLL.DTOs.Usuario;
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

        public async Task<List<DAL.Entidades.Paciente>> GetPacientesAsync()
        {
            return await _pacienteRepositorio.GetPacientesAsync();
        }

        public async Task<DAL.Entidades.Paciente> GetPacienteByIdAsync(int id)
        {
            return await _pacienteRepositorio.GetPacienteByIdAsync(id);
        }

        public async Task<DAL.Entidades.Paciente> GetPacienteByDUIAsync(string dui)
        {
            return await _pacienteRepositorio.GetPacienteByDUIAsync(dui);
        }

        public async Task<bool> DesactivarPacienteAsync(int idPaciente)
        {
            var paciente = await _pacienteRepositorio.GetPacienteByIdAsync(idPaciente);

            if (paciente == null || paciente.Usuario == null)
                return false;

            var resultado = await _usuarioRepositorio.DesactivarUsuarioAsync(paciente.IdUsuario);

            if (!resultado)
                return false;

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

            return true;
        }

        public async Task<MostrarUsuarioDTO> CreatePacienteAsync(RegistrarPacienteDTO dto)
        {
            var usuario = await _usuarioServicio.CrearUsuarioAsync(dto.Usuario);

            if (usuario?.Dato == null)
                throw new Exception(usuario?.mensaje ?? "No se pudo crear el usuario.");

            var usuarioCreado = usuario.Dato;

            var usuarioEntidad = await _usuarioRepositorio
                .GetUsuarioByEmailAsync(dto.Usuario.CorreoElectronico);

            if (usuarioEntidad == null)
                throw new Exception("El usuario se creó, pero no se pudo recuperar desde la base de datos.");

            var rolPaciente = await _context.AspNetRoles
                .FirstOrDefaultAsync(r => r.Name == "Paciente");

            if (rolPaciente == null)
                throw new Exception("No existe el rol Paciente.");

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

            if (pacienteCreado)
                return usuarioCreado;

            throw new Exception("No se pudo crear el paciente con su historial médico.");
        }

        public async Task<bool> RegistrarPacienteAsync(RegistrarPacienteDTO pacienteDto)
        {
            var usuarioCreado = await CreatePacienteAsync(pacienteDto);
            return usuarioCreado != null;
        }

        public async Task<DAL.Entidades.Paciente> GetPacienteByUsuarioIdAsync(int idUsuario)
        {
            return await _pacienteRepositorio.GetPacienteByUsuarioIdAsync(idUsuario);
        }

        public async Task<int> ObtenerIdPacientePorUsuarioIdAsync(int idUsuario)
        {
            var paciente = await _pacienteRepositorio.ObtenerPorUsuarioIdAsync(idUsuario);
            return paciente != null ? paciente.IdPaciente : 0;
        }
        public async Task<IEnumerable<DAL.Entidades.Paciente>> ObtenerPacientesActivosAsync()
        {
            return await _pacienteRepositorio.ObtenerPacientesActivosAsync();
        }

        public async Task<bool> ActivarPacienteAsync(int idPaciente)
        {
            var paciente = await _pacienteRepositorio.GetPacienteByIdAsync(idPaciente);

            if (paciente == null || paciente.Usuario == null)
                return false;

            paciente.Usuario.Estado = true;

            _context.Usuarios.Update(paciente.Usuario);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}