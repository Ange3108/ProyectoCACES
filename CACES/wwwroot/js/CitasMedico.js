const CitasMedico = {

    tabla: null,

    init() {
        this.inicializarTabla();
        this.registrarEventos();
        this.configurarFechaMinima();
    },

    inicializarTabla() {

        this.tabla = $('#tbCitasMedico').DataTable({

            ajax: {
                url: '/Cita/ObtenerCitasMedico',
                type: 'GET',

                dataSrc: function (respuesta) {

                    CitasMedico.ocultarMensajes();

                    if (!respuesta || !respuesta.esCorrecto) {

                        CitasMedico.mostrarError(
                            respuesta?.mensaje ||
                            'No se pudieron obtener las citas.'
                        );

                        $('#contenedorTablaCitasMedico').addClass('d-none');

                        return [];
                    }

                    const citas = respuesta.dato || [];

                    if (citas.length === 0) {

                        $('#sinCitasMedico').removeClass('d-none');
                        $('#contenedorTablaCitasMedico').addClass('d-none');

                        return [];
                    }

                    $('#contenedorTablaCitasMedico').removeClass('d-none');

                    return citas;
                },

                error: function (xhr) {

                    CitasMedico.mostrarError(
                        'Ocurrió un error al consultar las citas médicas.'
                    );

                    $('#contenedorTablaCitasMedico').addClass('d-none');

                    console.error(xhr.responseText);
                }
            },

            columns: [

                {
                    data: 'idCita',
                    className: 'fw-semibold'
                },

                {
                    data: 'fechaCita',
                    render: function (fecha) {
                        return CitasMedico.formatearFecha(fecha);
                    }
                },

                {
                    data: 'hora',
                    render: function (hora) {
                        return CitasMedico.formatearHora(hora);
                    }
                },

                {
                    data: 'nombrePaciente',
                    defaultContent: 'No disponible'
                },

                {
                    data: 'nombreEspecialidad',
                    defaultContent: 'No disponible'
                },

                {
                    data: 'motivo',
                    defaultContent: ''
                },

                {
                    data: 'estado',
                    className: 'text-center',

                    render: function (estado) {

                        if (estado === 1) {
                            return `
                            <span class="badge bg-success-subtle text-success border border-success-subtle rounded-pill px-3 py-1">
                                Pendiente
                            </span>
                        `;
                        }

                        return `
                        <span class="badge bg-danger-subtle text-danger border border-danger-subtle rounded-pill px-3 py-1">
                            Cancelada
                        </span>
                    `;
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
                           class="btn btn-sm btn-outline-primary rounded-3">

                            <i class="bi bi-receipt me-1"></i>
                            Ticket

                        </a>
                    `;

                        if (data.estado === 1) {

                            botones += `

                            <button type="button"
                                    class="btn btn-sm btn-outline-primary rounded-3"
                                    onclick="CitasMedico.abrirModalReprogramar(
                                        ${data.idCita},
                                        '${data.fechaCita}'
                                    )">

                                <i class="bi bi-pencil-square me-1"></i>
                                Reprogramar

                            </button>

                            <button type="button"
                                    class="btn btn-sm btn-outline-success rounded-3"
                                    onclick="CitasMedico.abrirModalReceta(${data.idCita})">

                                <i class="bi bi-capsule-pill me-1"></i>
                                Recetar

                            </button>

                            <button type="button"
                                    class="btn btn-sm btn-outline-danger rounded-3"
                                    onclick="CitasMedico.cancelarCita(${data.idCita})">

                                <i class="bi bi-x-circle me-1"></i>
                                Cancelar

                            </button>
                        `;
                        }

                        return `
                        <div class="d-flex justify-content-center gap-2 flex-wrap">
                            ${botones}
                        </div>
                    `;
                    }
                }

            ],

            order: [[1, 'asc'], [2, 'asc']],

            pageLength: 10,

            responsive: true,

            language: {
                url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json',
                emptyTable: 'No hay citas médicas registradas.'
            }
        });
    },

    registrarEventos() {

        $('#btnRecargarCitas').on('click', function () {
            CitasMedico.recargarTabla();
        });

        $('#btnGuardarReprogramacion').on('click', function () {
            CitasMedico.reprogramarCita();
        });

        $('#btnGuardarReceta').on('click', function () {
            CitasMedico.guardarReceta();
        });

        $('#modalReprogramarCita').on('hidden.bs.modal', function () {

            $('#reprogramarIdCita').val('');
            $('#reprogramarFecha').val('');

        });

        $('#modalReceta').on('hidden.bs.modal', function () {

            $('#idCitaReceta').val('');
            $('#medicamentos').val('');
            $('#instrucciones').val('');
            $('#fechaVencimiento').val('');

            $('#btnGuardarReceta')
                .prop('disabled', false)
                .html(`
                <i class="bi bi-check-circle me-1"></i>
                Guardar receta
            `);
        });
    },

    abrirModalReceta(idCita) {

        $('#idCitaReceta').val(idCita);
        $('#medicamentos').val('');
        $('#instrucciones').val('');
        $('#fechaVencimiento').val('');

        const modal = new bootstrap.Modal(
            document.getElementById('modalReceta')
        );

        modal.show();
    },

    guardarReceta() {

        const idCita = parseInt(
            $('#idCitaReceta').val()
        );

        const medicamentos =
            $('#medicamentos').val().trim();

        const instrucciones =
            $('#instrucciones').val().trim();

        const fechaDeVencimiento =
            $('#fechaVencimiento').val();

        if (!idCita) {

            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'No se pudo identificar la cita.'
            });

            return;
        }

        if (!medicamentos) {

            Swal.fire({
                icon: 'warning',
                title: 'Medicamentos requeridos',
                text: 'Debe indicar los medicamentos de la receta.'
            });

            return;
        }

        if (!fechaDeVencimiento) {

            Swal.fire({
                icon: 'warning',
                title: 'Fecha requerida',
                text: 'Seleccione la fecha de vencimiento.'
            });

            return;
        }

        const dto = {
            idCita: idCita,
            medicamentos: medicamentos,
            instrucciones: instrucciones || null,
            fechaDeVencimiento: fechaDeVencimiento
        };

        const boton = $('#btnGuardarReceta');

        boton
            .prop('disabled', true)
            .html(`
            <span class="spinner-border spinner-border-sm me-2"></span>
            Guardando...
        `);

        fetch('/Receta/RegistrarJson', {

            method: 'POST',

            headers: {
                'Content-Type': 'application/json'
            },

            body: JSON.stringify(dto)

        })
            .then(async response => {

                const respuesta = await response.json();

                if (!response.ok) {
                    throw new Error(
                        respuesta.mensaje ||
                        'No se pudo registrar la receta.'
                    );
                }

                return respuesta;
            })
            .then(respuesta => {

                if (!respuesta.esCorrecto) {
                    throw new Error(
                        respuesta.mensaje ||
                        'No se pudo registrar la receta.'
                    );
                }

                const elementoModal =
                    document.getElementById('modalReceta');

                const modal =
                    bootstrap.Modal.getInstance(elementoModal);

                modal?.hide();

                Swal.fire({
                    icon: 'success',
                    title: 'Receta registrada',
                    text: respuesta.mensaje,
                    confirmButtonText: 'Aceptar'
                });

                CitasMedico.recargarTabla();
            })
            .catch(error => {

                console.error(error);

                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: error.message
                });
            })
            .finally(() => {

                boton
                    .prop('disabled', false)
                    .html(`
                    <i class="bi bi-check-circle me-1"></i>
                    Guardar receta
                `);
            });
    },
    abrirModalReprogramar(idCita, fechaActual) {

        const fecha = fechaActual
            ? fechaActual.substring(0, 10)
            : '';

        $('#reprogramarIdCita').val(idCita);
        $('#reprogramarFecha').val(fecha);

        const modal = new bootstrap.Modal(
            document.getElementById('modalReprogramarCita')
        );

        modal.show();
    },

    reprogramarCita() {

        const idCita = parseInt(
            $('#reprogramarIdCita').val()
        );

        const nuevaFecha = $('#reprogramarFecha').val();

        if (!idCita || !nuevaFecha) {

            Swal.fire({
                icon: 'warning',
                title: 'Datos incompletos',
                text: 'Seleccione una nueva fecha.'
            });

            return;
        }

        const parametros = new URLSearchParams();

        parametros.append('idCita', idCita);
        parametros.append('nuevaFecha', nuevaFecha);

        fetch('/Cita/ActualizarFechaCita', {
            method: 'POST',
            headers: {
                'Content-Type':
                    'application/x-www-form-urlencoded; charset=UTF-8'
            },
            body: parametros.toString()
        })
            .then(response => {

                if (!response.ok) {
                    throw new Error(
                        'No se pudo reprogramar la cita.'
                    );
                }

                return response.json();
            })
            .then(respuesta => {

                if (!respuesta.esCorrecto) {

                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: respuesta.mensaje
                    });

                    return;
                }

                const elementoModal =
                    document.getElementById('modalReprogramarCita');

                const modal =
                    bootstrap.Modal.getInstance(elementoModal);

                modal?.hide();

                Swal.fire({
                    icon: 'success',
                    title: 'Cita reprogramada',
                    text: respuesta.mensaje,
                    confirmButtonText: 'Aceptar'
                });

                CitasMedico.recargarTabla();
            })
            .catch(error => {

                console.error(error);

                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: error.message
                });
            });
    },

    cancelarCita(idCita) {

        Swal.fire({

            icon: 'warning',
            title: '¿Cancelar cita?',
            text: 'La cita quedará registrada como cancelada.',

            showCancelButton: true,

            confirmButtonText: 'Sí, cancelar',
            cancelButtonText: 'Volver',

            confirmButtonColor: '#dc3545'

        }).then(resultado => {

            if (!resultado.isConfirmed) {
                return;
            }

            const parametros = new URLSearchParams();

            parametros.append('idCita', idCita);

            fetch('/Cita/CancelarCita', {

                method: 'POST',

                headers: {
                    'Content-Type':
                        'application/x-www-form-urlencoded; charset=UTF-8'
                },

                body: parametros.toString()

            })
                .then(response => {

                    if (!response.ok) {
                        throw new Error(
                            'No se pudo cancelar la cita.'
                        );
                    }

                    return response.json();
                })
                .then(respuesta => {

                    if (!respuesta.esCorrecto) {

                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: respuesta.mensaje
                        });

                        return;
                    }

                    Swal.fire({
                        icon: 'success',
                        title: 'Cita cancelada',
                        text: respuesta.mensaje,
                        confirmButtonText: 'Aceptar'
                    });

                    CitasMedico.recargarTabla();
                })
                .catch(error => {

                    console.error(error);

                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: error.message
                    });
                });
        });
    },

    recargarTabla() {

        this.ocultarMensajes();

        $('#contenedorTablaCitasMedico').removeClass('d-none');

        if (this.tabla) {
            this.tabla.ajax.reload(null, false);
        }
    },

    configurarFechasMinimas() {

        const hoy = new Date();

        const fechaMinima =
            hoy.getFullYear() +
            '-' +
            String(hoy.getMonth() + 1).padStart(2, '0') +
            '-' +
            String(hoy.getDate()).padStart(2, '0');

        $('#reprogramarFecha').attr('min', fechaMinima);
        $('#fechaVencimiento').attr('min', fechaMinima);
    },

    mostrarError(mensaje) {

        const alerta = $('#alertaCitasMedico');
        const texto = $('#textoAlertaCitasMedico');

        if (texto.length) {

            texto.text(mensaje);
            alerta.removeClass('d-none');
            return;
        }

        alerta
            .text(mensaje)
            .removeClass('d-none');
    },

    ocultarMensajes() {

        $('#alertaCitasMedico')
            .addClass('d-none');

        $('#textoAlertaCitasMedico').text('');

        $('#sinCitasMedico')
            .addClass('d-none');
    },

    formatearFecha(fecha) {

        if (!fecha) {
            return '';
        }

        const partes = fecha.substring(0, 10).split('-');

        if (partes.length !== 3) {
            return fecha;
        }

        return `${partes[2]}/${partes[1]}/${partes[0]}`;
    },

    formatearHora(hora) {

        if (!hora) {
            return '';
        }

        return hora.substring(0, 5);
    }
};

$(document).ready(function () {
    CitasMedico.init();
});