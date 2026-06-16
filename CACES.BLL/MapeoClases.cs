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
                .ForMember(dest => dest.IdUsuario, opt => opt.Ignore())
                .ForMember(dest => dest.IdEspecialidad, opt => opt.Ignore());

            //Mapeo de los DTOs de perfil

            CreateMap<Usuario, PerfilUsuarioDTO>()
            .ForMember(dest => dest.TipoSangre, opt => opt.MapFrom(src =>
            (src.Paciente != null && src.Paciente.HistorialMedico != null)
            ? src.Paciente.HistorialMedico.TipoSangre
            : "No asignado"))

            .ForMember(dest => dest.Alergias, opt => opt.MapFrom(src =>
            (src.Paciente != null && src.Paciente.HistorialMedico != null)
            ? src.Paciente.HistorialMedico.Alergias
            : "Ninguna reportada"))

            .ForMember(dest => dest.EnfermedadesCronicas, opt => opt.MapFrom(src =>
            (src.Paciente != null && src.Paciente.HistorialMedico != null)
            ? src.Paciente.HistorialMedico.EnfermedadesCronicas
            : "Ninguna registrada"))

            .AfterMap((src, dest) =>
             {
                 var paciente = src.Paciente;

                 // Datos del historial
                 if (paciente?.HistorialMedico != null)
                 {
                     dest.TipoSangre = paciente.HistorialMedico.TipoSangre;
                     dest.Alergias = paciente.HistorialMedico.Alergias;
                     dest.EnfermedadesCronicas = paciente.HistorialMedico.EnfermedadesCronicas;
                 }
                 else
                 {
                     dest.TipoSangre = "No asignado";
                     dest.Alergias = "Ninguna";
                     dest.EnfermedadesCronicas = "Ninguna";
                 }

                 // Datos del medicamento (Extracción lineal y directa)
                 dest.MedicamentosActuales = paciente?.Cita?.Receta?.Medicamentos ?? "Sin medicamentos prescritos";

            });

             CreateMap<Usuario, ActualizarPerfilDTO>()
               .ReverseMap();
        }
    }
}
