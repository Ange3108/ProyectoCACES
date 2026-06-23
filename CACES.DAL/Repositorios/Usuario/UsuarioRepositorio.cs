using CACES.DAL.DBContext;
using CACES.DAL.Entidades;
using CACES.DAL.Entidades.Roles;
using Microsoft.EntityFrameworkCore;

namespace CACES.DAL.Repositorios.Usuario
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        // Inyección de dependencia de la BD
        private readonly CACESDbContext _context;

        public UsuarioRepositorio(CACESDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateUsuarioAsync(Entidades.Usuario usuario)
        {
            if (usuario == null)
                return false;

            await _context.Usuarios.AddAsync(usuario);
            await _context.UsuarioRoles.AddAsync(new UsuarioRoles
            {
                IdUsuario = usuario.IdUsuario,
                RoleId = "2"
            });
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<Entidades.Usuario> GetUsuarioByDUIAsync(string dui)
        {
            if (string.IsNullOrEmpty(dui))
                return null;

            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.DUI == dui);
        }

        public async Task<Entidades.Usuario> GetUsuarioByIdAsync(int id)
        {

            return await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);
        }
      

        public async Task<Entidades.Usuario> GetUsuarioByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email)) return null;
            return await _context.Usuarios.Include(u => u.UsuarioRoles)
                .ThenInclude(ur => ur.Rol)
                 .FirstOrDefaultAsync(u => u.CorreoElectronico == email);
        }

        public async Task<List<Entidades.Usuario>> GetUsuariosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<bool> UpdateUsuarioAsync(Entidades.Usuario usuario)
        {
            if (usuario == null)
                return false;

            _context.Usuarios.Update(usuario);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DesactivarUsuarioAsync(int id)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (usuario == null)
                return false;

            usuario.Estado = false;
            usuario.EmailConfirmed = false;
            usuario.FechaDeModificacion = DateTime.Now;

            _context.Usuarios.Update(usuario);

            return await _context.SaveChangesAsync() > 0;
        }

        
    }
}