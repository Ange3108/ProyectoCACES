using System;
using System.Collections.Generic;
using System.Text;
using CACES.DAL.DTOs.Roles;
using CACES.DAL.Entidades.Roles;
using CACES.DAL.Repositorios.Roles;

namespace CACES.BLL.Servicios.Roles
{
    public class RolServicio : IRolServicio
    {
        private readonly IRolRepositorio _rolRepositorio;

        public RolServicio(IRolRepositorio rolRepositorio)
        {
            _rolRepositorio = rolRepositorio;
        }

        public async Task<List<GestionRolDTO>> GetUsuariosConRolesAsync()
        {
            return await _rolRepositorio.GetUsuariosConRolesAsync();
        }

        public async Task<List<AspNetRole>> GetRolesAsync()
        {
            return await _rolRepositorio.GetRolesAsync();
        }

        public async Task<bool> CambiarRolAsync(string userId, string roleId)
        {
            return await _rolRepositorio.CambiarRolAsync(userId, roleId);
        }
    }
}