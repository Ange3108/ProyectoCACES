using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CACES.BLL.DTOs.Paciente;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.Pacientes;
using CACES.DAL.Repositorios.Usuario;
using CACES.DAL.Repositorios.HistorialMedicos;

namespace CACES.BLL.Servicios.Paciente
{
    public class PacienteServicio : IPacienteServicio
    {
        private readonly IPacienteRepositorio _pacienteRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IHistorialMedicoRepositorio _historialRepositorio;

        public PacienteServicio(
            IPacienteRepositorio pacienteRepositorio,
            IUsuarioRepositorio usuarioRepositorio,
            IHistorialMedicoRepositorio historialRepositorio)
        {
            _pacienteRepositorio = pacienteRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _historialRepositorio = historialRepositorio;
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

        public async Task<bool> CreatePacienteAsync(DAL.Entidades.Paciente paciente)
        {
            return await _pacienteRepositorio.CreatePacienteAsync(paciente);
        }

        public async Task<bool> UpdatePacienteAsync(DAL.Entidades.Paciente paciente)
        {
            return await _pacienteRepositorio.UpdatePacienteAsync(paciente);
        }

        public async Task<bool> DeletePacienteAsync(int id)
        {
            return await _pacienteRepositorio.DeletePacienteAsync(id);
        }

        public async Task<bool> RegistrarPacienteAsync(
            RegistrarPacienteDTO dto)
        {
            try
            {
                // Validar correo
                var correoExistente =
                    await _usuarioRepositorio
                        .GetUsuarioByEmailAsync(dto.CorreoElectronico);

                if (correoExistente != null)
                    return false;

                // Validar DUI
                var duiExistente =
                    await _usuarioRepositorio
                        .GetUsuarioByDUIAsync(dto.DUI);

                if (duiExistente != null)
                    return false;

                // Crear Usuario
                var usuario = new CACES.DAL.Entidades.Usuario
                {
                    Nombres = dto.Nombres,
                    PrimerApellido = dto.PrimerApellido,
                    SegundoApellido = dto.SegundoApellido,
                    CorreoElectronico = dto.CorreoElectronico,
                    DUI = dto.DUI,
                    Telefono = dto.Telefono,
                    Direccion = dto.Direccion,
                    Edad = dto.Edad,

                    PasswordHash = dto.Password,
                    SecurityStamp = Guid.NewGuid().ToString(),

                    Estado = true,
                    FechaDeRegistro = DateTime.Now
                };
                await _usuarioRepositorio
                    .CreateUsuarioAsync(usuario);

                // Crear Historial Médico
                var historial = new HistorialMedico
                {
                    TipoSangre = dto.TipoSangre,
                    Alergias = dto.Alergias,
                    EnfermedadesCronicas = dto.EnfermedadesCronicas,
                    Antecedentes = dto.Antecedentes,
                    Detalles = dto.Detalles,
                    FechaDeCreacion = DateTime.Now
                };

                historial = await _historialRepositorio
                    .CreateHistorialAsync(historial);

                // Crear Paciente
                var paciente = new DAL.Entidades.Paciente
                {
                    IdUsuario = usuario.IdUsuario,
                    IdHistorial = historial.IdHistorial
                };

                return await _pacienteRepositorio
                    .CreatePacienteAsync(paciente);
            }
            catch
            {
                return false;
            }
        }
    }
}