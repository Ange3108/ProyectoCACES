using System;
using System.Collections.Generic;
using System.Text;
using CACES.BLL.DTOs.Auth;

namespace CACES.BLL.Servicios.Auth
{
    public interface IAuthServicio
    {
        Task<bool> LoginAsync(LoginDTO dto);
    }
}