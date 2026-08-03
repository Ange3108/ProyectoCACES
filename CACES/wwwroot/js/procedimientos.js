const Procedimientos = {

    init() {
        if ($('#tbMisProcedimientos').length) {
            this.cargarMisProcedimientos();
        }

        if ($('#formAgendarProcedimiento').length) {
            this.cargarEspecialidades();
            this.registrarEventosFormulario();
        }
    },

    registrarEventosFormulario() {
        $('#IdEspecialidad').on('change', function () {
            const idEspecialidad = $(this).val();
            Procedimientos.cargarProcedimientos(idEspecialidad);
            Procedimientos.limpiarSelect($('#IdMedico'), 'Seleccione un procedimiento primero');
            Procedimientos.limpiarSelect($('#IdHorario'), 'Seleccione un médico primero');
        });

        $('#IdProcedimiento').on('change', function () {
            const idEspecialidad = $('#IdEspecialidad').val();
            Procedimientos.cargarMedicos(idEspecialidad);
            Procedimientos.limpiarSelect($('#IdHorario'), 'Seleccione un médico primero');
        });

        $('#IdMedico').on('change', function () {
            const idMedico = $(this).val();
            Procedimientos.cargarHorarios(idMedico);
        });

        $('#IdHorario').on('change', function () {
            const horaInicio = $(this).find('option:selected').data('hora');
            $('#Hora').val(horaInicio ? horaInicio.substring(0, 5) : '');
        });

        $('#Motivo').on('input', function () {
            const len = $(this).val().length;
            $('#contadorCaracteres').text(`${len} / 100`);
        });

        $('#formAgendarProcedimiento').on('submit', function (e) {
            e.preventDefault();
            Procedimientos.guardarProcedimiento();
        });
    },

    cargarEspecialidades() {
        fetch('/Cita/ObtenerEspecialidadesActivas')
            .then(r => r.json())
            .then(res => {
                const select = $('#IdEspecialidad');
                select.empty().append('<option value="">Seleccione una especialidad...</option>');
                if (res.dato && Array.isArray(res.dato)) {
                    res.dato.forEach(e => select.append(`<option value="${e.id}">${e.nombre}</option>`));
                }
            })
            .catch(() => console.error('Error al cargar especialidades.'));
    },

    cargarProcedimientos(idEspecialidad) {
        const select = $('#IdProcedimiento');
        this.limpiarSelect(select, 'Cargando procedimientos...');

        if (!idEspecialidad) return;

        fetch(`/ProcedimientoAgendar/ObtenerProcedimientosFijos?idEspecialidad=${idEspecialidad}`)
            .then(r => {
                if (!r.ok) throw new Error(`HTTP error! status: ${r.status}`);
                return r.json();
            })
            .then(res => {
                select.empty().append('<option value="">Seleccione un procedimiento...</option>');
                const lista = Array.isArray(res) ? res : (res.dato || res.data || []);

                if (lista.length > 0) {
                    lista.forEach(p => {
                        const id = p.id_Procedimiento ?? p.idProcedimiento ?? p.id ?? p.Id_Procedimiento ?? p.IdProcedimiento ?? p.Id;
                        const nombre = p.nombre ?? p.nombreProcedimiento ?? p.Nombre ?? p.NombreProcedimiento;
                        select.append(`<option value="${id}">${nombre}</option>`);
                    });
                    select.prop('disabled', false);
                } else {
                    select.append('<option value="">Sin procedimientos disponibles</option>');
                }
            })
            .catch(err => {
                console.error('Error al cargar procedimientos:', err);
                this.limpiarSelect(select, 'Error al cargar procedimientos');
            });
    },

    cargarMedicos(idEspecialidad) {
        const select = $('#IdMedico');
        this.limpiarSelect(select, 'Cargando médicos...');

        if (!idEspecialidad) return;

        fetch(`/Cita/ObtenerMedicos?idEspecialidad=${idEspecialidad}`)
            .then(r => r.json())
            .then(res => {
                select.empty().append('<option value="">Seleccione un especialista...</option>');
                if (res.dato && Array.isArray(res.dato)) {
                    res.dato.forEach(m => select.append(`<option value="${m.id}">${m.nombre}</option>`));
                    select.prop('disabled', false);
                }
            })
            .catch(() => this.limpiarSelect(select, 'Error al cargar médicos'));
    },

    cargarHorarios(idMedico) {
        const select = $('#IdHorario');
        this.limpiarSelect(select, 'Cargando horarios...');

        if (!idMedico) return;

        fetch(`/Cita/ObtenerHorariosPorMedico?idMedico=${idMedico}`)
            .then(r => r.json())
            .then(res => {
                select.empty().append('<option value="">Seleccione un horario...</option>');
                if (res.dato && Array.isArray(res.dato)) {
                    res.dato.forEach(h => {
                        select.append(`
                            <option value="${h.idHorario}" data-hora="${h.horaInicio}">
                                ${h.horarioTexto}
                            </option>
                        `);
                    });
                    select.prop('disabled', false);
                }
            })
            .catch(() => this.limpiarSelect(select, 'Error al cargar horarios'));
    },

    guardarProcedimiento() {
        const dto = {
            IdPaciente: 0,
            IdEspecialidad: parseInt($('#IdEspecialidad').val(), 10) || 0,
            IdProcedimiento: parseInt($('#IdProcedimiento').val(), 10) || null,
            IdMedico: parseInt($('#IdMedico').val(), 10) || 0,
            IdHorario: parseInt($('#IdHorario').val(), 10) || 0,
            FechaCita: $('#FechaCita').val(),
            Hora: $('#Hora').val() ? $('#Hora').val() + ":00" : "00:00:00",
            Motivo: $('#Motivo').val().trim() || "Agendamiento de Procedimiento Médico"
        };

        $('#btnGuardar').prop('disabled', true);

        fetch('/ProcedimientoAgendar/AgendarProcedimientoJson', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(dto)
        })
            .then(r => r.json())
            .then(res => {
                $('#btnGuardar').prop('disabled', false);

                if (res.esCorrecto) {
                    Swal.fire('¡Éxito!', res.mensaje || 'El procedimiento fue agendado correctamente.', 'success')
                        .then(() => {
                            window.location.href = '/ProcedimientoAgendar/MisProcedimientos';
                        });
                } else {
                    Swal.fire('Atención', res.mensaje || 'No se pudo agendar el procedimiento.', 'warning');
                }
            })
            .catch(() => {
                $('#btnGuardar').prop('disabled', false);
                Swal.fire('Error', 'Ocurrió un error inesperado al procesar la solicitud.', 'error');
            });
    },

    cargarMisProcedimientos() {
        if (!$.fn.DataTable.isDataTable('#tbMisProcedimientos')) {
            $('#tbMisProcedimientos').DataTable({
                ajax: {
                    url: '/ProcedimientoAgendar/ObtenerMisProcedimientos',
                    type: 'GET',
                    dataSrc: function (res) {
                        if (res && res.esCorrecto && Array.isArray(res.dato)) {
                            if (res.dato.length === 0) {
                                $('#sinMisProcedimientos').removeClass('d-none');
                            } else {
                                $('#sinMisProcedimientos').addClass('d-none');
                            }
                            return res.dato;
                        }

                        $('#sinMisProcedimientos').removeClass('d-none');
                        return [];
                    },
                    error: function (xhr, error, thrown) {
                        console.error('Error al cargar procedimientos:', xhr.responseText);
                        $('#sinMisProcedimientos').removeClass('d-none');
                    }
                },
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
                },
                columns: [
                    { data: 'idCita', className: 'text-center fw-bold' },
                    {
                        data: 'nombreProcedimiento',
                        render: data => `<span class="fw-semibold text-primary"><i class="bi bi-activity me-1"></i>${data || 'Procedimiento General'}</span>`
                    },
                    { data: 'nombreEspecialidad' },
                    { data: 'nombreMedico' },
                    {
                        data: 'fechaCita',
                        render: data => {
                            if (!data) return '';
                            const f = new Date(data);
                            return f.toLocaleDateString('es-ES', { year: 'numeric', month: '2-digit', day: '2-digit' });
                        }
                    },
                    {
                        data: 'hora',
                        render: h => {
                            if (!h) return '--:--';
                            if (typeof h === 'string') return h.substring(0, 5);
                            if (typeof h === 'object' && h.hours !== undefined) {
                                const hh = String(h.hours).padStart(2, '0');
                                const mm = String(h.minutes).padStart(2, '0');
                                return `${hh}:${mm}`;
                            }
                            return String(h);
                        }
                    },
                    {
                        data: 'estado',
                        className: 'text-center',
                        render: (estado, type, row) => row.estadoTexto === 'Pendiente' || estado === 1
                            ? '<span class="badge bg-success-subtle text-success border border-success px-3 py-2">Pendiente</span>'
                            : '<span class="badge bg-danger-subtle text-danger border border-danger px-3 py-2">Cancelada</span>'
                    },
                    {
                        data: null,
                        orderable: false,
                        searchable: false,
                        className: 'text-center',
                        render: (data, type, row) => {
                            let idCita = row.idCita || row.id;

                            let btnCancelar = (row.estado === 1 || row.estadoTexto === 'Pendiente')
                                ? `<button class="btn btn-sm btn-outline-danger rounded-3" onclick="Procedimientos.cancelarProcedimiento(${idCita})" title="Cancelar"><i class="bi bi-x-circle"></i></button>`
                                : '';

                            return `
                                <div class="d-flex justify-content-center gap-2">
                                    <a href="/ProcedimientoAgendar/Ticket/${idCita}" class="btn btn-sm btn-outline-info rounded-3" title="Ver Detalle">
                                        <i class="bi bi-receipt"></i>
                                    </a>
                                    ${btnCancelar}
                                </div>
                            `;
                        }
                    }
                ]
            });
        }
    },

    cancelarProcedimiento(idCita) {
        Swal.fire({
            title: '¿Está seguro?',
            text: '¿Desea cancelar este procedimiento agendado?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#6c757d',
            confirmButtonText: 'Sí, cancelar',
            cancelButtonText: 'No, mantener'
        }).then((result) => {
            if (result.isConfirmed) {
                const formData = new FormData();
                formData.append('idCita', idCita);

                fetch('/ProcedimientoAgendar/CancelarCita', {
                    method: 'POST',
                    body: formData
                })
                    .then(r => r.json())
                    .then(res => {
                        const esExito = res.esCorrecto ?? res.esExitoso ?? res.EsCorrecto;
                        const mensaje = res.mensaje ?? res.Mensaje;

                        if (esExito) {
                            Swal.fire('Cancelado', mensaje || 'El procedimiento ha sido cancelado.', 'success')
                                .then(() => {
                                    if ($.fn.DataTable.isDataTable('#tbMisProcedimientos')) {
                                        $('#tbMisProcedimientos').DataTable().ajax.reload(null, false);
                                    } else {
                                        window.location.reload();
                                    }
                                });
                        } else {
                            Swal.fire('Atención', mensaje || 'No se pudo cancelar el procedimiento.', 'warning');
                        }
                    })
                    .catch(err => {
                        console.error('Error al cancelar:', err);
                        Swal.fire('Error', 'Ocurrió un error inesperado al procesar la solicitud.', 'error');
                    });
            }
        });
    },

    limpiarSelect(element, placeholder) {
        element.empty().append(`<option value="">${placeholder}</option>`).prop('disabled', true);
    }
};

$(document).ready(function () {
    Procedimientos.init();
});