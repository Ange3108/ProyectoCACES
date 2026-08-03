using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Configuracion;
using CACES.BLL.Mappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Configuracion
{
    public class ConfiguracionServicio : IConfiguracionServicio
    {
        private readonly IConfiguracionRepositorio _repositorio;

        public ConfiguracionServicio(IConfiguracionRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<respuestaErrores<List<ConfiguracionDTO>>> ObtenerTodos()
        {
            var lista = await _repositorio.ObtenerTodosAsync();
            var dtos = lista.Select(c => c.ToDTO()).ToList();

            return new respuestaErrores<List<ConfiguracionDTO>>
            {
                EsCorrecto = true,
                mensaje = "Configuraciones obtenidas correctamente",
                codigo = 200,
                Dato = dtos
            };
        }

        public async Task<respuestaErrores<ConfiguracionDTO>> ObtenerPorId(int id)
        {
            var entidad = await _repositorio.ObtenerPorIdAsync(id);
            if (entidad == null)
            {
                return new respuestaErrores<ConfiguracionDTO>
                {
                    EsCorrecto = false,
                    mensaje = "Configuración no encontrada",
                    codigo = 404,
                    Dato = null
                };
            }

            return new respuestaErrores<ConfiguracionDTO>
            {
                EsCorrecto = true,
                mensaje = "Configuración obtenida correctamente",
                codigo = 200,
                Dato = entidad.ToDTO()
            };
        }

        public async Task<respuestaErrores<ConfiguracionDTO>> ObtenerPorClave(string clave)
        {
            var entidad = await _repositorio.ObtenerPorClaveAsync(clave);
            if (entidad == null)
            {
                return new respuestaErrores<ConfiguracionDTO>
                {
                    EsCorrecto = false,
                    mensaje = $"No existe configuración con la clave '{clave}'",
                    codigo = 404,
                    Dato = null
                };
            }

            return new respuestaErrores<ConfiguracionDTO>
            {
                EsCorrecto = true,
                mensaje = "Configuración obtenida correctamente",
                codigo = 200,
                Dato = entidad.ToDTO()
            };
        }

        public async Task<respuestaErrores<List<ConfiguracionDTO>>> ObtenerPorCategoria(string categoria)
        {
            var lista = await _repositorio.ObtenerPorCategoriaAsync(categoria);
            var dtos = lista.Select(c => c.ToDTO()).ToList();

            return new respuestaErrores<List<ConfiguracionDTO>>
            {
                EsCorrecto = true,
                mensaje = "Configuraciones obtenidas correctamente",
                codigo = 200,
                Dato = dtos
            };
        }

        public async Task<respuestaErrores<bool>> Crear(ConfiguracionDTO dto)
        {
            var existente = await _repositorio.ObtenerPorClaveAsync(dto.Clave);
            if (existente != null)
            {
                return new respuestaErrores<bool>
                {
                    EsCorrecto = false,
                    mensaje = "Ya existe una configuración con esa clave",
                    codigo = 400,
                    Dato = false
                };
            }

            var entidad = dto.ToEntity();
            await _repositorio.Crear(entidad);
            await _repositorio.GuardarCambiosAsync();

            return new respuestaErrores<bool>
            {
                EsCorrecto = true,
                mensaje = "Configuración creada correctamente",
                codigo = 200,
                Dato = true
            };
        }

        public async Task<respuestaErrores<bool>> Actualizar(ConfiguracionDTO dto)
        {
            var entidad = await _repositorio.ObtenerPorIdAsync(dto.IdConfiguracion);
            if (entidad == null)
            {
                return new respuestaErrores<bool>
                {
                    EsCorrecto = false,
                    mensaje = "Configuración no encontrada",
                    codigo = 404,
                    Dato = false
                };
            }

            entidad.Valor = dto.Valor;
            entidad.Categoria = dto.Categoria;
            entidad.Descripcion = dto.Descripcion;

            _repositorio.Actualizar(entidad);
            await _repositorio.GuardarCambiosAsync();

            return new respuestaErrores<bool>
            {
                EsCorrecto = true,
                mensaje = "Configuración actualizada correctamente",
                codigo = 200,
                Dato = true
            };
        }

        // --- Helpers tipados, pensados para el job de Hangfire ---

        public async Task<int> ObtenerValorInt(string clave, int valorPorDefecto = 0)
        {
            var entidad = await _repositorio.ObtenerPorClaveAsync(clave);
            if (entidad == null || !int.TryParse(entidad.Valor, out int resultado))
                return valorPorDefecto;

            return resultado;
        }

        public async Task<bool> ObtenerValorBool(string clave, bool valorPorDefecto = false)
        {
            var entidad = await _repositorio.ObtenerPorClaveAsync(clave);
            if (entidad == null || !bool.TryParse(entidad.Valor, out bool resultado))
                return valorPorDefecto;

            return resultado;
        }

        public async Task<string> ObtenerValorString(string clave, string valorPorDefecto = "")
        {
            var entidad = await _repositorio.ObtenerPorClaveAsync(clave);
            return entidad?.Valor ?? valorPorDefecto;
        }
    }
}
