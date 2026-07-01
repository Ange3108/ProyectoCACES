using CACES.BLL.DTOs;
using CACES.DAL.Repositorios.Quirofano;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Quirofano
{
    public class QuirofanoServicio : IQuirofanoServicio
    {
        private readonly IQuirofanoRepositorio _repositorioQuirofano;
        public QuirofanoServicio(IQuirofanoRepositorio repositorioQuirofano)
        {
            _repositorioQuirofano = repositorioQuirofano;
        }
        public async Task<respuestaErrores<int>> ActualizarCupoAsync(int nuevoCupo)
        {
            await _repositorioQuirofano.ActualizarCupoAsync(nuevoCupo);
            return new respuestaErrores<int>
            {
                EsCorrecto = true,
                mensaje = "Cupo actualizado correctamente",
                Dato = nuevoCupo,
                codigo = 200
            };
        }

        public async Task<respuestaErrores<int>> GetCupoMaximoAsync()
        {
            var cupo = await _repositorioQuirofano.GetCupoMaximoAsync();
            return new respuestaErrores<int>
            {
                EsCorrecto = true,
                mensaje = "Cupo máximo obtenido correctamente",
                Dato = cupo,
                codigo = 200
            };
        }
    }
}
