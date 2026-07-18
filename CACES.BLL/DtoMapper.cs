using CACES.BLL.DTOs.Cita;
using CACES.BLL.DTOs.Especialidad;
using CACES.BLL.DTOs.Horario;
using CACES.BLL.DTOs.Icono;
using CACES.BLL.DTOs.Medico;
using CACES.BLL.DTOs.Paciente;
using CACES.BLL.DTOs.Perfil;
using CACES.BLL.DTOs.Procedimientos;
using CACES.BLL.DTOs.Receta;
using CACES.BLL.DTOs.Usuario;
using CACES.BLL.DTOs.Paquete;
using CACES.DAL.Entidades;
using System;
using System.Linq;

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
            IdReceta = null
        };

        public static RegistrarCitaDTO? ToRegistrarCitaDTO(this Cita src) => src == null ? null : new RegistrarCitaDTO
        {
            IdPaciente = src.IdPaciente,
            IdMedico = src.IdMedico,
            IdEspecialidad = src.IdEspecialidad,
            IdHorario = src.IdHorario,
            FechaCita = src.Fecha,
            Hora = src.Horario?.HoraInicio ?? TimeSpan.Zero,
            Motivo = src.Motivo
           
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
            FechaDeRegistro = DateTime.Now
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
            FechaDeRegistro = DateTime.Now
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
        public static RegistrarProcedimientosDto? ToRegistrarProcedimientosDto(this Cirugias src) => src == null ? null :
    new RegistrarProcedimientosDto
    {
        IdPaciente = src.Id_Paciente,
        NombrePaciente = $"{src.Paciente?.Usuario?.Nombres} {src.Paciente?.Usuario?.PrimerApellido}",
        IdMedico = src.Id_Medico,
        NombreMedico = $"{src.Medico?.Usuario?.Nombres} {src.Medico?.Usuario?.PrimerApellido}",
        IdCirugia = src.Id_Cirugia,
        NombreCirugia = src.Procedimiento?.Nombre,
        IdCita = src.Id_Cita,
        CitaFechaHora = $"{src.Cita?.Fecha:dd/MM/yyyy} {src.Horario?.HoraInicio}"
    };

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

        public static EditarProcedimientosDTO? ToEditarProcedimientosDTO(this Procedimiento src) => src == null ? null :
            new EditarProcedimientosDTO
            {
                Id_Procedimiento = src.Id_Procedimiento,
                Nombre = src.Nombre,
                Descripcion = src.Descripcion,
                NombreMedico = $"{src.Cirugias?.FirstOrDefault()?.Medico?.Usuario?.Nombres} {src.Cirugias?.FirstOrDefault()?.Medico?.Usuario?.PrimerApellido}",
                NombrePaciente = $"{src.Cirugias?.FirstOrDefault()?.Paciente?.Usuario?.Nombres} {src.Cirugias?.FirstOrDefault()?.Paciente?.Usuario?.PrimerApellido} {src.Cirugias?.FirstOrDefault()?.Paciente?.Usuario?.SegundoApellido}",
                Fecha = src.Cirugias?.FirstOrDefault()?.Cita?.Fecha ?? DateTime.MinValue,
                Estado = src.Estado,
                // PrecioBase = src.PrecioBase,

            };

        public static Procedimiento? ToProcedimiento(this EditarProcedimientosDTO src) => src == null ? null :
            new Procedimiento
            {
                Id_Procedimiento = src.Id_Procedimiento,
                Nombre = src.Nombre,
                Descripcion = src.Descripcion,
               // PrecioBase = src.PrecioBase,
 
            };


        public static MostrarProcedimientosDTO? ToMostrarProcedimientosDTO(this Procedimiento src) => src == null ? null :
            new MostrarProcedimientosDTO
            {
                Id_Cirugia = src.Cirugias?.FirstOrDefault()?.Id_Cirugia ?? 0,
                Id_Paciente = src.Cirugias?.FirstOrDefault()?.Id_Paciente ?? 0,
                Nombre = src.Nombre,
                NombreMedico = $"{src.Cirugias?.FirstOrDefault()?.Medico?.Usuario?.Nombres} {src.Cirugias?.FirstOrDefault()?.Medico?.Usuario?.PrimerApellido}",
                NombrePaciente = $"{src.Cirugias?.FirstOrDefault()?.Paciente?.Usuario?.Nombres} {src.Cirugias?.FirstOrDefault()?.Paciente?.Usuario?.PrimerApellido} {src.Cirugias?.FirstOrDefault()?.Paciente?.Usuario?.SegundoApellido}",
                Fecha = src.Cirugias?.FirstOrDefault()?.Cita?.Fecha ?? DateTime.MinValue,
                Descripcion = src.Descripcion,
                //PrecioBase = src.PrecioBase,
           
                Estado = src.Estado
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
                Activo = src.Activo

            };

        public static HorariosDisponibles? ToHorariosDisponibles(this RegistrarHorarioDTO src) => src == null ? null :
            new HorariosDisponibles
            {
                HoraInicio = src.HoraInicio,
                DiaSemana = src.DiaSemana,
                Activo = src.Activo,
       
            };

        public static HorariosDisponibles? ToHorariosDisponibles(this EditarHorarioDTO src) => src == null ? null :
            new HorariosDisponibles
            {
                Id_Medico = src.Id_Medico,
                HoraInicio = src.HoraInicio,
                DiaSemana = src.DiaSemana,
                Activo = src.Activo
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
    }
}