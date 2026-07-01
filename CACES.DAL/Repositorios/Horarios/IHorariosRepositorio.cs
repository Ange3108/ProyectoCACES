using CACES.DAL.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.Horarios
{
    public interface IHorariosRepositorio
    {
        Task<bool> RegistrarHorariosAsync(IEnumerable<HorariosDisponibles> horarios, CancellationToken cancellationToken);
    }
}
