using CACES.BLL.DTOs.Procedimientos;
using CACES.DAL.Repositorios.Procedimientos;

namespace CACES.BLL.Servicios.Procedimientos
{
    public class ProcedimientosServicio : IProcedimientosServicio
    {
        private readonly IProcedimientosRepositorio _procedimientosRepositorio;

        public ProcedimientosServicio(IProcedimientosRepositorio procedimientosRepositorio)
        {
            _procedimientosRepositorio = procedimientosRepositorio;
        }

        public async Task<List<MostrarProcedimientosDTO?>> ObtenerDetalleCirugiaAsync(int idPaciente)
        {
            var listaCirugias = await _procedimientosRepositorio.ObtenerDetalleCirugiaAsync(idPaciente);
            var listaDtos = new List<MostrarProcedimientosDTO>();

            foreach (var cirugia in listaCirugias)
            {
                DateTime fechaFinalCita = DateTime.Today;

                if (cirugia.Horario != null)
                {
                    DayOfWeek diaObjetivo = (DayOfWeek)cirugia.Horario.DiaSemana;
                    while (fechaFinalCita.DayOfWeek != diaObjetivo)
                    {
                        fechaFinalCita = fechaFinalCita.AddDays(1);
                    }
                    fechaFinalCita = fechaFinalCita.Add(cirugia.Horario.HoraInicio);
                }

                string nombreDelMedico = cirugia.Medico?.Usuario != null
                    ? $"Dr. {cirugia.Medico.Usuario.Nombres}"
                    : "Médico No Asignado";

                listaDtos.Add(new MostrarProcedimientosDTO
                {
                    Nombre = cirugia.Procedimiento?.Nombre ?? "Procedimiento Desconocido",
                    NombreMedico = nombreDelMedico,
                    Fecha = fechaFinalCita,
                    Estado = cirugia.Estado,
                    Descripcion = cirugia.Procedimiento?.Descripcion ?? "Sin indicaciones particulares."
                });
            }

            return listaDtos;
        }
    }
}
