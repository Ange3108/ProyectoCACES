using AutoMapper;
using CACES.BLL.DTOs.Usuario;
using CACES.BLL.DTOs.Paciente;
using CACES.DAL.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using CACES.BLL.DTOs.Medico;
using CACES.BLL.DTOs.Perfil;

namespace CACES.BLL
{
    public class MapeoClases : Profile
    {
        public MapeoClases()
        {
            //Mapeo de los 3 DTO de usuario
            CreateMap<Usuario, RegistrarUsuarioDTO>()
              .ForMember(dest => dest.passwordHash, opt => opt.Ignore()) // No mapear password
              .ReverseMap()
              .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<Usuario, ActualizarUsuarioDTO>()
                .ReverseMap();
            CreateMap<Usuario, MostrarUsuarioDTO>()
                .ReverseMap();


            //Mapeo de los DTOs de paciente

            CreateMap<Paciente, RegistrarPacienteDTO>().
                ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.Historial, opt => opt.Ignore());


            //Mapeo de los DTOs de médicos
            CreateMap<Medico, RegistrarMedicoDTO>()
                .ForMember(dest => dest.IdEspecialidad, opt => opt.Ignore()).ReverseMap();
            CreateMap<Medico, mostrarMedicoEspecialidadDTO>()
                .ForMember(dest => dest.Nombres, opt => opt.MapFrom(src => src.Usuario.Nombres))
                    .ForMember(dest => dest.PrimerApellido, opt => opt.MapFrom(src => src.Usuario.PrimerApellido))
                    .ForMember(dest => dest.SegundoApellido, opt => opt.MapFrom(src => src.Usuario.SegundoApellido))
                    .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Usuario.Telefono))
                    .ForMember(dest => dest.Foto, opt => opt.MapFrom(src => src.Usuario.Foto));
            CreateMap<Medico, MedicoDTO>()
                .ForMember(dest => dest.NombreEspecialidad, opt => opt.MapFrom(src => src.Especialidad.Nombre)).ReverseMap();
            CreateMap<Medico, EditarMedicoDTO>().ReverseMap();

            // Mapeo de los DTOs de perfil
            CreateMap<Usuario, PerfilUsuarioDTO>()
                // Nota: Eliminamos el mapeo manual de Estado porque Entity Framework 
                // ahora convierte automáticamente el BIT de SQL a bool de C#.

                .AfterMap((src, dest) =>
                {
                    var paciente = src.Paciente;

                    // Datos del historial médico
                    if (paciente?.HistorialMedico != null)
                    {
                        dest.TipoSangre = paciente.HistorialMedico.TipoSangre;
                        dest.Alergias = paciente.HistorialMedico.Alergias;
                        dest.EnfermedadesCronicas = paciente.HistorialMedico.EnfermedadesCronicas;
                    }
                    else
                    {
                        dest.TipoSangre = "No asignado";
                        dest.Alergias = "Ninguna reportada";
                        dest.EnfermedadesCronicas = "Ninguna registrada";
                    }

                    // Datos del medicamento (Extracción lineal desde la cita inyectada en tu repositorio)
                    if (paciente?.Cita?.Receta != null)
                    {
                        dest.MedicamentosActuales = paciente.Cita.Receta.Medicamentos;
                    }
                    else
                    {
                        dest.MedicamentosActuales = "Sin medicamentos prescritos";
                    }
                });


            CreateMap<Usuario, ActualizarPerfilDTO>()
                .ReverseMap();


            //mapeo de los dto de especialidad

            CreateMap<Especialidad, DTOs.Especialidad.especialidadDTO>()
                .ReverseMap();
            CreateMap<Especialidad, DTOs.Especialidad.mostrarEspecialidadDTO>()
               .ReverseMap();
            CreateMap<Especialidad, DTOs.Especialidad.mostrarDetalleEspecialidadDTO>()
               .ReverseMap();

            //mapeo de los dto de turismo medico(paquetes)
            CreateMap<Paquete, DTOs.Paquete.PaqueteDTO>()
                .ReverseMap()
                .ForMember(dest => dest.IdPaquete, opt => opt.Ignore())
                .ForMember(dest => dest.FechaDeRegistro, opt => opt.Ignore());

            CreateMap<Procedimiento, DTOs.Procedimientos.MostrarProcedimientosDTO>()
                .ReverseMap();
        }
    }
}
