using CACES.BLL.DTOs.Precio;
using CACES.BLL.Mappers;
using CACES.DAL.Repositorios.Precio;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Precio
{
    public class PrecioServicio : IPrecioServicio
    {
        private readonly IPrecioRepositorio _precioRepositorio;

        public PrecioServicio(IPrecioRepositorio precioRepositorio)
        {
            _precioRepositorio = precioRepositorio;
        }

        public async Task<IEnumerable<MostrarPrecioDTO>> GetAllPreciosAsync()
        {
            var precios = await _precioRepositorio.GetAllAsync();
            return precios.Select(p => p.ToMostrarPrecioDTO()!).ToList();
        }

        public async Task<MostrarPrecioDTO?> GetPrecioByIdAsync(int id)
        {
            var precio = await _precioRepositorio.GetByIdAsync(id);
            return precio?.ToMostrarPrecioDTO();
        }

        public async Task<IEnumerable<MostrarPrecioDTO>> GetPreciosByMedicoAsync(int idMedico)
        {
            var precios = await _precioRepositorio.GetByMedicoIdAsync(idMedico);
            return precios.Select(p => p.ToMostrarPrecioDTO()!).ToList();
        }

        public async Task<MostrarPrecioDTO?> CreatePrecioAsync(RegistrarPrecioDTO dto)
        {
            var existe = await _precioRepositorio.GetByMedicoYProcedimientoAsync(dto.IdMedico, dto.IdProcedimiento);
            if (existe != null)
            {
                throw new InvalidOperationException("El médico ya tiene asignado un precio para este procedimiento.");
            }

            var entidad = dto.ToPrecio();
            if (entidad == null) return null;

            await _precioRepositorio.AddAsync(entidad);
            await _precioRepositorio.SaveChangesAsync();

            // Cargar entidad con las relaciones (Medico, Usuario, Procedimiento) para el DTO final
            var precioCreado = await _precioRepositorio.GetByIdAsync(entidad.Id_Precio);
            return precioCreado?.ToMostrarPrecioDTO();
        }

        public async Task<bool> UpdatePrecioAsync(EditarPrecioDTO dto)
        {
            var precioExistente = await _precioRepositorio.GetByIdAsync(dto.IdPrecio);
            if (precioExistente == null) return false;

            precioExistente.UpdateFromEditarPrecioDTO(dto);

            _precioRepositorio.Update(precioExistente);
            return await _precioRepositorio.SaveChangesAsync();
        }

       
    }
}
