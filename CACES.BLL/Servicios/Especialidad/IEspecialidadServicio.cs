using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Especialidad;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Especialidad
{
    public interface IEspecialidadServicio
    {
        Task<respuestaErrores<List<mostrarEspecialidadDTO>>> GetEspecialidadesAsync();
        Task<respuestaErrores<List<especialidadDTO>>> GetListadoEspecialidadesAsync();
        Task<respuestaErrores<List<mostrarEspecialidadDTO>>> GetEspecialidadesActivasAsync();
        Task<respuestaErrores<mostrarDetalleEspecialidadDTO>> GetDetalleEspecialidadAsync(int id);
        Task<respuestaErrores<mostrarEspecialidadDTO>> GetEspecialidadPorIdAsync(int id);
        Task<respuestaErrores<mostrarEspecialidadDTO>> GetEspecialidadPorNombreAsync(string nombre);
        Task<respuestaErrores<mostrarEspecialidadDTO>> CrearEspecialidadAsync(especialidadDTO especialidad);
        Task<respuestaErrores<mostrarEspecialidadDTO>> ActualizarEspecialidadAsync(int id, especialidadDTO especialidad);
        Task<respuestaErrores<mostrarEspecialidadDTO>> DesactivarEspecialidadAsync(int id);
        
    }
}
