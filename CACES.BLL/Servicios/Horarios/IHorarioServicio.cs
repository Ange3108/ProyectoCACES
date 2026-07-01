using CACES.BLL.DTOs.HorariosDisponibles;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.BLL.Servicios.Horarios
{
    public interface IHorarioServicio
    {
        Task<bool> RegistrarDisponibilidadAsync(RegistrarHorarioDto dto, CancellationToken cancellationToken);
    }
}
