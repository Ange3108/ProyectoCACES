using CACES.BLL.DTOs.Procedimientos;
using CACES.DAL.Repositorios.Procedimientos;
using System.Reflection.Metadata.Ecma335;

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

                string nombreDelPaciente = "Paciente Desconocido";

                if (cirugia.Paciente?.Usuario != null)
                {
                    var u = cirugia.Paciente.Usuario;

                    nombreDelPaciente = $"{u.Nombres} {u.PrimerApellido} {u.SegundoApellido}".Trim();
                }

                listaDtos.Add(new MostrarProcedimientosDTO
                {
                    Id_Cirugia = cirugia.Id_Cirugia,
                    Id_Paciente = cirugia.Id_Paciente,
                    Nombre = cirugia.Procedimiento?.Nombre ?? "Procedimiento Desconocido",
                    NombreMedico = nombreDelMedico,
                    NombrePaciente = nombreDelPaciente,
                    PrimerApellidoPaciente = cirugia.Paciente?.Usuario?.PrimerApellido ?? "Desconocido",
                    SegundoApellidoPaciente = cirugia.Paciente?.Usuario?.SegundoApellido ?? "",
                    Fecha = fechaFinalCita,
                    Estado = cirugia.Estado,
                    Descripcion = cirugia.Procedimiento?.Descripcion ?? "Sin indicaciones particulares."
                });
            }

            return listaDtos;
        }

        public async Task<bool> ActualizarProcedimientoAsync(EditarProcedimientosDTO editarProcedimientosDTO, int idPaciente)
        {
            var listaProcedimientos = await _procedimientosRepositorio.ObtenerDetalleCirugiaAsync(idPaciente);
            var cirugia = listaProcedimientos.FirstOrDefault(c => c.Id_Cirugia == editarProcedimientosDTO.Id_Procedimiento);
            if(cirugia == null) return false;
            cirugia.Estado = editarProcedimientosDTO.Estado;
            if(cirugia.Horario != null)
            {
                cirugia.Horario.DiaSemana = (int)editarProcedimientosDTO.Fecha.DayOfWeek;
                cirugia.Horario.HoraInicio = editarProcedimientosDTO.Fecha.TimeOfDay;
                cirugia.Horario.HoraFin = editarProcedimientosDTO.Fecha.TimeOfDay.Add(TimeSpan.FromHours(2));
            }
            if (cirugia.Procedimiento != null)
            {
                cirugia.Procedimiento.Descripcion = editarProcedimientosDTO.Descripcion;
            }
            return await _procedimientosRepositorio.ActualizarProcedimientoAsync(cirugia);
        }


        public async Task<EditarProcedimientosDTO?> ObtenerPorIdParaEditarAsync(int idPaciente, int idCirugia)
        {
            var listaCirugias = await _procedimientosRepositorio.ObtenerDetalleCirugiaAsync(idPaciente);
            var cirugia = listaCirugias.FirstOrDefault(c => c.Id_Cirugia == idCirugia);

            if (cirugia == null) return null;

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

            return new EditarProcedimientosDTO
            {
                Id_Procedimiento = cirugia.Id_Cirugia,
                Nombre = cirugia.Procedimiento?.Nombre ?? "Procedimiento Desconocido",
                NombreMedico = cirugia.Medico?.Usuario != null ? $"Dr. {cirugia.Medico.Usuario.Nombres}" : "Médico No Asignado",
                NombrePaciente = cirugia.Paciente?.Usuario != null ? cirugia.Paciente.Usuario.Nombres : "Paciente Desconocido",
                PrimerApellidoPaciente = cirugia.Paciente?.Usuario?.PrimerApellido ?? "Desconocido",
                SegundoApellidoPaciente = cirugia.Paciente?.Usuario?.SegundoApellido ?? "",
                Fecha = fechaFinalCita,
                Estado = cirugia.Estado,
                Descripcion = cirugia.Procedimiento?.Descripcion ?? "Sin indicaciones particulares."
            };
        }
    }
}
