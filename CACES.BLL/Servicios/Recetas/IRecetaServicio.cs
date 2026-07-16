using System;
using System.Collections.Generic;
using System.Text;
using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Receta;

namespace CACES.BLL.Servicios.Recetas
{
    public interface IRecetaServicio
    {
        Task<respuestaErrores<MostrarRecetaDTO>> RegistrarAsync(RegistrarRecetaDTO dto);

        Task<respuestaErrores<MostrarRecetaDTO>> ObtenerPorIdAsync(int idReceta);

        Task<respuestaErrores<MostrarRecetaDTO>> ObtenerPorCitaAsync(int idCita);

        Task<respuestaErrores<List<MostrarRecetaDTO>>> ObtenerPorPacienteAsync(int idPaciente);

        Task<respuestaErrores<List<MostrarRecetaDTO>>> ObtenerPorMedicoAsync(int idMedico);

        Task<respuestaErrores<MostrarRecetaDTO>> ActualizarAsync(RegistrarRecetaDTO dto);
    }
}