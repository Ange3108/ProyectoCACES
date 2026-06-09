using System;
using System.Collections.Generic;
using System.Text;
using CACES.DAL.DTOs.Roles;
using CACES.DAL.Entidades.Roles;

namespace CACES.DAL.Repositorios.Roles
{
    public interface IRolRepositorio
    {
        Task<List<GestionRolDTO>> GetUsuariosConRolesAsync();
        Task<List<AspNetRole>> GetRolesAsync();
        Task<bool> CambiarRolAsync(string userId, string roleId);
    }
}