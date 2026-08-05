using CACES.BLL.DTOs.Cirugia;
using CACES.BLL.DTOs.Cita;
using CACES.BLL.DTOs.Configuracion;
using CACES.BLL.DTOs.Convenios;
using CACES.BLL.DTOs.Especialidad;
using CACES.BLL.DTOs.Horario;
using CACES.BLL.DTOs.Icono;
using CACES.BLL.DTOs.Medico;
using CACES.BLL.DTOs.Notificacion;
using CACES.BLL.DTOs.Paciente;
using CACES.BLL.DTOs.Paquete;
using CACES.BLL.DTOs.Perfil;
using CACES.BLL.DTOs.Precio;
using CACES.BLL.DTOs.Procedimientos;
using CACES.BLL.DTOs.Receta;
using CACES.BLL.DTOs.SeguimientoPostOperatorio;
using CACES.BLL.DTOs.Usuario;
using CACES.DAL.Entidades;
using CACES.DAL.Entidades.Configuración;
using CACES.DAL.Entidades.SeguimientoPostOperatorio;


namespace CACES.BLL.Mappers
{
    public static class DtoMapper
    {
        // ===== Usuario =====
        public static MostrarUsuarioDTO? ToMostrarUsuarioDTO(this Usuario src) => src == null ? null : new MostrarUsuarioDTO
        {
            idUsuario = src.IdUsuario,
            Nombres = src.Nombres,
            PrimerApellido = src.PrimerApellido,
            SegundoApellido = src.SegundoApellido,
            CorreoElectronico = src.CorreoElectronico,
            Telefono = src.Telefono,
            Direccion = src.Direccion,
            Nacimiento = src.Nacimiento,
            Estado = src.Estado,
            DUI = src.DUI,
            Foto = src.Foto
        };

        public static Usuario? ToUsuario(this RegistrarUsuarioDTO src) => src == null ? null : new Usuario
        {
            Nombres = src.Nombres,
            PrimerApellido = src.PrimerApellido,
            SegundoApellido = src.SegundoApellido,
            CorreoElectronico = src.CorreoElectronico,
            Telefono = src.Telefono,
            DUI = src.DUI,
            Direccion = src.Direccion,
            Nacimiento = src.Nacimiento,
            Estado = true,
        };

        public static Usuario? ToUsuario(this ActualizarUsuarioDTO src) => src == null ? null : new Usuario
        {

            Nombres = src.Nombres,
            PrimerApellido = src.PrimerApellido,
            SegundoApellido = src.SegundoApellido,
            CorreoElectronico = src.CorreoElectronico,
            Telefono = src.Telefono,
            Estado = src.Estado,
            Direccion = src.Direccion,
            Nacimiento = src.Nacimiento,
            Foto = src.Foto
        };

        // ===== Cita =====
        public static MostrarCitaDTO? ToMostrarCitaDTO(this Cita src) => src == null ? null : new MostrarCitaDTO
        {
            IdCita = src.IdCita,
            IdPaciente = src.IdPaciente,
            IdMedico = src.IdMedico,
            IdEspecialidad = src.IdEspecialidad,
            IdHorario = src.IdHorario,
            NombrePaciente = $"{src.Paciente?.Usuario?.Nombres} {src.Paciente?.Usuario?.PrimerApellido}",
            NombreMedico = $"{src.Medico?.Usuario?.Nombres} {src.Medico?.Usuario?.PrimerApellido}",
            NombreEspecialidad = src.Especialidad?.Nombre,
            FechaCita = src.Fecha,
            Hora = src.Horario?.HoraInicio ?? TimeSpan.Zero,
            Motivo = src.Motivo,
            Estado = src.Estado,
            IdReceta = null,
            IdProcedimiento = src.IdProcedimiento,
            NombreProcedimiento = src.Procedimiento?.Nombre ?? "N/A"
        };

