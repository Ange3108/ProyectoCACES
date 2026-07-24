using CACES.BLL.DTOs;
using CACES.BLL.DTOs.SeguimientoPostOperatorio;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.PreguntasPOp
{
    public interface IPreguntasPOpServicio
    {
      
            Task<respuestaErrores<List<PreguntasPOpDTO>>> ObtenerTodos();
            Task<respuestaErrores<List<PreguntasPOpDTO>>> ObtenerActivas();
            Task<respuestaErrores<PreguntasPOpDTO>> ObtenerPorId(int idPregunta);
            Task<respuestaErrores<PreguntasPOpDTO>> Crear(RegistrarPreguntasPOpDTO dto);
            Task<respuestaErrores<PreguntasPOpDTO>> Actualizar(PreguntasPOpDTO dto);
            Task<respuestaErrores<PreguntasPOpDTO>> Eliminar(int idPregunta);
            Task<respuestaErrores<PreguntasPOpDTO>> Desactivar(int idPregunta);

    }
}
