
using CACES.BLL.DTOs;
using CACES.BLL.DTOs.Procedimientos;
using CACES.BLL.Mappers;
using CACES.BLL.Servicios.Especialidad;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.Especialidades;
using CACES.DAL.Repositorios.Procedimientos;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace CACES.BLL.Servicios.Procedimientos
{
    public class ProcedimientosServicio : IProcedimientosServicio
    {
        private readonly IProcedimientosRepositorio _procedimientosRepositorio;
 
        public ProcedimientosServicio(IProcedimientosRepositorio procedimientosRepositorio)
        {
            _procedimientosRepositorio = procedimientosRepositorio;

        }

        

      

       

        
        public async Task<List<InsertarProcedimientosDto>> ListarProcedimientosAsync()
        {
            var entidades = await _procedimientosRepositorio.ObtenerTodosLosProcedimientosAsync();
            return entidades.Select(e => e.ToInsertarProcedimientosDto()).ToList();
        }

        public async Task<bool> GuardarProcedimientoAsync(InsertarProcedimientosDto dto)
        {
            var entidad = dto.ToProcedimiento();

            entidad.Estado = dto.Estado;

            return await _procedimientosRepositorio.InsertarProcedimientoAsync(entidad);
        }

        public async Task<InsertarProcedimientosDto> ObtenerPorIdAsync(int id)
        {
            var entidad = await _procedimientosRepositorio.ObtenerProcedimientoPorIdAsync(id);
            return entidad?.ToInsertarProcedimientosDto();
        }
        public async Task<bool> EditarProcedimientoAdminAsync(InsertarProcedimientosDto dto)
        {
            try
            {
                var entidad = dto.ToProcedimiento();

                entidad.Estado = dto.Estado;

                return await _procedimientosRepositorio.ActualizarProcedimientoAdminAsync(entidad);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CambiarEstadoProcedimientoAsync(int id)
        {
            try
            {
                return await _procedimientosRepositorio.CambiarEstadoProcedimientoAsync(id);
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}