        public static RegistrarCitaDTO? ToRegistrarCitaDTO(this Cita src) => src == null ? null : new RegistrarCitaDTO
        {
            IdPaciente = src.IdPaciente,
            IdMedico = src.IdMedico,
            IdEspecialidad = src.IdEspecialidad,
            IdHorario = src.IdHorario,
            FechaCita = src.Fecha,
            Hora = src.Horario?.HoraInicio ?? TimeSpan.Zero,
            Motivo = src.Motivo,
            IdProcedimiento = src.IdProcedimiento ?? 0
        };

        public static Cita? ToCita(this RegistrarCitaDTO src) => src == null ? null : new Cita
        {
            IdPaciente = src.IdPaciente,
            IdMedico = src.IdMedico,
            IdEspecialidad = src.IdEspecialidad,
            IdHorario = src.IdHorario,
            Fecha = src.FechaCita,
            Motivo = src.Motivo,
            Estado = 1,
            FechaDeRegistro = DateTime.Now,
            IdProcedimiento = src.IdProcedimiento
        };

        // ===== Especialidad =====
        public static mostrarEspecialidadDTO? ToMostrarEspecialidadDTO(this Especialidad src) => src == null ? null :
            new mostrarEspecialidadDTO
            {
                IdEspecialidad = src.IdEspecialidad,
                Nombre = src.Nombre,
                Descripcion = src.Descripcion,
                IdIcono = src.IdIcono,
                NombreIcono = src.Icono?.Codigo
            };

        public static mostrarDetalleEspecialidadDTO? ToMostrarDetalleEspecialidadDTO(this Especialidad src) => src == null ? null :
            new mostrarDetalleEspecialidadDTO
            {
                IdEspecialidad = src.IdEspecialidad,
                Nombre = src.Nombre,
                Descripcion = src.Descripcion,
                IdIcono = src.IdIcono,
                NombreIcono = src.Icono?.Codigo,
                Procedimientos = src.Procedimientos?.Select(p => p.ToMostrarProcedimientosDTO()).ToList(),
                Medicos = src.Medicos?.Select(m => m.ToMostrarMedicoEspecialidadDTO()).ToList() ?? new List<mostrarMedicoEspecialidadDTO>()
            };

        public static especialidadDTO? ToEspecialidadDTO(this Especialidad src) => src == null ? null :
            new especialidadDTO
            {
                IdEspecialidad = src.IdEspecialidad,
                Nombre = src.Nombre,
                Descripcion = src.Descripcion,
                IdIcono = src.IdIcono,
                NombreIcono = src.Icono?.Codigo,
                Estado = src.Estado,
                FechaDeRegistro = src.FechaDeRegistro
            };

        public static Especialidad? ToEspecialidad(this especialidadDTO src) => src == null ? null : new Especialidad
        {
            IdEspecialidad = src.IdEspecialidad,
            Nombre = src.Nombre,
            Descripcion = src.Descripcion,
            IdIcono = src.IdIcono,
            Estado = src.Estado,
            FechaDeRegistro = src.FechaDeRegistro
        };

        // ===== Medico =====
        public static mostrarMedicoEspecialidadDTO? ToMostrarMedicoEspecialidadDTO(this Medico src) => src == null ? null :
            new mostrarMedicoEspecialidadDTO
            {
                IdMedico = src.IdMedico,
                Nombres = src.Usuario?.Nombres,
                PrimerApellido = src.Usuario?.PrimerApellido,
                SegundoApellido = src.Usuario?.SegundoApellido,
                Telefono = src.Usuario?.Telefono,
                Foto = src.Usuario?.Foto

            };

        public static MedicoDTO? ToMedicoDTO(this Medico src) => src == null ? null : new MedicoDTO
        {
            IdMedico = src.IdMedico,
            Experiencia = src.Experiencia,
            Certificaciones = src.Certificaciones,
            NombreEspecialidad = src.Especialidad?.Nombre,
            Usuario = src.Usuario?.ToMostrarUsuarioDTO()
        };

