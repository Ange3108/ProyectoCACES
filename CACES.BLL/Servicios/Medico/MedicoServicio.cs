using System;
using CACES.DAL.Entidades;
using CACES.DAL.Repositorios.Medicos;

namespace CACES.BLL.Servicios.Medicos
{
    public class MedicoServicio : IMedicoServicio
    {
        private readonly IMedicoRepositorio _medicoRepositorio;

        public MedicoServicio(IMedicoRepositorio medicoRepositorio)
        {
            _medicoRepositorio = medicoRepositorio;
        }

        public async Task<List<Medico>> GetMedicosAsync()
        {
            return await _medicoRepositorio.GetMedicosAsync();
        }

        public async Task<Medico> GetMedicoByIdAsync(int id)
        {
            return await _medicoRepositorio.GetMedicoByIdAsync(id);
        }

        public async Task<bool> CreateMedicoAsync(Medico medico)
        {
            return await _medicoRepositorio.CreateMedicoAsync(medico);
        }

        public async Task<bool> UpdateMedicoAsync(Medico medico)
        {
            return await _medicoRepositorio.UpdateMedicoAsync(medico);
        }

        public async Task<bool> DeleteMedicoAsync(int id)
        {
            return await _medicoRepositorio.DeleteMedicoAsync(id);
        }
    }
}