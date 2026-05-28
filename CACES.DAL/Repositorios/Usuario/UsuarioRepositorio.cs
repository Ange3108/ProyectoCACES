using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.Usuario
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        //Inyerccion de dependencia de la BD
        private readonly CACESContext _context;

        public UsuarioRepositorio(CACESContext context)
        {
            _context = context;
        }



        public async Task<bool> CreateUsuarioAsync(Entidades.Usuario usuario)
        {
            if (usuario == null) return false;
            await _context.Usuarios.AddAsync(usuario);
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<bool> DeleteUsuarioAsync(int id)
        {
            var entity = await _context.Usuarios.FindAsync(id);
            if (entity == null) return false;
            _context.Usuarios.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Entidades.Usuario> GetUsuarioByDUIAsync(string dui)
        {
            throw new NotImplementedException();
        }

        public async Task<Entidades.Usuario> GetUsuarioByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Entidades.Usuario>> GetUsuariosAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateUsuarioAsync(Entidades.Usuario usuario)
        {
            throw new NotImplementedException();
        }
    }
}