        public static RegistrarMedicoDTO? ToRegistrarMedicoDTO(this Medico src) => src == null ? null : new RegistrarMedicoDTO
        {
            IdEspecialidad = src.IdEspecialidad,
            Nombres = src.Usuario?.Nombres,
            PrimerApellido = src.Usuario?.PrimerApellido,
            SegundoApellido = src.Usuario?.SegundoApellido,
            CorreoElectronico = src.Usuario?.CorreoElectronico,
            Telefono = src.Usuario?.Telefono,
            DUI = src.Usuario?.DUI,
            Password = src.Usuario?.PasswordHash,
            Direccion = src.Usuario?.Direccion,
            Nacimiento = src.Usuario?.Nacimiento ?? DateTime.MinValue,
            Foto = src.Usuario?.Foto,
            Experiencia = src.Experiencia,
            Certificaciones = src.Certificaciones
        };

        public static Medico? ToMedico(this RegistrarMedicoDTO src) => src == null ? null : new Medico
        {
            IdEspecialidad = src.IdEspecialidad,
            Experiencia = src.Experiencia,
            Certificaciones = src.Certificaciones,
            FechaDeRegistro = DateTime.UtcNow
        };

        public static EditarMedicoDTO? ToEditarMedicoDTO(this Medico src) => src == null ? null : new EditarMedicoDTO
        {
            IdMedico = src.IdMedico,
            IdEspecialidad = src.IdEspecialidad,
            IdUsuario = src.IdUsuario,
            Nombres = src.Usuario?.Nombres,
            PrimerApellido = src.Usuario?.PrimerApellido,
            SegundoApellido = src.Usuario?.SegundoApellido,
            CorreoElectronico = src.Usuario?.CorreoElectronico,
            Telefono = src.Usuario?.Telefono,
            DUI = src.Usuario?.DUI,
            Direccion = src.Usuario?.Direccion,
            Nacimiento = src.Usuario?.Nacimiento ?? DateTime.MinValue,
            Foto = src.Usuario?.Foto,
            Estado = src.Usuario?.Estado ?? false,
            Experiencia = src.Experiencia,
            Certificaciones = src.Certificaciones


        };

        public static void UpdateFromEditarMedicoDTO(this Medico dest, EditarMedicoDTO src)
        {
            if (src == null) return;

            dest.IdEspecialidad = src.IdEspecialidad;
            dest.Experiencia = src.Experiencia;
            dest.Certificaciones = src.Certificaciones ?? dest.Certificaciones;

            // Actualizar Usuario si existe
            if (dest.Usuario != null)
            {
                dest.Usuario.Nombres = src.Nombres ?? dest.Usuario.Nombres;
                dest.Usuario.PrimerApellido = src.PrimerApellido ?? dest.Usuario.PrimerApellido;
                dest.Usuario.SegundoApellido = src.SegundoApellido ?? dest.Usuario.SegundoApellido;
                dest.Usuario.CorreoElectronico = src.CorreoElectronico ?? dest.Usuario.CorreoElectronico;
                dest.Usuario.Telefono = src.Telefono ?? dest.Usuario.Telefono;
                dest.Usuario.DUI = src.DUI ?? dest.Usuario.DUI;
                dest.Usuario.Direccion = src.Direccion ?? dest.Usuario.Direccion;
                if (src.Nacimiento != DateTime.MinValue)
                    dest.Usuario.Nacimiento = src.Nacimiento;
                if (!string.IsNullOrEmpty(src.Foto))
                    dest.Usuario.Foto = src.Foto;
                dest.Usuario.Estado = src.Estado;
            }
        }

        // ===== Paciente =====
        public static Paciente? ToPaciente(this RegistrarPacienteDTO src) => src == null ? null : new Paciente
        {
            IdUsuario = src.Usuario?.IdUsuario ?? 0,
            IdHistorial = src.Historial?.IdHistorial ?? 0
        };

