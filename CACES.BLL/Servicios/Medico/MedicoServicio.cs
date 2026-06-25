using AutoMapper;
using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Medico;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.Medicos;
using CACES.DAL.Repositorios.Usuario;
using Microsoft.Extensions.Logging;


namespace CACES.BLL.Servicios.Medicos
{
    [Serializable]
    public class MedicoServicio : IMedicoServicio
    {
        private readonly IMedicoRepositorio _medicoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IMapper _mapper;
        private readonly ILogger<MedicoServicio> _logger;

        public MedicoServicio(IMedicoRepositorio medicoRepositorio, IUsuarioRepositorio usuarioRepositorio, IMapper mapper, ILogger<MedicoServicio> logger)
        {
            _medicoRepositorio = medicoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<respuestaErrores<List<MedicoDTO>>> GetMedicosAsync()
        {
            var medicos = await _medicoRepositorio.GetMedicosAsync();
            var medicosDTO = _mapper.Map<List<MedicoDTO>>(medicos);

            return new respuestaErrores<List<MedicoDTO>>
            {
                EsCorrecto = true,
                mensaje = "Médicos obtenidos exitosamente.",
                Dato = medicosDTO,
                codigo = 200
            };
        }
        
        public async Task<respuestaErrores<MedicoDTO>> GetMedicoPorIdAsync(int id)
        {
            var medico = await _medicoRepositorio.GetMedicoConUsuarioByIdAsync(id);
            if (medico == null)
                return new respuestaErrores<MedicoDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Médico no encontrado.",
                    codigo = 404
                };
            return new respuestaErrores<MedicoDTO>
            {
                EsCorrecto = true,
                mensaje = "Médico obtenido exitosamente.",
                Dato = _mapper.Map<MedicoDTO>(medico),
                codigo = 200
            };
        }


        
        //Este es el metodo que trae los datos del medico para ser editados, 
        public async Task<respuestaErrores<EditarMedicoDTO>> GetMedicoParaEditarAsync(int id)
        {
            var medico = await _medicoRepositorio.GetMedicoConUsuarioByIdAsync(id);

            if (medico == null)
                return new respuestaErrores<EditarMedicoDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Médico no encontrado.",
                    codigo = 404
                };

            
            return new respuestaErrores<EditarMedicoDTO>
            {
                EsCorrecto = true,
                mensaje = "Médico obtenido exitosamente.",
                Dato = new EditarMedicoDTO
                {
                    IdMedico = medico.IdMedico,
                    IdUsuario = medico.IdUsuario,
                    Nombres = medico.Usuario.Nombres,
                    PrimerApellido = medico.Usuario.PrimerApellido,
                    SegundoApellido = medico.Usuario.SegundoApellido,
                    CorreoElectronico = medico.Usuario.CorreoElectronico,
                    DUI = medico.Usuario.DUI,
                    Direccion = medico.Usuario.Direccion,
                    Nacimiento = medico.Usuario.Nacimiento,
                    Telefono = medico.Usuario.Telefono,
                    Foto = medico.Usuario.Foto,
                    Estado = medico.Usuario.Estado == true,
                    IdEspecialidad = medico.IdEspecialidad,
                    Experiencia = medico.Experiencia,
                    Certificaciones = medico.Certificaciones
                },
                codigo = 200

            };
        
        }

        public async Task<respuestaErrores<MedicoDTO>> UpdateMedicoConUsuarioAsync(EditarMedicoDTO dto)
        {
            var medico = await _medicoRepositorio.GetMedicoConUsuarioByIdAsync(dto.IdMedico);
            if (medico == null)
            {
                return new respuestaErrores<MedicoDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Médico no encontrado.",
                    codigo = 404
                };
            }
            //variables del medico
            medico.IdEspecialidad = dto.IdEspecialidad;
            medico.Experiencia = dto.Experiencia;
            medico.Certificaciones = dto.Certificaciones;

            //variables del usuario
            medico.Usuario.Nombres = dto.Nombres;
            medico.Usuario.PrimerApellido = dto.PrimerApellido;
            medico.Usuario.SegundoApellido = dto.SegundoApellido;
            medico.Usuario.CorreoElectronico = dto.CorreoElectronico;
            medico.Usuario.DUI = dto.DUI;
            medico.Usuario.Telefono = dto.Telefono;
            if (!string.IsNullOrEmpty(dto.Foto))
            {
                medico.Usuario.Foto = dto.Foto;
            }
            medico.Usuario.Estado = dto.Estado;
            medico.Usuario.FechaDeModificacion = DateTime.UtcNow;

