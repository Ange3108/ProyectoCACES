using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.Medicos
{
    public class MedicoRepositorio : IMedicoRepositorio
    {

        // Inyección de dependencia de la BD
        private readonly CACESDbContext _context;

        public MedicoRepositorio(CACESDbContext context)
        {
            _context = context;
        }

        // CREAR
        public async Task<bool> CreateMedicoAsync(Medico medico)
        {
            if (medico == null) return false;

            await _context.Medicos.AddAsync(medico);

            return await _context.SaveChangesAsync() > 0;
        }

        // ELIMINAR
        public async Task<bool> DeleteMedicoAsync(int id)
        {
            var entity = await _context.Medicos.FindAsync(id);

            if (entity == null) return false;

            _context.Medicos.Remove(entity);

            return await _context.SaveChangesAsync() > 0;
        }

        // OBTENER POR ESPECIALIDAD
        public async Task<Medico> GetMedicoByEspecialidadAsync(string especialidad)
        {
            return await _context.Medicos
                .FirstOrDefaultAsync(m => m.Especialidad == especialidad);
        }

        // OBTENER POR ID
        public async Task<Medico> GetMedicoByIdAsync(int id)
        {
            return await _context.Medicos
                .FirstOrDefaultAsync(m => m.IdMedico == id);
        }

        // LISTAR TODOS
        public async Task<List<Medico>> GetMedicosAsync()
        {
            return await _context.Medicos.ToListAsync();
        }

        // ACTUALIZAR
        public async Task<bool> UpdateMedicoAsync(Medico medico)
        {
            if (medico == null) return false;

            var existing = await _context.Medicos.FindAsync(medico.IdMedico);

            if (existing == null) return false;

            // Actualizar campos
            existing.IdUsuario = medico.IdUsuario;
            existing.Especialidad = medico.Especialidad;
            existing.AniosExperiencia = medico.AniosExperiencia;
            existing.Estado = medico.Estado;

            _context.Medicos.Update(existing);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}