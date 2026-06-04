using CACES.DAL.DTOs.Roles;
using CACES.DAL.Entidades.Roles;
using CACES.DAL.DTOs.Roles;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Roles
{
    public interface IRolServicio
    {
        Task<List<GestionRolDTO>> GetUsuariosConRolesAsync();
        Task<List<AspNetRole>> GetRolesAsync();
        Task<bool> CambiarRolAsync(string userId, string roleId);
    }
}