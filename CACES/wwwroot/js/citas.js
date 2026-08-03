const Citas = {

    tablaMisCitas: null,
    tablaGestionCitas: null,

    init() {
        this.cargarEspecialidades();
        this.registrarEventos();

        if ($('#tbMisCitas').length) {
            this.cargarMisCitas();
        }

        if ($('#tbGestionCitas').length) {
            this.cargarGestionCitas();
        }
    },

    registrarEventos() {

        $('#idEspecialidad').on('change', function () {
            const idEspecialidad = $(this).val();
            Citas.cargarMedicos(idEspecialidad);
            $('#idHorario').empty().append('<option value="">Seleccione un médico primero</option>');
        });

        $('#idMedico').on('change', function () {
            const idMedico = $(this).val();
            Citas.cargarHorarios(idMedico);
        });

        $('#idHorario').on('change', function () {
            const horaInicio = $(this).find('option:selected').data('hora');
            $('#hora').val(horaInicio ? horaInicio.substring(0, 5) : '');
        });

        $('#formRegistrarCita').on('submit', function (e) {
            e.preventDefault();
            Citas.registrarCita();
        });
    },

    cargarEspecialidades() {
        const select = $('#idEspecialidad');

        if (!select.length) return;

        fetch('/Cita/ObtenerEspecialidadesActivas')
            .then(r => r.json())
            .then(res => {
                select.empty();
                select.append('<option value="">Seleccione una especialidad</option>');

                if (res.esCorrecto) {
                    res.dato.forEach(e => {
                        select.append(`<option value="${e.id}">${e.nombre}</option>`);
                    });
                }
            });
    },

    cargarMedicos(idEspecialidad) {
        const select = $('#idMedico');

        if (!select.length) return;

        select.empty();

        if (!idEspecialidad) {
            select.append('<option value="">Seleccione una especialidad primero</option>');
            return;
        }

        select.append('<option value="">Seleccione un médico</option>');

        fetch(`/Cita/ObtenerMedicos?idEspecialidad=${idEspecialidad}`)
            .then(r => r.json())
            .then(res => {
                select.empty();
                select.append('<option value="">Seleccione un médico</option>');

                if (res.esCorrecto) {
                    res.dato.forEach(m => {
                        select.append(`<option value="${m.id}">${m.nombre}</option>`);
                    });
                }
            });
    },

    cargarHorarios(idMedico) {
        const select = $('#idHorario');

        select.empty();
        select.append('<option value="">Seleccione un horario</option>');

        if (!idMedico) return;

        fetch(`/Cita/ObtenerHorariosPorMedico?idMedico=${idMedico}`)
            .then(r => r.json())
            .then(res => {
                if (res.esCorrecto) {
                    res.dato.forEach(h => {
                        select.append(`
                            <option value="${h.idHorario}"
                                    data-hora="${h.horaInicio}">
                                ${h.horarioTexto}
                            </option>
                        `);
                    });
                }
            });
    },

    registrarCita() {

        const horarioSeleccionado = $('#idHorario option:selected');
        const horaInicio = horarioSeleccionado.data('hora');

        const dto = {
            idEspecialidad: parseInt($('#idEspecialidad').val()),
            idMedico: parseInt($('#idMedico').val()),
            idHorario: parseInt($('#idHorario').val()),
            fechaCita: $('#fechaCita').val(),
            hora: horaInicio,
            motivo: $('#motivo').val()
        };

        fetch('/Cita/RegistrarCitaJson', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(dto)
        })
            .then(r => r.json())
            .then(res => {

                if (res.esCorrecto) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Cita registrada',
                        text: res.mensaje
                    }).then(() => {
                        window.location.href = '/Cita/MisCitas';
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: res.mensaje
                    });
                }
            })
            .catch(() => {
                Swal.fire('Error', 'No se pudo registrar la cita.', 'error');
            });
    },

    cargarMisCitas() {

        this.tablaMisCitas = $('#tbMisCitas').DataTable({

            ajax: {
                url: '/Cita/ObtenerMisCitas',
                type: 'GET',

                dataSrc: function (res) {
                    return res.esCorrecto ? res.dato : [];
                }
            },

            columns: [
                {
                    data: 'idCita',
                    className: 'text-center'
                },
                {
                    data: 'fechaCita',
                    render: function (data) {
                        return Citas.formatearFecha(data);
                    }
                },
                {
                    data: 'hora',
                    render: function (data) {
                        return Citas.formatearHora(data);
                    }
                },
                {
                    data: 'nombreMedico'
                },
                {
                    data: 'nombreEspecialidad'
                },
                {
                    data: 'estado',
                    className: 'text-center',

                    render: function (estado) {
                        return estado === 1
                            ? '<span class="badge bg-success">Activa</span>'
                            : '<span class="badge bg-secondary">Cancelada</span>';
                    }
                },
                {
                    data: null,
                    orderable: false,
                    searchable: false,
                    className: 'text-center',

                    render: function (data) {

                        let botones = `
            <a href="/Cita/Ticket/${data.idCita}"
               class="btn btn-sm btn-outline-info rounded-3"
               title="Ver ticket">

                <i class="bi bi-receipt"></i>

            </a>
        `;

                        if (data.tieneReceta === true && data.idReceta) {

                            botones += `
                <a href="/Receta/Detalle/${data.idReceta}"
                   class="btn btn-sm btn-outline-success rounded-3"
                   title="Ver receta">

                    <i class="bi bi-capsule-pill"></i>

                </a>
            `;
                        }

                        if (data.estado === 1) {

                            botones += `
                <button type="button"
                        class="btn btn-sm btn-outline-danger rounded-3"
                        title="Cancelar cita"
                        onclick="Citas.cancelarCita(${data.idCita})">

                    <i class="bi bi-x-circle"></i>

                </button>
            `;
                        }

                        return `
            <div class="d-flex justify-content-center gap-2">
                ${botones}
            </div>
        `;
                    }
                }
            ],

            order: [[1, 'desc']],

            language: {
                url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json'
            }
        });
    },

    cargarGestionCitas() {

        this.tablaGestionCitas = $('#tbGestionCitas').DataTable({

            ajax: {
                url: '/Cita/ObtenerListadoCitas',
                type: 'GET',

                dataSrc: function (res) {
                    return res.esCorrecto ? res.dato : [];
                }
            },

            columns: [
                {
                    data: 'idCita',
                    className: 'text-center'
                },
                {
                    data: 'fechaCita',
                    render: function (data) {
                        return Citas.formatearFecha(data);
                    }
                },
                {
                    data: 'hora',
                    render: function (data) {
                        return Citas.formatearHora(data);
                    }
                },
                {
                    data: 'nombrePaciente'
                },
                {
                    data: 'nombreMedico'
                },
                {
                    data: 'nombreEspecialidad'
                },
                {
                    data: 'motivo'
                },
                {
                    data: 'estado',
                    className: 'text-center',

                    render: function (estado) {
                        return estado === 1
                            ? '<span class="badge bg-success">Activa</span>'
                            : '<span class="badge bg-secondary">Cancelada</span>';
                    }
                },
                {
                    data: null,
                    orderable: false,
                    searchable: false,
                    className: 'text-center',

                    render: function (data) {

                        let botones = `
                    <button type="button"
                            class="btn btn-sm btn-outline-primary rounded-3"
                            title="Editar"
                            onclick="Citas.abrirEditar(${data.idCita})">

                        <i class="bi bi-pencil-square"></i>

                    </button>

                    <a href="/Cita/Ticket/${data.idCita}"
                       class="btn btn-sm btn-outline-info rounded-3"
                       title="Ver ticket">

                        <i class="bi bi-receipt"></i>

                    </a>
                `;

                        if (data.idReceta) {

                            botones += `
                <a href="/Receta/Detalle/${data.idReceta}"
                   class="btn btn-sm btn-outline-success rounded-3"
                   title="Ver receta">

                    <i class="bi bi-capsule-pill"></i>

                </a>
            `;
                        }

                        if (data.estado === 1) {

                            botones += `
                <button type="button"
                        class="btn btn-sm btn-outline-danger rounded-3"
                        title="Cancelar cita"
                        onclick="Citas.cancelarCita(${data.idCita})">

                    <i class="bi bi-x-circle"></i>

                </button>
            `;
                        }

                        return `
            <div class="d-flex justify-content-center gap-2">
                ${botones}
            </div>
        `;
                    }
                }
            ],

            order: [[1, 'desc']],

            language: {
                url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json'
            }
        });
    },

    abrirEditar(idCita) {

        fetch(`/Cita/ObtenerCita?idCita=${idCita}`)
            .then(r => r.json())
            .then(res => {

                if (!res.esCorrecto) {
                    Swal.fire("Error", res.mensaje, "error");
                    return;
                }

                const cita = res.dato;

                $("#editarIdCita").val(cita.idCita);
                $("#editarFecha").val(cita.fechaCita.substring(0, 10));
                $("#editarMotivo").val(cita.motivo);

                Citas.cargarEspecialidadesEditar(
                    cita.idEspecialidad,
                    cita.idMedico,
                    cita.idHorario
                );

                const modal = new bootstrap.Modal(
                    document.getElementById("modalEditarCita")
                );

                modal.show();
            });

    },

    cargarEspecialidadesEditar(idEspecialidad, idMedico, idHorario) {

        fetch('/Cita/ObtenerEspecialidadesActivas')
            .then(r => r.json())
            .then(res => {

                const select = $("#editarEspecialidad");

                select.empty();

                res.dato.forEach(e => {

                    select.append(`
                    <option value="${e.id}">
                        ${e.nombre}
                    </option>
                `);

                });

                select.val(idEspecialidad);

                Citas.cargarMedicosEditar(
                    idEspecialidad,
                    idMedico,
                    idHorario
                );

            });

    },

    cargarMedicosEditar(idEspecialidad, idMedico, idHorario) {

        fetch(`/Cita/ObtenerMedicos?idEspecialidad=${idEspecialidad}`)
            .then(r => r.json())
            .then(res => {

                const select = $("#editarMedico");

                select.empty();

                res.dato.forEach(m => {

                    select.append(`
                    <option value="${m.id}">
                        ${m.nombre}
                    </option>
                `);

                });

                select.val(idMedico);

                Citas.cargarHorariosEditar(
                    idMedico,
                    idHorario
                );

            });

    },

    cargarHorariosEditar(idMedico, idHorario) {

        fetch(`/Cita/ObtenerHorariosPorMedico?idMedico=${idMedico}`)
            .then(r => r.json())
            .then(res => {

                const select = $("#editarHorario");

                select.empty();

                res.dato.forEach(h => {

                    select.append(`
                    <option value="${h.idHorario}">
                        ${h.horarioTexto}
                    </option>
                `);

                });

                select.val(idHorario);

            });

    },

    guardarEdicion() {

        const dto = {

            idCita: parseInt($("#editarIdCita").val()),
            idEspecialidad: parseInt($("#editarEspecialidad").val()),
            idMedico: parseInt($("#editarMedico").val()),
            idHorario: parseInt($("#editarHorario").val()),
            fechaCita: $("#editarFecha").val(),
            motivo: $("#editarMotivo").val()

        };

        fetch("/Cita/EditarCita", {

            method: "POST",

            headers: {
                "Content-Type": "application/json"
            },

            body: JSON.stringify(dto)

        })
            .then(r => r.json())
            .then(res => {

                if (res.esCorrecto) {

                    Swal.fire(
                        "Correcto",
                        res.mensaje,
                        "success"
                    );

                    bootstrap.Modal
                        .getInstance(
                            document.getElementById("modalEditarCita")
                        )
                        .hide();

                    Citas.tablaGestionCitas.ajax.reload();

                }
                else {

                    Swal.fire(
                        "Error",
                        res.mensaje,
                        "error"
                    );

                }

            });

    },

    cancelarCita(idCita) {

        Swal.fire({
            title: '¿Cancelar cita?',
            text: 'Esta acción cambiará el estado de la cita.',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, cancelar',
            cancelButtonText: 'No'
        }).then(result => {

            if (!result.isConfirmed) return;

            fetch(`/Cita/CancelarCita?idCita=${idCita}`, {
                method: 'POST'
            })
                .then(r => r.json())
                .then(res => {

                    if (res.esCorrecto) {
                        Swal.fire('Listo', res.mensaje, 'success');

                        if (Citas.tablaMisCitas) {
                            Citas.tablaMisCitas.ajax.reload();
                        }

                        if (Citas.tablaGestionCitas) {
                            Citas.tablaGestionCitas.ajax.reload();
                        }

                    } else {
                        Swal.fire('Error', res.mensaje, 'error');
                    }
                });
        });
    },

    formatearFecha(fecha) {
        if (!fecha) return '';
        const f = new Date(fecha);
        return f.toLocaleDateString('es-CR');
    },

    formatearHora(hora) {
        if (!hora) return '';
        return hora.substring(0, 5);
    }
};

$(document).ready(function () {
    Citas.init();
});