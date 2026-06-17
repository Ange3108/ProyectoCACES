using AutoMapper;
using CACES.BLL.DTOs.Paciente;
using CACES.BLL.DTOs.Usuario;
using CACES.BLL.Servicios.ConfirmacionCorreo;
using CACES.BLL.Servicios.Usuario;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.HistorialMedicos;
using CACES.DAL.Repositorios.Pacientes;
using CACES.DAL.Repositorios.Usuario;

namespace CACES.BLL.Servicios.Paciente
{
    public class PacienteServicio : IPacienteServicio
    {
        private readonly IPacienteRepositorio _pacienteRepositorio;
        private readonly IUsuarioService _usuarioServicio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IHistorialMedicoRepositorio _historialRepositorio;
        private readonly IEmailServicio _emailServicio;
        private readonly IMapper _mapper;

        public PacienteServicio(
            IPacienteRepositorio pacienteRepositorio,
            IUsuarioService usuarioServicio,
            IUsuarioRepositorio usuarioRepositorio,
            IHistorialMedicoRepositorio historialRepositorio,
            IEmailServicio emailServicio,
            IMapper mapper)
        {
            _pacienteRepositorio = pacienteRepositorio;
            _usuarioServicio = usuarioServicio;
            _usuarioRepositorio = usuarioRepositorio;
            _historialRepositorio = historialRepositorio;
            _emailServicio = emailServicio;
            _mapper = mapper;
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
            {
                throw new Exception(usuario?.mensaje ?? "No se pudo crear el usuario.");
            }

            var usuarioCreado = usuario.Dato;

            var usuarioEntidad = await _usuarioRepositorio.GetUsuarioByEmailAsync(dto.Usuario.CorreoElectronico);

            if (usuarioEntidad == null)
            {
                throw new Exception("El usuario se creó, pero no se pudo recuperar desde la base de datos.");
            }

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

            var paciente = new DAL.Entidades.Paciente
            {
                IdUsuario = usuarioEntidad.IdUsuario,
                HistorialMedico = nuevoHistorial
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
    }
}