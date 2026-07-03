using System;
using System.Collections.Generic;
using System.Text;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.Soportes;

namespace CACES.BLL.Servicios.Soportes
{
    public class SoporteServicio : ISoporteServicio
    {
        private readonly ISoporteRepositorio _soporteRepositorio;

        public SoporteServicio(ISoporteRepositorio soporteRepositorio)
        {
            _soporteRepositorio = soporteRepositorio;
        }

        public async Task<bool> CrearConsultaAsync(int idUsuario, string asunto, string mensaje)
        {
            if (idUsuario <= 0)
                return false;

            if (string.IsNullOrWhiteSpace(asunto))
                return false;

            if (string.IsNullOrWhiteSpace(mensaje))
                return false;

            var soporte = new Soporte
            {
                IdUsuario = idUsuario,
                Asunto = asunto.Trim(),
                Mensaje = mensaje.Trim(),
                FechaConsulta = DateTime.Now,
                Estado = true
            };

            return await _soporteRepositorio.CrearConsultaAsync(soporte);
        }

        public async Task<List<Soporte>> GetConsultasAsync()
        {
            return await _soporteRepositorio.GetConsultasAsync();
        }

        public async Task<Soporte?> GetConsultaByIdAsync(int id)
        {
            return await _soporteRepositorio.GetConsultaByIdAsync(id);
        }
    }
}