        public static MostrarPacienteDTO? ToMostrarPacienteDTO(this Paciente paciente) => paciente == null ? null : new MostrarPacienteDTO
        {

            IdPaciente = paciente.IdPaciente,
            IdHistorial = paciente.IdHistorial,
            Usuario = paciente.Usuario.ToMostrarUsuarioDTO()

        };



        // ===== Perfil =====
        public static PerfilUsuarioDTO? ToPerfilUsuarioDTO(this Usuario src)
        {
            if (src == null) return null;

            var paciente = src.Paciente;
            return new PerfilUsuarioDTO
            {
                IdUsuario = src.IdUsuario,
                Nombres = src.Nombres,
                PrimerApellido = src.PrimerApellido,
                SegundoApellido = src.SegundoApellido,
                CorreoElectronico = src.CorreoElectronico,
                Telefono = src.Telefono,
                Direccion = src.Direccion,
                Nacimiento = src.Nacimiento,
                Estado = src.Estado,
                DUI = src.DUI,
                Foto = src.Foto,
                IdHistorial = paciente?.IdHistorial ?? 0,
                TipoSangre = paciente?.HistorialMedico?.TipoSangre ?? "No asignado",
                Alergias = paciente?.HistorialMedico?.Alergias ?? "Ninguna reportada",
                EnfermedadesCronicas = paciente?.HistorialMedico?.EnfermedadesCronicas ?? "Ninguna registrada",
                MedicamentosActuales = paciente?.Citas?.OrderByDescending(c => c.IdCita).FirstOrDefault()?.Receta?.Medicamentos ?? "Sin medicamentos prescritos"
            };
        }


        public static ActualizarPerfilDTO? ToActualizarPerfilDTO(this Usuario src) => src == null ? null : new ActualizarPerfilDTO
        {
            IdUsuario = src.IdUsuario,
            Nombres = src.Nombres,
            PrimerApellido = src.PrimerApellido,
            SegundoApellido = src.SegundoApellido,
            CorreoElectronico = src.CorreoElectronico,
            Telefono = src.Telefono,
            DUI = src.DUI,
            Direccion = src.Direccion,
            Foto = src.Foto
        };

        public static void UpdateFromActualizarPerfilDTO(this Usuario dest, ActualizarPerfilDTO src)
        {
            if (src == null) return;

            dest.Nombres = src.Nombres ?? dest.Nombres;
            dest.PrimerApellido = src.PrimerApellido ?? dest.PrimerApellido;
            dest.SegundoApellido = src.SegundoApellido ?? dest.SegundoApellido;
            dest.CorreoElectronico = src.CorreoElectronico ?? dest.CorreoElectronico;
            dest.Telefono = src.Telefono ?? dest.Telefono;
            dest.DUI = src.DUI ?? dest.DUI;
            dest.Estado = src.Estado;
            dest.Direccion = src.Direccion ?? dest.Direccion;
            dest.Foto = src.Foto ?? dest.Foto;
        }

        // ===== Paquete =====
        public static PaqueteDTO? ToPaqueteDTO(this Paquete src) => src == null ? null : new PaqueteDTO
        {
            IdPaquete = src.IdPaquete,
            Nombre = src.Nombre,
            Descripcion = src.Descripcion,
            Duracion = src.Duracion,
            Precio = src.Precio,
            Estado = src.Estado
        };

        public static Paquete? ToPaquete(this PaqueteDTO src) => src == null ? null : new Paquete
        {
            IdPaquete = src.IdPaquete,
            Nombre = src.Nombre,
            Descripcion = src.Descripcion,
            Duracion = src.Duracion,
            Precio = src.Precio,
            Estado = src.Estado,
            FechaDeRegistro = DateTime.UtcNow
        };

        public static void UpdateFromPaqueteDTO(this Paquete dest, PaqueteDTO src)
        {
            if (src == null) return;

            dest.Nombre = src.Nombre ?? dest.Nombre;
            dest.Descripcion = src.Descripcion ?? dest.Descripcion;
            dest.Duracion = src.Duracion ?? dest.Duracion;
            dest.Precio = src.Precio;
            dest.Estado = src.Estado;
        }

