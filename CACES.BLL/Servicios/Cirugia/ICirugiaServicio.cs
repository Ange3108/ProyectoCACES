using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Cirugia;
using CACES.BLL.DTOs.Cita;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Cirugia
{
    public interface ICirugiaServicio
    {
        Task<respuestaErrores<List<MostrarCirugiaDTO>>> GetAllCirugiaAsync();
        Task<respuestaErrores<MostrarCirugiaDTO>> GetCirugiaByIdAsync(int id);
        Task<respuestaErrores<List<MostrarCirugiaDTO>>> ObtenerCirugiaPorPacienteAsync(int idPaciente);
        Task<respuestaErrores<MostrarCitaDTO>> CancelarCirugiaAsync(int idCirugia);
        Task<respuestaErrores<MostrarCirugiaDTO>> ActualizarCirugiaAsync(int id,CirugiaDTO cirugia);
        Task<respuestaErrores<MostrarCirugiaDTO>> CompletarCirugia(int id);
    }
}
