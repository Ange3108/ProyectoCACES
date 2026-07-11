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
                    render: function (estado) {

                        if (estado === 1) {
                            return `
                                <span class="badge bg-success">
                                    Pendiente
                                </span>
                            `;
                        }

                        return `
                            <span class="badge bg-secondary">
                                Cancelada
                            </span>
                        `;
                    }
                },

                {
                    data: null,
                    orderable: false,
                    searchable: false,

                    render: function (data) {

                        let botones = `
                            <a href="/Cita/Ticket/${data.idCita}"
                               class="btn btn-sm btn-outline-info me-1"
                               title="Ver ticket">

                                <i class="bi bi-receipt"></i>

                            </a>
                        `;

                        if (data.estado === 1) {

                            botones += `
                                <button type="button"
                                        class="btn btn-sm btn-outline-primary me-1"
                                        title="Reprogramar"
                                        onclick="CitasMedico.abrirModalReprogramar(
                                            ${data.idCita},
                                            '${data.fechaCita}'
                                        )">

                                    <i class="bi bi-pencil"></i>

                                </button>

                                <button type="button"
                                        class="btn btn-sm btn-outline-danger"
                                        title="Cancelar"
                                        onclick="CitasMedico.cancelarCita(
                                            ${data.idCita}
                                        )">

                                    <i class="bi bi-x-circle"></i>

                                </button>
                            `;
                        }

                        return `
                            <div class="d-flex align-items-center">
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

        $('#modalReprogramarCita').on('hidden.bs.modal', function () {

            $('#reprogramarIdCita').val('');
            $('#reprogramarFecha').val('');

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

    configurarFechaMinima() {

        const hoy = new Date();

        const fechaMinima =
            hoy.getFullYear() +
            '-' +
            String(hoy.getMonth() + 1).padStart(2, '0') +
            '-' +
            String(hoy.getDate()).padStart(2, '0');

        $('#reprogramarFecha').attr('min', fechaMinima);
    },

    mostrarError(mensaje) {

        $('#alertaCitasMedico')
            .text(mensaje)
            .removeClass('d-none');
    },

    ocultarMensajes() {

        $('#alertaCitasMedico')
            .addClass('d-none')
            .text('');

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