        // ===== Procedimiento =====
        

        public static InsertarProcedimientosDto? ToInsertarProcedimientosDto(this Procedimiento src) => src == null ? null :
            new InsertarProcedimientosDto
            {
                Id_Procedimiento = src.Id_Procedimiento,
                Id_Especialidad = src.Id_Especialidad,
                NombreEspecialidad = src.Especialidad?.Nombre,
                Nombre = src.Nombre,
                Descripcion = src.Descripcion,
                PrecioBase = src.PrecioBase,
                Estado = src.Estado
            };

        public static Procedimiento? ToProcedimiento(this InsertarProcedimientosDto src) => src == null ? null :
            new Procedimiento
            {
                Id_Procedimiento = src.Id_Procedimiento,
                Nombre = src.Nombre,
                Descripcion = src.Descripcion,
                PrecioBase = src.PrecioBase,
                Id_Especialidad = src.Id_Especialidad,
                Estado = src.Estado
            };


        public static MostrarProcedimientosDTO? ToMostrarProcedimientosDTO(this Procedimiento src) => src == null ? null :
            new MostrarProcedimientosDTO
            {
                
                Nombre = src.Nombre,
                NombreMedico = $"{src.Cirugias?.FirstOrDefault()?.Medico?.Usuario?.Nombres} {src.Cirugias?.FirstOrDefault()?.Medico?.Usuario?.PrimerApellido}",
                Fecha = src.Cirugias?.FirstOrDefault()?.Cita?.Fecha ?? DateTime.MinValue,
                Descripcion = src.Descripcion,
                PrecioBase = src.PrecioBase,
                Estado = src.Estado
            };
        public static ProcedimientoDTO? ToProcedimientoDTO(this Procedimiento? src) => src == null ? null :
            new ProcedimientoDTO
            {
                Id_Procedimiento = src.Id_Procedimiento,
                Nombre = src.Nombre,
                PrecioBase = src.PrecioBase,
                Estado = src.Estado
            };

        //======Cirugia======
        public static MostrarCirugiaDTO? ToMostrarCirugiaDTO(this Cirugias? src) => src == null ? null :
            new MostrarCirugiaDTO
            {
                idCirugia = src.Id_Cirugia,
                NombrePaciente = src.Paciente.Usuario.Nombres,
                Procedimiento = src.Procedimiento.Nombre,
                FechaProcedimiento = src.Cita.Fecha,
                HoraProcedimiento = src.Horario.HoraInicio,
                Estado = src.Estado,
                MedicoResponsable = $"{src.Medico.Usuario.Nombres} {src.Medico.Usuario.PrimerApellido} {src.Medico.Usuario.SegundoApellido}".Trim(),

            };

        public static CirugiaDTO? ToCirugiaDTO(this Cirugias? src) => src == null ? null :
            new CirugiaDTO
            {
                Id_Cirugia = src.Id_Cirugia,
                Paciente = src.Id_Paciente,
                Procedimiento = src.Id_Procedimiento,
                id_cita = src.Id_Cita,
                idhorario = src.Id_Horario,
                Estado = src.Estado,
                Medico = src.Id_Medico

            };

        public static Cirugias? ToCirugia(this CirugiaDTO? src) => src == null ? null : 
            new Cirugias
            {
                Id_Cirugia = src.Id_Cirugia,
                Id_Paciente = src.Paciente,
                Id_Procedimiento = src.Procedimiento,
                Id_Cita = src.id_cita,
                Id_Horario = src.idhorario,
                Estado = src.Estado,
                Id_Medico = src.Medico
            };

