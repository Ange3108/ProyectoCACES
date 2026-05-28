using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.Usuario
{
    public interface IUsuarioRepositorio
    {
        Task<List<Entidades.Usuario>> GetUsuariosAsync();
        Task<Entidades.Usuario> GetUsuarioByIdAsync(int id);
        Task<Entidades.Usuario> GetUsuarioByDUIAsync(string dui);
        Task<bool> CreateUsuarioAsync(Entidades.Usuario usuario);
        Task<bool> UpdateUsuarioAsync(Entidades.Usuario usuario);
        Task<bool> DeleteUsuarioAsync(int id);
    }
}
