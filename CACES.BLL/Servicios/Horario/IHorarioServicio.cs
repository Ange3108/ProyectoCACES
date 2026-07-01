using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Horario;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Horario
{
    public interface IHorarioServicio
    {

        Task<respuestaErrores<List<MostrarHorarioDTO>>> ObtenerHorarioPorMedicoIdAsync(int idMedico);
        Task<respuestaErrores<MostrarHorarioDTO>> CrearHorarioAsync(RegistrarHorarioDTO horario);
        Task<respuestaErrores<MostrarHorarioDTO>> ActualizarHorarioAsync(int id, EditarHorarioDTO horario);
        Task<respuestaErrores<MostrarHorarioDTO>> DesactivarHorarioAsync(int id);



       
    }
}
