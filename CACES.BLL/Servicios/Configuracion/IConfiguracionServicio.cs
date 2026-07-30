using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Configuracion;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Configuracion
{
    public interface IConfiguracionServicio
    {
        Task<respuestaErrores<List<ConfiguracionDTO>>> ObtenerTodos();
        Task<respuestaErrores<ConfiguracionDTO>> ObtenerPorId(int id);
        Task<respuestaErrores<ConfiguracionDTO>> ObtenerPorClave(string clave);
        Task<respuestaErrores<List<ConfiguracionDTO>>> ObtenerPorCategoria(string categoria);
        Task<respuestaErrores<bool>> Crear(ConfiguracionDTO dto);
        Task<respuestaErrores<bool>> Actualizar(ConfiguracionDTO dto);

        // Helpers para leer valores tipados directamente, útil para el job de Hangfire
        Task<int> ObtenerValorInt(string clave, int valorPorDefecto = 0);
        Task<bool> ObtenerValorBool(string clave, bool valorPorDefecto = false);
        Task<string> ObtenerValorString(string clave, string valorPorDefecto = "");
    }
}