        // ===== Receta =====
        public static MostrarRecetaDTO? ToMostrarRecetaDTO(this Receta src) => src == null ? null : new MostrarRecetaDTO
        {
            IdReceta = src.IdReceta,
            IdCita = src.IdCita,
            NombrePaciente = $"{src.Cita?.Paciente?.Usuario?.Nombres} {src.Cita?.Paciente?.Usuario?.PrimerApellido}",
            NombreMedico = $"{src.Cita?.Medico?.Usuario?.Nombres} {src.Cita?.Medico?.Usuario?.PrimerApellido}",
            NombreEspecialidad = src.Cita?.Especialidad?.Nombre,
            Medicamentos = src.Medicamentos,
            Instrucciones = src.Instrucciones,
            FechaDeRegistro = src.FechaDeRegistro,
            FechaDeVencimiento = src.FechaDeVencimiento
        };

        // ===== Horario =====

        public static MostrarHorarioDTO? ToMostrarHorarioDTO(this HorariosDisponibles src) => src == null ? null :
            new MostrarHorarioDTO
            {
                Id_Horario = src.Id_Horario,
                DiaSemana = src.DiaSemana,
                HoraInicio = src.HoraInicio,
                Estado = src.Estado

            };

        public static HorariosDisponibles? ToHorariosDisponibles(this RegistrarHorarioDTO src) => src == null ? null :
     new HorariosDisponibles
     {
         Id_Medico = src.Id_Medico,
         HoraInicio = src.HoraInicio,
         DiaSemana = src.DiaSemana,
         Estado = src.Estado,
     };
        public static HorariosDisponibles? ToHorariosDisponibles(this EditarHorarioDTO src) => src == null ? null :
            new HorariosDisponibles
            {
                Id_Medico = src.Id_Medico,
                HoraInicio = src.HoraInicio,
                DiaSemana = src.DiaSemana,
                Estado = src.Estado
            };


        // ===== Icono =====
        public static IconoDTO? ToIconoDTO(this Icono src) => src == null ? null : new IconoDTO
        {
            IdIcono = src.IdIcono,
            Codigo = src.Codigo,
            Nombre = src.Nombre
        };

        public static Icono? ToIcono(this IconoDTO src) => src == null ? null : new Icono
        {
            IdIcono = src.IdIcono,
            Codigo = src.Codigo,
            Nombre = src.Nombre
        };
        // ===== ConfiguracionCheckpoints =====

        public static ConfiguracionCheckPointDTO? ToConfiguracionCheckpointDTO(this ConfiguracionCheckpoints src) => src == null ? null : new ConfiguracionCheckPointDTO
        {
            IdCheckPoint = src.IdCheckPoint,
            DiaCheckPoint = src.DiaCheckPoint,
            Estado = src.Estado
        };

        public static ConfiguracionCheckpoints? ToConfiguracionCheckpoints(this RegistrarConfiguracionCheckpointDTO src) => src == null ? null : new ConfiguracionCheckpoints
        {
            DiaCheckPoint = src.DiaCheckpoint,
            Estado = true
        };
        public static ConfiguracionCheckpoints? ToConfiguracionCheckpoints(this ConfiguracionCheckPointDTO src) => src == null ? null : new ConfiguracionCheckpoints
        {
            IdCheckPoint = src.IdCheckPoint,
            DiaCheckPoint = src.DiaCheckPoint,
            Estado = src.Estado

        };


        // ===== PreguntaSeguimiento =====

        public static PreguntasPOpDTO? ToPreguntaSeguimientoDTO(this PreguntaSeguimiento src) => src == null ? null : new PreguntasPOpDTO
        {
            idPregunta = src.IdPregunta,
            Texto = src.Texto,
            ValorMinimo = src.ValorMinimo,
            ValorMaximo = src.ValorMaximo,
            UmbralAlerta = src.UmbralAlerta,
            DireccionAlerta = src.DireccionAlerta.ToString(),
            Estado = src.Estado
        };

        public static PreguntaSeguimiento? ToPreguntaSeguimiento(this RegistrarPreguntasPOpDTO src) => src == null ? null : new PreguntaSeguimiento
        {
            Texto = src.Texto,
            ValorMinimo = src.ValorMinimo,
            ValorMaximo = src.ValorMaximo,
            UmbralAlerta = src.UmbralAlerta,
            DireccionAlerta = src.DireccionAlerta,
            Estado = true
        };

