using CACES.DAL.DBContext;
using CACES.DAL.DTOs.Roles;
using CACES.DAL.Entidades.Roles;
using Microsoft.EntityFrameworkCore;

namespace CACES.DAL.Repositorios.Roles
{
    public class RolRepositorio : IRolRepositorio
    {
        private readonly CACESDbContext _context;

        public RolRepositorio(CACESDbContext context)
        {
            _context = context;
        }

        public async Task<List<GestionRolDTO>> GetUsuariosConRolesAsync()
        {
            var usuarios = await (
                from u in _context.Usuarios
                join ur in _context.UsuarioRoles
                    on u.IdUsuario equals ur.IdUsuario
                join r in _context.AspNetRoles
                    on ur.RoleId equals r.Id
                select new GestionRolDTO
                {
                    IdUsuario = u.IdUsuario,
                    UserId = u.IdUsuario.ToString(),
                    NombreCompleto = u.Nombres + " " + u.PrimerApellido + " " + u.SegundoApellido,
                    CorreoElectronico = u.CorreoElectronico,
                    RoleId = r.Id,
                    NombreRol = r.Name
                }
            ).ToListAsync();

            return usuarios;
        }

        public async Task<List<AspNetRole>> GetRolesAsync()
        {
            return await _context.AspNetRoles.ToListAsync();
        }

        public async Task<bool> CambiarRolAsync(string userId, string roleId)
        {
            if (!int.TryParse(userId, out int idUsuario))
                return false;

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(x => x.IdUsuario == idUsuario);

            if (usuario == null)
                return false;

            var rolesActualesUsuario = await _context.UsuarioRoles
                .Where(x => x.IdUsuario == idUsuario)
                .ToListAsync();

            if (rolesActualesUsuario.Any())
            {
                _context.UsuarioRoles.RemoveRange(rolesActualesUsuario);
            }

            await _context.UsuarioRoles.AddAsync(new UsuarioRoles
            {
                IdUsuario = idUsuario,
                RoleId = roleId
            });

            usuario.FechaDeModificacion = DateTime.Now;

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DesactivarUsuarioPorRolAsync(string userId, string nombreRol)
        {
            if (!int.TryParse(userId, out int idUsuario))
                return false;

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(x => x.IdUsuario == idUsuario);

            if (usuario == null)
                return false;

            var usuarioRol = await (
                from ur in _context.UsuarioRoles
                join r in _context.AspNetRoles
                    on ur.RoleId equals r.Id
                where ur.IdUsuario == idUsuario && r.Name == nombreRol
                select ur
            ).FirstOrDefaultAsync();

            if (usuarioRol == null)
                return false;

            usuario.Estado = false;
            usuario.FechaDeModificacion = DateTime.Now;

            _context.Usuarios.Update(usuario);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}