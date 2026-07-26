document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendario');
    if (!calendarEl) return;

    var urlEventos = calendarEl.dataset.eventosUrl;
    var popoverActivo = null;

    var calendar = new FullCalendar.Calendar(calendarEl, {
        locale: 'es',
        buttonText: {
            today: 'Hoy',
            month: 'Mes',
            week: 'Semana',
            list: 'Lista'
        },
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
                        var esVistaMedico = calendarEl.dataset.eventosUrl.includes('ObtenerCitasMedico');
                        var titulo = cita.nombrePaciente && cita.nombreMedico
                            ? (esVistaMedico
                                ? `${cita.nombrePaciente} - ${cita.nombreEspecialidad}`
                                : `${cita.nombreMedico} - ${cita.nombreEspecialidad}`)
                            : cita.nombreEspecialidad;

                        return {
                            id: cita.idCita,
                            title: titulo,
                            start: `${fecha}T${cita.hora}`,
                            color: cita.estado === 1 ? '#28a745' : '#dc3545',
                            extendedProps: {
                                paciente: cita.nombrePaciente,
                                medico: cita.nombreMedico,
                                especialidad: cita.nombreEspecialidad,
                                hora: cita.hora,
                                estado: cita.estado,
                                motivo: cita.motivo
                            }
                        };
                    });

                    successCallback(eventos);
                })
                .catch(failureCallback);
        },
        eventClick: function (info) {
            info.jsEvent.preventDefault();
            info.jsEvent.stopPropagation();

            if (popoverActivo) {
                popoverActivo.dispose();
                popoverActivo = null;
            }

            var props = info.event.extendedProps;
            var estadoTexto = props.estado === 1
                ? '<span class="badge bg-success">Confirmada</span>'
                : '<span class="badge bg-danger">Cancelada</span>';

            var horaFormateada = (props.hora ?? '').toString().substring(0, 5);

            var contenido = `
                <div class="text-start">
                    <div class="mb-1"><i class="bi bi-clock me-1"></i><strong>${horaFormateada}</strong></div>
                    ${props.paciente ? `<div class="mb-1"><i class="bi bi-person me-1"></i>${props.paciente}</div>` : ''}
                    ${props.medico ? `<div class="mb-1"><i class="bi bi-heart-pulse me-1"></i>${props.medico}</div>` : ''}
                    <div class="mb-1"><i class="bi bi-clipboard2-pulse me-1"></i>${props.especialidad ?? ''}</div>
                    ${props.motivo ? `<div class="mb-2"><i class="bi bi-chat-square-text me-1"></i>${props.motivo}</div>` : ''}
                    ${estadoTexto}
                </div>
            `;

            popoverActivo = new bootstrap.Popover(info.el, {
                title: 'Detalle de la cita',
                content: contenido,
                html: true,
                placement: 'top',
                trigger: 'manual',
                container: 'body',
                sanitize: false
            });

            popoverActivo.show();
        },
        datesSet: function () {
            if (popoverActivo) {
                popoverActivo.dispose();
                popoverActivo = null;
            }
        }
    });

    calendar.render();

    document.addEventListener('click', function (e) {
        if (popoverActivo) {
            var esPopover = e.target.closest('.popover');
            var esEvento = e.target.closest('.fc-event');
            if (!esPopover && !esEvento) {
                popoverActivo.dispose();
                popoverActivo = null;
            }
        }
    });
});