using AutoMapper;
using CACES.BLL.DTOs.Paciente;
using CACES.BLL.DTOs.Usuario;
using CACES.BLL.Servicios.Usuario;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.HistorialMedicos;
using CACES.DAL.Repositorios.Pacientes;
using CACES.DAL.Repositorios.Usuario;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CACES.BLL.Servicios.Paciente
{
    public class PacienteServicio : IPacienteServicio
    {
        //Se inyecto el servicio para no tener que repertir los métodos de validación de usuario, como validar correo, DUI y contraseña segura
        private readonly IPacienteRepositorio _pacienteRepositorio;
        private readonly IUsuarioService _usuarioServicio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IHistorialMedicoRepositorio _historialRepositorio;
        private readonly IMapper _mapper;


        public PacienteServicio(
            IPacienteRepositorio pacienteRepositorio,
            IUsuarioService usuarioServicio,
            IUsuarioRepositorio usuarioRepositorio,
            IHistorialMedicoRepositorio historialRepositorio,
            IMapper mapper)
        {
            _pacienteRepositorio = pacienteRepositorio;
            _usuarioServicio = usuarioServicio;
            _usuarioRepositorio = usuarioRepositorio;
            _historialRepositorio = historialRepositorio;
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

            if (paciente == null)
                return false;

            return await _usuarioRepositorio.DesactivarUsuarioAsync(paciente.IdUsuario);
        }

        public async Task<MostrarUsuarioDTO> CreatePacienteAsync(
            RegistrarPacienteDTO dto)
        {
            //Delega la creación del usuario al servicio correspondiente.
            //Este método se encargará de encriptar la contraseña y guardar el usuario.
            var usuario = await _usuarioServicio.CrearUsuarioAsync(dto.Usuario);

            if(usuario?.Dato == null)
            {
                return null;
            }
            var usuarioCreado = usuario.Dato;
            // Crear Paciente
            var paciente = _mapper.Map<DAL.Entidades.Paciente>(dto.Usuario);
            // Crear Historial Médico
            var nuevoHistorial = _mapper.Map<HistorialMedico>(dto.Historial);

            //Asigna los IDs correspondientes para establecer las relaciones entre las entidades
            paciente.IdUsuario = usuarioCreado.idUsuario;
            paciente.HistorialMedico = nuevoHistorial;

            //crear el paciente y el historial médico en la base de datos. El repositorio se encargará de guardar ambas entidades en una sola transacción para garantizar la integridad de los datos.
            bool pacienteCreado = await _pacienteRepositorio.CreatePacienteAsync(paciente);

            if (pacienteCreado)
            {
                return usuarioCreado;
            }
            else
            {
                return null;
            }

        }
    }
}