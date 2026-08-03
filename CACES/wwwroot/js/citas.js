const Citas = {
    tablaMisCitas: null,
    tablaGestionCitas: null,

    init() {
        this.cargarEspecialidades();
        this.cargarProcedimientos();
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
            $('#hora').val('');
        });

        $('#idMedico').on('change', function () {
            const idMedico = $(this).val();
            Citas.cargarHorarios(idMedico);
            $('#hora').val('');
        });

        $('#idHorario').on('change', function () {
            const horaInicio = $(this).find('option:selected').data('hora');
            $('#hora').val(horaInicio ? horaInicio.substring(0, 5) : '');
        });

        // Corregido: Coincide exactamente con el ID del formulario en Razor
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
                select.empty().append('<option value="">Seleccione una especialidad</option>');

                if (res.esCorrecto && Array.isArray(res.dato)) {
                    res.dato.forEach(e => {
                        select.append(`<option value="${e.id}">${e.nombre}</option>`);
                    });
                }
            })
            .catch(err => console.error('Error al cargar especialidades:', err));
    },

    cargarProcedimientos() {
        const select = $('#idProcedimiento');
        if (!select.length) return;

        fetch('/Cita/ObtenerProcedimientos')
            .then(response => {
                if (!response.ok) throw new Error('Error al obtener procedimientos');
                return response.json();
            })
            .then(res => {
                select.empty().append('<option value="">-- Seleccione un procedimiento --</option>');

                const lista = res.dato || res;

                console.log("Objetos recibidos del backend:", lista);

                if (Array.isArray(lista)) {
                    lista.forEach(p => {
                        const idVal = p.idProcedimiento
                            ?? p.idProcedimientoMedico
                            ?? p.id
                            ?? p.id_Procedimiento
                            ?? p.IdProcedimiento;

                        const textoPrecio = (p.precio !== null && p.precio !== undefined)
                            ? ` - ₡${parseFloat(p.precio).toLocaleString('es-CR')}`
                            : '';

                        select.append(`<option value="${idVal}">${p.nombre}${textoPrecio}</option>`);
                    });
                }
            })
            .catch(error => console.error('Error cargando procedimientos:', error));
    },

    cargarMedicos(idEspecialidad) {
        const select = $('#idMedico');
        if (!select.length) return;

        select.empty();

        if (!idEspecialidad) {
            select.append('<option value="">Seleccione una especialidad primero</option>');
            return;
        }

        select.append('<option value="">Cargando médicos...</option>');

        fetch(`/Cita/ObtenerMedicos?idEspecialidad=${idEspecialidad}`)
            .then(r => r.json())
            .then(res => {
                select.empty().append('<option value="">Seleccione un médico</option>');

                if (res.esCorrecto && Array.isArray(res.dato)) {
                    res.dato.forEach(m => {
                        select.append(`<option value="${m.id}">${m.nombre}</option>`);
                    });
                }
            })
            .catch(err => console.error('Error al cargar médicos:', err));
    },

    cargarHorarios(idMedico) {
        const select = $('#idHorario');
        select.empty();

        if (!idMedico) {
            select.append('<option value="">Seleccione un médico primero</option>');
            return;
        }

        select.append('<option value="">Cargando horarios...</option>');

        fetch(`/Cita/ObtenerHorariosPorMedico?idMedico=${idMedico}`)
            .then(r => r.json())
            .then(res => {
                select.empty().append('<option value="">Seleccione un horario</option>');

                if (res.esCorrecto && Array.isArray(res.dato)) {
                    res.dato.forEach(h => {
                        select.append(`
                            <option value="${h.idHorario}" data-hora="${h.horaInicio}">
                                ${h.horarioTexto}
                            </option>
                        `);
                    });
                }
            })
            .catch(err => console.error('Error al cargar horarios:', err));
    },

    registrarCita() {
        const horarioSeleccionado = $('#idHorario option:selected');
        const horaInicio = horarioSeleccionado.data('hora');

        // Obtenemos el valor del select
        const valProcedimiento = $('#idProcedimiento').val();

        // Parseamos a entero solo si es un valor numérico real
        const parsedProc = parseInt(valProcedimiento, 10);
        const idProcedimientoLimpio = !isNaN(parsedProc) ? parsedProc : null;

        const dto = {
            idEspecialidad: parseInt($('#idEspecialidad').val(), 10),
            idMedico: parseInt($('#idMedico').val(), 10),
            idHorario: parseInt($('#idHorario').val(), 10),
            idProcedimiento: idProcedimientoLimpio, // Enviará un int válido o null, NUNCA NaN
            fechaCita: $('#fechaCita').val(),
            hora: horaInicio,
            motivo: $('#motivo').val()
        };

        console.log("DTO final a enviar:", dto);

        fetch('/Cita/RegistrarCitaJson', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
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
                    const datos = (res.esCorrecto && Array.isArray(res.dato)) ? res.dato : [];

                    if (datos.length === 0) {
                        $('#sinMisCitas').removeClass('d-none');
                        $('#tbMisCitas').hide();
                    } else {
                        $('#sinMisCitas').addClass('d-none');
                        $('#tbMisCitas').show();
                    }

                    return datos;
                }
            },
            columns: [
                { data: 'idCita', className: 'text-center' },
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
                { data: 'nombreMedico' },
                { data: 'nombreEspecialidad' },
                {
                    data: 'nombreProcedimiento',
                    render: function (data) {
                        return data ? `<span class="fw-semibold text-primary">${data}</span>` : '<span class="text-muted">N/A</span>';
                    }
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
                            <a href="/Cita/Ticket/${data.idCita}" class="btn btn-sm btn-outline-info rounded-3" title="Ver ticket">
                                <i class="bi bi-receipt"></i>
                            </a>
                        `;

                        if (data.tieneReceta === true && data.idReceta) {
                            botones += `
                                <a href="/Receta/Detalle/${data.idReceta}" class="btn btn-sm btn-outline-success rounded-3" title="Ver receta">
                                    <i class="bi bi-capsule-pill"></i>
                                </a>
                            `;
                        }

                        if (data.estado === 1) {
                            botones += `
                                <button type="button" class="btn btn-sm btn-outline-danger rounded-3" title="Cancelar cita" onclick="Citas.cancelarCita(${data.idCita})">
                                    <i class="bi bi-x-circle"></i>
                                </button>
                            `;
                        }

                        return `<div class="d-flex justify-content-center gap-2">${botones}</div>`;
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
                { data: 'idCita', className: 'text-center' },
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
                { data: 'nombrePaciente' },
                { data: 'nombreMedico' },
                { data: 'nombreEspecialidad' },
                {
                    data: 'nombreProcedimiento',
                    render: function (data) {
                        return data ? `<span class="fw-semibold text-primary">${data}</span>` : '<span class="text-muted">N/A</span>';
                    }
                },
                { data: 'motivo' },
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
                                title="Editar cita"
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
                                <a href="/Receta/Detalle/${data.idReceta}" class="btn btn-sm btn-outline-success rounded-3" title="Ver receta">
                                    <i class="bi bi-capsule-pill"></i>
                                </a>
                            `;
                        }

                        if (data.estado === 1) {
                            botones += `
                                <button type="button" class="btn btn-sm btn-outline-danger rounded-3" title="Cancelar cita" onclick="Citas.cancelarCita(${data.idCita})">
                                    <i class="bi bi-x-circle"></i>
                                </button>
                            `;
                        }

                        return `<div class="d-flex justify-content-center gap-2">${botones}</div>`;
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

            fetch(`/Cita/CancelarCita?idCita=${idCita}`, { method: 'POST' })
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
        const partes = fecha.split('T')[0].split('-');
        if (partes.length === 3) {
            const [anio, mes, dia] = partes;
            return `${dia}/${mes}/${anio}`;
        }
        return fecha;
    },

    formatearHora(hora) {
        if (!hora) return '';
        return hora.substring(0, 5);
    }
};

$(document).ready(function () {
    Citas.init();
});