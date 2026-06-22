using System;
using System.Collections.Generic;
using System.Text;
using CACES.DAL.DTOs.Roles;
using CACES.DAL.DBContext;
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
                join au in _context.AspNetUsers
                    on u.CorreoElectronico equals au.Email
                join ur in _context.AspNetUserRoles
                    on au.Id equals ur.UserId
                join r in _context.AspNetRoles
                    on ur.RoleId equals r.Id
                select new GestionRolDTO
                {
                    IdUsuario = u.IdUsuario,
                    UserId = au.Id,
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
            var rolesActuales = await _context.AspNetUserRoles
                .Where(x => x.UserId == userId)
                .ToListAsync();

            if (rolesActuales.Any(x => x.RoleId == roleId))
                return true;

            if (rolesActuales.Any())
                _context.AspNetUserRoles.RemoveRange(rolesActuales);

            var nuevoRol = new AspNetUserRole
            {
                UserId = userId,
                RoleId = roleId
            };

            await _context.AspNetUserRoles.AddAsync(nuevoRol);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> EliminarUsuarioPorRolAsync(string userId, string nombreRol)
        {
            var aspUser = await _context.AspNetUsers
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (aspUser == null)
                return false;

            var rolUsuario = await (
                from ur in _context.AspNetUserRoles
                join r in _context.AspNetRoles
                    on ur.RoleId equals r.Id
                where ur.UserId == userId && r.Name == nombreRol
                select ur
            ).FirstOrDefaultAsync();

            if (rolUsuario == null)
                return false;

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(x => x.CorreoElectronico == aspUser.Email);

            if (usuario == null)
                return false;

            usuario.Estado = 0;
            usuario.FechaDeModificacion = DateTime.Now;

            _context.AspNetUserRoles.Remove(rolUsuario);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}