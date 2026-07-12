document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendario');
    if (!calendarEl) return;

    var urlEventos = calendarEl.dataset.eventosUrl;

    var calendar = new FullCalendar.Calendar(calendarEl, {
        locale: 'es',
        initialView: 'dayGridMonth',
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,listWeek'
        },
        height: 'auto',
        events: function (info, successCallback, failureCallback) {
            fetch(urlEventos)
                .then(res => res.json())
                .then(respuesta => {
                    if (!respuesta.esCorrecto) {
                        failureCallback(respuesta.mensaje);
                        return;
                    }

                    var eventos = respuesta.dato.map(function (cita) {
                        var fecha = cita.fechaCita.substring(0, 10);
                        var titulo = cita.nombrePaciente && cita.nombreMedico
                            ? (calendarEl.dataset.eventosUrl.includes('ObtenerCitasMedico')
                                ? `${cita.nombrePaciente} - ${cita.nombreEspecialidad}`
                                : `${cita.nombreMedico} - ${cita.nombreEspecialidad}`)
                            : cita.nombreEspecialidad;

                        return {
                            id: cita.idCita,
                            title: titulo,
                            start: `${fecha}T${cita.hora}`,
                            color: cita.estado === 1 ? '#28a745' : '#dc3545'
                        };
                    });

                    successCallback(eventos);
                })
                .catch(failureCallback);
        }
    });

    calendar.render();
});