        public static PreguntaSeguimiento? ToPreguntaSeguimiento(this PreguntasPOpDTO src) => src == null ? null : new PreguntaSeguimiento
        {
            IdPregunta = src.idPregunta,
            Texto = src.Texto,
            ValorMinimo = src.ValorMinimo,
            ValorMaximo = src.ValorMaximo,
            UmbralAlerta = src.UmbralAlerta,
            DireccionAlerta = Enum.Parse<DireccionAlerta>(src.DireccionAlerta),
            Estado = src.Estado
        };

        public static MostrarSeguimientoPacienteDTO? ToMostrarSeguimientoPacienteDTO(this SeguimientoPaciente src) => src == null ? null : new MostrarSeguimientoPacienteDTO
        {
            IdSeguimiento = src.Id_Seguimiento,
            IdCirugia = src.Id_Cirugia,
            DiaCheckpoint = src.DiaCheckpoint,
            FechaProgramada = src.FechaProgramada,
            Estado = src.Estado.ToString(),
            FechaRegistro = src.FechaRegistro
        };

        // ===== AlertaStaff =====
        public static AlertaStaffDTO? ToMostrarAlertaStaffDTO(this AlertaStaff src) => src == null ? null : new AlertaStaffDTO
        {
            idAlerta = src.IdAlerta,
            IdSeguimiento = src.IdSeguimiento,
            IdCirugia = src.SeguimientoPaciente?.Id_Cirugia ?? 0,
            FechaGenerada = src.FechaGenerada,
            Estado = src.Estado.ToString(),
            NombreUsuarioAtendio = src.UsuarioAtendio?.Nombres,
            Observaciones = src.Observaciones,
            FechaAtencion = src.FechaAtencion
        };

        // ===== RespuestaSeguimiento =====
        public static MostrarRespuestaSeguimientoDTO? ToMostrarRespuestaSeguimientoDTO(this RespuestaSeguimiento src) => src == null ? null : new MostrarRespuestaSeguimientoDTO
        {
            idRespuesta = src.IdRespuesta,
            IdSeguimiento = src.IdSeguimiento,
            IdPregunta = src.IdPregunta,
            TextoPregunta = src.PreguntaSeguimiento?.Texto ?? string.Empty,
            ValorRespuesta = src.ValorRespuesta,
            GeneroAlerta = src.GeneroAlerta
        };

        public static RespuestaSeguimiento? ToRespuestaSeguimiento(this RegistrarRespuestaSeguimientoDTO src) => src == null ? null : new RespuestaSeguimiento
        {
            IdSeguimiento = src.IdSeguimiento,
            IdPregunta = src.IdPregunta,
            ValorRespuesta = src.ValorRespuesta,
            GeneroAlerta = false
        };
        // ===== Notificacion =====
        public static NotificacionDTO? ToDTO(this Notificacion entidad) => entidad == null ? null :
    new NotificacionDTO
    {
        Id_Notificacion = entidad.Id_Notificacion,
        Evento = entidad.Evento,
        CanalPlataforma = entidad.CanalPlataforma,
        CanalEmail = entidad.CanalEmail,
        Estado = entidad.Estado
    };


        public static Notificacion? ToEntity(this NotificacionDTO dto) => dto == null ? null : new Notificacion
        {

            Id_Notificacion = dto.Id_Notificacion,
            Evento = dto.Evento,
            CanalPlataforma = dto.CanalPlataforma,
            CanalEmail = dto.CanalEmail,
            Estado = dto.Estado

        };

        // ===== Configuracion =====
        public static ConfiguracionDTO? ToDTO(this Configuracion entidad) => entidad == null ? null :
    new ConfiguracionDTO
    {
        IdConfiguracion = entidad.IdConfiguracion,
        Clave = entidad.Clave,
        Valor = entidad.Valor,
        Tipo = entidad.Tipo,
        Categoria = entidad.Categoria,
        Descripcion = entidad.Descripcion
    };