            bool resultado = await _medicoRepositorio.UpdateMedicoConUsuarioAsync(medico);
            if ( resultado)
            {
                return new respuestaErrores<MedicoDTO>
                {
                    EsCorrecto = true,
                    mensaje = "Médico actualizado exitosamente.",
                    Dato = _mapper.Map<MedicoDTO>(medico),
                    codigo = 200
                };
            }
            else
            {
                return new respuestaErrores<MedicoDTO>
                {
                    EsCorrecto = false,
                    mensaje = "No se pudo actualizar al médico en el sistema.",
                    codigo = 500
                };
            }
        }

        public async Task<respuestaErrores<MedicoDTO>> CreateMedicoAsync(RegistrarMedicoDTO dto)
        {
            try
            {
                var validacion = ValidarContraseña(dto.Password);
                if (!validacion.IsValid)
                    return new respuestaErrores<MedicoDTO> { EsCorrecto = false, mensaje = validacion.Message };

                if (await _usuarioRepositorio.GetUsuarioByEmailAsync(dto.CorreoElectronico) != null)
                    return new respuestaErrores<MedicoDTO> { EsCorrecto = false, mensaje = "El correo ya está registrado" };

                if (await _usuarioRepositorio.GetUsuarioByDUIAsync(dto.DUI) != null)
                    return new respuestaErrores<MedicoDTO> { EsCorrecto = false, mensaje = "El DUI ya está registrado" };

                
            var usuario = new DAL.Entidades.Usuario
            {
                Nombres = dto.Nombres,
                PrimerApellido = dto.PrimerApellido,
                SegundoApellido = dto.SegundoApellido,
                CorreoElectronico = dto.CorreoElectronico,
                DUI = dto.DUI,
                Telefono = dto.Telefono,
                Direccion = dto.Direccion,
                Nacimiento = dto.Nacimiento,
                Edad = DateTime.UtcNow.Year - dto.Nacimiento.Year,
                PasswordHash = HashContraseña(dto.Password),
                SecurityStamp = Guid.NewGuid().ToString(),
                Foto = string.IsNullOrWhiteSpace(dto.Foto) ? "default.jpg" : dto.Foto,
                Estado = true,
                FechaDeRegistro = DateTime.UtcNow,
                EmailConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = false,
                AccessFailedCount = 0
            };

                var medico = new Medico
                {
                    IdEspecialidad = dto.IdEspecialidad,
                    Experiencia = dto.Experiencia,
                    Certificaciones = dto.Certificaciones,
                    FechaDeRegistro = DateTime.UtcNow
                };

                bool resultado = await _medicoRepositorio.CreateMedicoConUsuarioAsync(usuario, medico);

                if (!resultado)
                    return new respuestaErrores<MedicoDTO> { EsCorrecto = false, mensaje = "No se pudo registrar el médico.", codigo = 500 };

                return new respuestaErrores<MedicoDTO>
                {
                    EsCorrecto = true,
                    mensaje = "Médico registrado exitosamente."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear médico");
                return new respuestaErrores<MedicoDTO> { EsCorrecto = false, mensaje = "Ocurrió un error al procesar la solicitud.", codigo = 500 };
            }
        }
        private string HashContraseña(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private (bool IsValid, string Message) ValidarContraseña(string password)
        {
            if (string.IsNullOrEmpty(password)) return (false, "La contraseña es requerida");
            if (password.Length < 8) return (false, "La contraseña debe tener mínimo 8 caracteres");
            if (!password.Any(char.IsDigit)) return (false, "La contraseña debe contener al menos un número");
            if (!password.Any(c => !char.IsLetterOrDigit(c))) return (false, "La contraseña debe contener al menos un carácter especial");
            return (true, "Contraseña válida");

        }


        public async Task<respuestaErrores<MedicoDTO>> DesactivarMedicoAsync(int id)
        {
            var medico = await _medicoRepositorio.GetMedicoConUsuarioByIdAsync(id);
            if (medico == null)
            {
                return new respuestaErrores<MedicoDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Médico no encontrado.",
                    codigo = 404
                };
            }

            bool resultado = await _medicoRepositorio.DesactivarMedicoAsync(id);
            if (resultado)
            {
                return new respuestaErrores<MedicoDTO>
                {
                    EsCorrecto = true,
                    mensaje = "Médico desactivado exitosamente.",
                    codigo = 200
                };
            }
            else
            {
                return new respuestaErrores<MedicoDTO>
                {
                    EsCorrecto = false,
                    mensaje = "No se pudo desactivar al médico en el sistema.",
                    codigo = 500
                };
            }
        }

        public async Task<respuestaErrores<List<MedicoDTO>>> GetEspecialistasActivosAsync()
        {
            var medicos = await _medicoRepositorio.GetEspecialistasActivosAsync();
            var medicosDTO = _mapper.Map<List<MedicoDTO>>(medicos);
            return new respuestaErrores<List<MedicoDTO>>
            {
                EsCorrecto = true,
                mensaje = "Médicos obtenidos exitosamente.",
                Dato = medicosDTO,
                codigo = 200
            };
        }
    }
}