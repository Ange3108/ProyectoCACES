using System;
using System.Collections.Generic;
using System.Text;
using CACES.DAL.Entidades;

namespace CACES.DAL.Repositorios.Recetas
{
    public interface IRecetaRepositorio
    {
        Task<Receta?> ObtenerPorIdAsync(int idReceta);

        Task<Receta?> ObtenerPorCitaAsync(int idCita);

        Task<List<Receta>> ObtenerPorPacienteAsync(int idPaciente);

        Task<List<Receta>> ObtenerPorMedicoAsync(int idMedico);

        Task<bool> ExistePorCitaAsync(int idCita);

        Task<Receta> RegistrarAsync(Receta receta);

        Task<Receta> ActualizarAsync(Receta receta);
    }
}