using CACES.DAL.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace CACES.DAL.Repositorios.SeguimientoPaciente
{
    public interface ISeguimientoPacienteRepositorio
    {
        Task<List<DAL.Entidades.SeguimientoPostOperatorio.SeguimientoPaciente>> ObtenerPorCirugia(int idCirugia);
        Task<List<Entidades.SeguimientoPostOperatorio.SeguimientoPaciente>> ObtenerProgramadosParaHoy();
        Task<Cirugias?> ObtenerCirugiaConFecha(int idCirugia);
        Task AgregarRango(List<Entidades.SeguimientoPostOperatorio.SeguimientoPaciente> entidades);
    }
}