        public static Configuracion? ToEntity(this ConfiguracionDTO dto) => dto == null ? null :
    new Configuracion
    {
        IdConfiguracion = dto.IdConfiguracion,
        Clave = dto.Clave,
        Valor = dto.Valor,
        Tipo = dto.Tipo,
        Categoria = dto.Categoria,
        Descripcion = dto.Descripcion
    };

        public static NotificacionUsuarioDTO? ToDTO(this NotificacionUsuario entidad) => entidad == null ? null : new NotificacionUsuarioDTO

        {
            IdNotificacionUsuario = entidad.IdNotificacionUsuario,
            IdUsuario = entidad.IdUsuario,
            Evento = entidad.Evento,
            Titulo = entidad.Titulo,
            Mensaje = entidad.Mensaje,
            Leido = entidad.Leido,
            FechaCreacion = entidad.FechaCreacion
        };



        public static NotificacionUsuario? ToEntity(this NotificacionUsuarioDTO dto) => dto == null ? null : new NotificacionUsuario
        {

            IdUsuario = dto.IdUsuario,
            Evento = dto.Evento,
            Titulo = dto.Titulo,
            Mensaje = dto.Mensaje,
            Leido = false,
            FechaCreacion = DateTime.UtcNow

        };

        // ===== Convenio =====
        public static MostrarConvenios? ToConvenioDTO(this Convenios src) => src == null ? null : new MostrarConvenios
        { 

            Id = src.Id,
            Nombre = src.Nombre,
            Descripcion = src.Descripcion,
            DescuentoPorcentaje = src.DescuentoPorcentaje,
            ContactoTelefono = src.ContactoTelefono,
            ImagenUrl = src.ImagenUrl,
            Estado = src.Estado,

            
    };

    public static Convenios? ToConvenio(this CrearModificarConvenio src) => src == null ? null : new Convenios
        {
            Nombre = src.Nombre,
            Descripcion = src.Descripcion,
            DescuentoPorcentaje = src.DescuentoPorcentaje,
            ContactoTelefono = src.ContactoTelefono,
            ImagenUrl = src.ImagenUrl,
            Estado = true,
            FechaCreacion = DateTime.Now
        };

        public static void UpdateFromActualizarConvenioDTO(this Convenios dest, CrearModificarConvenio src)
        {
            if (src == null || dest == null) return;

            dest.Nombre = src.Nombre ?? dest.Nombre;
            dest.Descripcion = src.Descripcion ?? dest.Descripcion;
            dest.DescuentoPorcentaje = src.DescuentoPorcentaje;
            dest.ContactoTelefono = src.ContactoTelefono ?? dest.ContactoTelefono;
            dest.ImagenUrl = src.ImagenUrl ?? dest.ImagenUrl;
            dest.Estado = src.Estado;
        }


        // ===== Precios =====
        public static MostrarPrecioDTO? ToMostrarPrecioDTO(this Precios src) => src == null ? null : new MostrarPrecioDTO
        {
            IdPrecio = src.Id_Precio,
            IdMedico = src.Id_Medico,
            NombreMedico = src.Medico?.Usuario != null
                ? $"{src.Medico.Usuario.Nombres} {src.Medico.Usuario.PrimerApellido}"
                : string.Empty,
            IdProcedimiento = src.Id_Procedimiento,
            NombreProcedimiento = src.Procedimiento?.Nombre ?? string.Empty,
            
            Detalles = src.Detalles
        };

        public static Precios? ToPrecio(this RegistrarPrecioDTO src) => src == null ? null : new Precios
        {
            Id_Medico = src.IdMedico,
            Id_Procedimiento = src.IdProcedimiento,
            Costo = src.Costo,
            Detalles = src.Detalles
        };

        public static void UpdateFromEditarPrecioDTO(this Precios dest, EditarPrecioDTO src)
        {
            if (src == null || dest == null) return;

            
            dest.Detalles = src.Detalles ?? dest.Detalles;
        }

    }
}

