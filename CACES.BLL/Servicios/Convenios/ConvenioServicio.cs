using CACES.BLL.DTOs.Convenios;
using CACES.BLL.Mappers;
using CACES.DAL.Repositorios.Convenios;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Convenios
{
    public class ConvenioServicio : IConvenioServicio
    {
        private readonly IConvenioRepositorio _convenioRepositorio;

        public ConvenioServicio(IConvenioRepositorio convenioRepositorio)
        {
            _convenioRepositorio = convenioRepositorio;
        }

        public async Task<List<MostrarConvenios>> GetConveniosAsync()
        {
            var convenios = await _convenioRepositorio.GetConveniosAsync();
            return convenios.Select(c => c.ToConvenioDTO()!).Where(c => c != null).ToList();
        }

        public async Task<List<MostrarConvenios>> GetConveniosSoloActivosAsync()
        {
            var convenios = await _convenioRepositorio.GetConveniosSoloActivosAsync();
            return convenios.Select(c => c.ToConvenioDTO()!).Where(c => c != null).ToList();
        }

        public async Task<MostrarConvenios?> GetConvenioByIdAsync(int id)
        {
            var convenio = await _convenioRepositorio.GetConvenioByIdAsync(id);
            return convenio?.ToConvenioDTO();
        }

        public async Task<bool> CreateConvenioAsync(CrearModificarConvenio dto)
        {
            if (dto == null) return false;

            var convenio = dto.ToConvenio();
            if (convenio == null) return false;

            return await _convenioRepositorio.CreateConvenioAsync(convenio);
        }

        public async Task<bool> UpdateConvenioAsync(int id, CrearModificarConvenio dto)
        {
            if (dto == null) return false;

            var convenioExistente = await _convenioRepositorio.GetConvenioByIdAsync(id);
            if (convenioExistente == null) return false;

            convenioExistente.UpdateFromActualizarConvenioDTO(dto);

            return await _convenioRepositorio.UpdateConvenioAsync(convenioExistente);
        }
    }
}
