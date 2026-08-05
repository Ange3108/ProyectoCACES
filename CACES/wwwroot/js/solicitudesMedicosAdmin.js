(() => {

    const SolicitudesMedicosAdmin = {

        tabla: null,

        init() {

            if (!$('#tbSolicitudesMedicos').length) {
                return;
            }

            this.cargarTabla();
            this.registrarEventos();
        },

        // =====================================================
        // TABLA
        // =====================================================

        cargarTabla() {

            this.tabla = $('#tbSolicitudesMedicos').DataTable({

                ajax: {
                    url: '/SolicitudMedico/ObtenerSolicitudes',
                    type: 'GET',

                    dataSrc: function (respuesta) {

                        SolicitudesMedicosAdmin.ocultarMensajes();

                        if (!respuesta?.esCorrecto) {

                            SolicitudesMedicosAdmin.mostrarError(
                                respuesta?.mensaje ??
                                'No fue posible cargar las solicitudes.'
                            );

                            return [];
                        }

                        const datos = Array.isArray(respuesta.dato)
                            ? respuesta.dato
                            : [];

                        if (datos.length === 0) {

                            $('#sinSolicitudes')
                                .removeClass('d-none');

                            $('#contenedorTablaSolicitudes')
                                .addClass('d-none');

                            return [];
                        }

                        $('#contenedorTablaSolicitudes')
                            .removeClass('d-none');

                        return datos;
                    },

                    error: function (xhr) {

                        console.error(xhr.responseText);

                        SolicitudesMedicosAdmin.mostrarError(
                            xhr.responseJSON?.mensaje ??
                            'Ocurrió un error al consultar las solicitudes.'
                        );

                        $('#contenedorTablaSolicitudes')
                            .addClass('d-none');
                    }
                },

                columns: [

                    {
                        data: 'fechaSolicitud',

                        render: function (fecha) {
                            return SolicitudesMedicosAdmin
                                .formatearFecha(fecha);
                        }
                    },

                    {
                        data: 'nombreCompleto',
                        defaultContent: 'No disponible',

                        render: function (valor) {

                            return `
                                <span class="fw-semibold"
                                      style="color:#0B2E59;">
                                    ${SolicitudesMedicosAdmin
                                    .escaparHtml(valor)}
                                </span>
                            `;
                        }
                    },

                    {
                        data: 'nombreEspecialidad',
                        defaultContent: 'Sin especialidad'
                    },

                    {
                        data: 'aniosExperiencia',
                        className: 'text-center',

                        render: function (valor) {

                            const anios = Number(valor) || 0;

                            return `
                                <span class="badge rounded-pill px-3 py-2"
                                      style="background:#E7F7F7;
                                             color:#148989;">
                                    ${anios} ${anios === 1 ? 'año' : 'años'}
                                </span>
                            `;
                        }
                    },

                    {
                        data: 'correoElectronico',
                        defaultContent: 'No disponible',

                        render: function (valor) {

                            return `
                                <a href="mailto:${SolicitudesMedicosAdmin
                                    .escaparHtml(valor)}"
                                   class="text-decoration-none"
                                   style="color:#0B2E59;">
                                    ${SolicitudesMedicosAdmin
                                    .escaparHtml(valor)}
                                </a>
                            `;
                        }
                    },

                    {
                        data: 'estado',
                        className: 'text-center',

                        render: function (estado) {
                            return SolicitudesMedicosAdmin
                                .renderEstado(estado);
                        }
                    },

                    {
                        data: null,
                        orderable: false,
                        searchable: false,
                        className: 'text-center',

                        render: function (data, type, fila) {

                            return `
                                <button type="button"
                                        class="btn btn-sm
                                               btn-outline-primary
                                               rounded-3
                                               btn-ver-solicitud"
                                        data-id="${fila.idSolicitud}"
                                        title="Revisar solicitud">

                                    <i class="bi bi-eye"></i>
                                </button>
                            `;
                        }
                    }
                ],

                order: [[0, 'desc']],

                pageLength: 10,

                responsive: true,

                autoWidth: false,

                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json',
                    emptyTable: 'No hay solicitudes médicas registradas.'
                }
            });
        },

        // =====================================================
        // EVENTOS
        // =====================================================

        registrarEventos() {

            $(document).on(
                'click',
                '.btn-ver-solicitud',
                function () {

                    const idSolicitud =
                        Number($(this).data('id'));

                    SolicitudesMedicosAdmin
                        .abrirDetalle(idSolicitud);
                }
            );

            $('#observacionAdministrador')
                .on('input', function () {

                    $('#contadorObservacionSolicitud')
                        .text($(this).val().length);
                });

            $('#btnAprobarSolicitud')
                .on('click', function () {

                    SolicitudesMedicosAdmin
                        .confirmarRespuesta(3);
                });

            $('#btnRechazarSolicitud')
                .on('click', function () {

                    SolicitudesMedicosAdmin
                        .confirmarRespuesta(4);
                });

            $('#modalDetalleSolicitud')
                .on('hidden.bs.modal', function () {

                    SolicitudesMedicosAdmin
                        .limpiarModal();
                });
        },

        // =====================================================
        // DETALLE
        // =====================================================

        abrirDetalle(idSolicitud) {

            if (!idSolicitud) {

                Swal.fire({
                    icon: 'error',
                    title: 'Solicitud inválida',
                    text: 'No se pudo identificar la solicitud.'
                });

                return;
            }

            Swal.fire({
                title: 'Cargando',
                text: 'Consultando la solicitud...',
                allowOutsideClick: false,

                didOpen: () => {
                    Swal.showLoading();
                }
            });

            fetch(
                `/SolicitudMedico/ObtenerSolicitud?idSolicitud=${idSolicitud}`
            )
                .then(async response => {

                    const respuesta = await response.json();

                    if (!response.ok ||
                        !respuesta?.esCorrecto ||
                        !respuesta.dato) {

                        throw new Error(
                            respuesta?.mensaje ??
                            'No fue posible obtener la solicitud.'
                        );
                    }

                    return respuesta.dato;
                })
                .then(solicitud => {

                    Swal.close();

                    SolicitudesMedicosAdmin
                        .llenarModal(solicitud);

                    const elementoModal =
                        document.getElementById(
                            'modalDetalleSolicitud'
                        );

                    if (!elementoModal) {
                        return;
                    }

                    bootstrap.Modal
                        .getOrCreateInstance(elementoModal)
                        .show();
                })
                .catch(error => {

                    Swal.close();

                    console.error(error);

                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: error.message
                    });
                });
        },

        llenarModal(solicitud) {

            $('#detalleIdSolicitud')
                .val(solicitud.idSolicitud);

            $('#detalleNombre')
                .text(
                    solicitud.nombreCompleto ??
                    'Nombre no disponible'
                );

            $('#detalleEspecialidad')
                .text(
                    solicitud.nombreEspecialidad ??
                    'Sin especialidad'
                );

            $('#detalleEspecialidadDato')
                .text(
                    solicitud.nombreEspecialidad ??
                    'Sin especialidad'
                );

            $('#detalleCorreo')
                .text(
                    solicitud.correoElectronico ??
                    'No disponible'
                );

            $('#detalleTelefono')
                .text(
                    solicitud.telefono ??
                    'No disponible'
                );

            $('#detalleFechaSolicitud')
                .text(
                    SolicitudesMedicosAdmin
                        .formatearFechaHora(
                            solicitud.fechaSolicitud
                        )
                );

            $('#detalleExperiencia')
                .text(
                    `${solicitud.aniosExperiencia ?? 0} años`
                );

            $('#detalleCertificaciones')
                .text(
                    solicitud.certificaciones?.trim()
                        ? solicitud.certificaciones
                        : 'No indicó certificaciones adicionales.'
                );

            $('#detalleMotivo')
                .text(
                    solicitud.motivo ??
                    'No disponible'
                );

            const foto =
                solicitud.foto?.trim()
                    ? solicitud.foto
                    : '/img/default.jpg';

            $('#detalleFoto')
                .attr('src', foto)
                .off('error')
                .on('error', function () {
                    $(this).attr(
                        'src',
                        '/img/default.jpg'
                    );
                });

            const curriculum =
                solicitud.curriculum?.trim();

            if (curriculum) {

                $('#btnDescargarCurriculum')
                    .attr('href', curriculum)
                    .removeClass('disabled')
                    .attr('aria-disabled', 'false');

            } else {

                $('#btnDescargarCurriculum')
                    .attr('href', '#')
                    .addClass('disabled')
                    .attr('aria-disabled', 'true');
            }

            $('#detalleEstado')
                .html(
                    SolicitudesMedicosAdmin
                        .renderEstado(solicitud.estado)
                );

            const respondida =
                Number(solicitud.estado) === 3 ||
                Number(solicitud.estado) === 4;

            $('#observacionAdministrador')
                .val(
                    solicitud.observacionAdministrador ??
                    ''
                )
                .prop('disabled', respondida);

            $('#contadorObservacionSolicitud')
                .text(
                    (solicitud.observacionAdministrador ?? '')
                        .length
                );

            if (respondida) {

                $('#respuestaRegistrada')
                    .removeClass('d-none');

                $('#textoRespuestaRegistrada')
                    .text(
                        solicitud.observacionAdministrador?.trim()
                            ? solicitud.observacionAdministrador
                            : 'Sin observación registrada.'
                    );

                $('#btnAprobarSolicitud')
                    .addClass('d-none');

                $('#btnRechazarSolicitud')
                    .addClass('d-none');

            } else {

                $('#respuestaRegistrada')
                    .addClass('d-none');

                $('#btnAprobarSolicitud')
                    .removeClass('d-none');

                $('#btnRechazarSolicitud')
                    .removeClass('d-none');
            }
        },

        // =====================================================
        // APROBAR / RECHAZAR
        // =====================================================

        confirmarRespuesta(estado) {

            const idSolicitud =
                Number($('#detalleIdSolicitud').val());

            const observacion =
                $('#observacionAdministrador')
                    .val()
                    ?.trim() ?? '';

            if (!idSolicitud) {

                Swal.fire({
                    icon: 'error',
                    title: 'Solicitud inválida',
                    text: 'No se pudo identificar la solicitud.'
                });

                return;
            }

            if (observacion.length > 500) {

                Swal.fire({
                    icon: 'warning',
                    title: 'Observación demasiado larga',
                    text: 'La observación no puede superar los 500 caracteres.'
                });

                return;
            }

            const esAprobacion =
                Number(estado) === 3;

            Swal.fire({
                icon: esAprobacion
                    ? 'question'
                    : 'warning',

                title: esAprobacion
                    ? '¿Aprobar solicitud?'
                    : '¿Rechazar solicitud?',

                text: esAprobacion
                    ? 'La solicitud quedará registrada como aprobada.'
                    : 'La solicitud quedará registrada como rechazada.',

                showCancelButton: true,

                confirmButtonText: esAprobacion
                    ? 'Sí, aprobar'
                    : 'Sí, rechazar',

                cancelButtonText: 'Cancelar',

                confirmButtonColor: esAprobacion
                    ? '#17B26A'
                    : '#DC3545'
            })
                .then(resultado => {

                    if (!resultado.isConfirmed) {
                        return;
                    }

                    SolicitudesMedicosAdmin
                        .responderSolicitud(
                            idSolicitud,
                            estado,
                            observacion
                        );
                });
        },

        responderSolicitud(
            idSolicitud,
            estado,
            observacion) {

            const boton =
                Number(estado) === 3
                    ? $('#btnAprobarSolicitud')
                    : $('#btnRechazarSolicitud');

            const textoOriginal =
                boton.html();

            boton
                .prop('disabled', true)
                .html(`
                    <span class="spinner-border
                                 spinner-border-sm
                                 me-2"></span>
                    Guardando...
                `);

            const dto = {
                idSolicitud: idSolicitud,
                estado: Number(estado),
                observacionAdministrador:
                    observacion || null
            };

            fetch(
                '/SolicitudMedico/ResponderSolicitud',
                {
                    method: 'POST',

                    headers: {
                        'Content-Type':
                            'application/json'
                    },

                    body: JSON.stringify(dto)
                }
            )
                .then(async response => {

                    let respuesta;

                    try {
                        respuesta =
                            await response.json();
                    }
                    catch {
                        throw new Error(
                            'El servidor devolvió una respuesta no válida.'
                        );
                    }

                    if (!response.ok ||
                        !respuesta?.esCorrecto) {

                        throw new Error(
                            respuesta?.mensaje ??
                            'No fue posible responder la solicitud.'
                        );
                    }

                    return respuesta;
                })
                .then(respuesta => {

                    const elementoModal =
                        document.getElementById(
                            'modalDetalleSolicitud'
                        );

                    bootstrap.Modal
                        .getInstance(elementoModal)
                        ?.hide();

                    Swal.fire({
                        icon: 'success',
                        title:
                            Number(estado) === 3
                                ? 'Solicitud aprobada'
                                : 'Solicitud rechazada',
                        text:
                            respuesta.mensaje ??
                            'La solicitud fue actualizada correctamente.',
                        confirmButtonText: 'Aceptar'
                    });

                    SolicitudesMedicosAdmin.tabla
                        ?.ajax.reload(null, false);
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
                        .html(textoOriginal);
                });
        },

        // =====================================================
        // ESTADOS
        // =====================================================

        renderEstado(estado) {

            switch (Number(estado)) {

                case 1:
                    return `
                        <span class="badge rounded-pill px-3 py-2"
                              style="background:#FEF3C7;
                                     color:#92400E;">
                            <i class="bi bi-clock-fill me-1"></i>
                            Pendiente
                        </span>
                    `;

                case 2:
                    return `
                        <span class="badge rounded-pill px-3 py-2"
                              style="background:#DBEAFE;
                                     color:#1E40AF;">
                            <i class="bi bi-search me-1"></i>
                            En revisión
                        </span>
                    `;

                case 3:
                    return `
                        <span class="badge rounded-pill px-3 py-2"
                              style="background:#DCFCE7;
                                     color:#166534;">
                            <i class="bi bi-check-circle-fill me-1"></i>
                            Aprobada
                        </span>
                    `;

                case 4:
                    return `
                        <span class="badge rounded-pill px-3 py-2"
                              style="background:#FEE2E2;
                                     color:#991B1B;">
                            <i class="bi bi-x-circle-fill me-1"></i>
                            Rechazada
                        </span>
                    `;

                default:
                    return `
                        <span class="badge bg-secondary rounded-pill">
                            Desconocido
                        </span>
                    `;
            }
        },

        // =====================================================
        // UTILIDADES
        // =====================================================

        limpiarModal() {

            $('#detalleIdSolicitud').val('');
            $('#detalleNombre').text('');
            $('#detalleEspecialidad').text('');
            $('#detalleEspecialidadDato').text('');
            $('#detalleCorreo').text('');
            $('#detalleTelefono').text('');
            $('#detalleFechaSolicitud').text('');
            $('#detalleExperiencia').text('');
            $('#detalleCertificaciones').text('');
            $('#detalleMotivo').text('');
            $('#detalleFoto').attr(
                'src',
                '/img/default.jpg'
            );

            $('#btnDescargarCurriculum')
                .attr('href', '#')
                .removeClass('disabled');

            $('#observacionAdministrador')
                .val('')
                .prop('disabled', false);

            $('#contadorObservacionSolicitud')
                .text('0');

            $('#respuestaRegistrada')
                .addClass('d-none');

            $('#textoRespuestaRegistrada')
                .text('');

            $('#btnAprobarSolicitud')
                .removeClass('d-none')
                .prop('disabled', false);

            $('#btnRechazarSolicitud')
                .removeClass('d-none')
                .prop('disabled', false);
        },

        formatearFecha(fecha) {

            if (!fecha) {
                return 'No registrada';
            }

            const fechaObj = new Date(fecha);

            if (Number.isNaN(fechaObj.getTime())) {
                return fecha;
            }

            return fechaObj.toLocaleDateString(
                'es-CR',
                {
                    day: '2-digit',
                    month: '2-digit',
                    year: 'numeric'
                }
            );
        },

        formatearFechaHora(fecha) {

            if (!fecha) {
                return 'No registrada';
            }

            const fechaObj = new Date(fecha);

            if (Number.isNaN(fechaObj.getTime())) {
                return fecha;
            }

            return fechaObj.toLocaleString(
                'es-CR',
                {
                    day: '2-digit',
                    month: '2-digit',
                    year: 'numeric',
                    hour: '2-digit',
                    minute: '2-digit'
                }
            );
        },

        escaparHtml(valor) {

            return $('<div>')
                .text(valor ?? '')
                .html();
        },

        mostrarError(mensaje) {

            $('#textoAlertaSolicitudes')
                .text(mensaje);

            $('#alertaSolicitudes')
                .removeClass('d-none');
        },

        ocultarMensajes() {

            $('#alertaSolicitudes')
                .addClass('d-none');

            $('#sinSolicitudes')
                .addClass('d-none');
        }
    };

    $(function () {
        SolicitudesMedicosAdmin.init();
    });

})();