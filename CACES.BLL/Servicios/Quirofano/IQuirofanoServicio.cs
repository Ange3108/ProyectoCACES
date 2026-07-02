using CACES.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Quirofano
{
    public interface IQuirofanoServicio
    {
        Task<respuestaErrores<int>> GetCupoMaximoAsync();
        Task<respuestaErrores<int>> ActualizarCupoAsync(int nuevoCupo);
    }
}
