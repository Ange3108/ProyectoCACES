(() => {

    const Cotizaciones = {

        tablaGestion: null,
        tablaMisCotizaciones: null,

        init() {

            if ($('#formCotizacion').length) {
                this.cargarProcedimientos();
                this.cargarMedicos();
                this.registrarSolicitud();
            }

            if ($('#tbGestionCotizaciones').length) {
                this.cargarGestionCotizaciones();
                this.registrarEventosGestion();
            }

            if ($('#tbMisCotizaciones').length) {
                this.cargarMisCotizaciones();
            }
        },

        // =====================================================
        // SOLICITAR COTIZACIÓN
        // =====================================================

        cargarProcedimientos() {

            const combo = $('#idProcedimiento');

            if (!combo.length) {
                return;
            }

            combo
                .prop('disabled', true)
                .empty()
                .append(`
            <option value="">
                Cargando procedimientos...
            </option>
        `);

            $.ajax({

                url: '/Cotizacion/ObtenerProcedimientos',
                type: 'GET',

                success: function (respuesta) {

                    combo.empty();

                    combo.append(`
                <option value="">
                    Seleccione un procedimiento
                </option>
            `);

                    if (!respuesta?.esCorrecto ||
                        !Array.isArray(respuesta.dato)) {

                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: respuesta?.mensaje ??
                                'No fue posible cargar los procedimientos.'
                        });

                        return;
                    }

                    respuesta.dato.forEach(procedimiento => {

                        const idProcedimiento =
                            procedimiento.idProcedimiento ??
                            procedimiento.id_Procedimiento;

                        const nombre =
                            procedimiento.nombre ??
                            'Procedimiento';

                        if (!idProcedimiento) {
                            return;
                        }

                        combo.append(`
                    <option value="${idProcedimiento}">
                        ${nombre}
                    </option>
                `);
                    });
                },

                error: function (xhr) {

                    console.error(xhr.responseText);

                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: xhr.responseJSON?.mensaje ??
                            'No fue posible cargar los procedimientos.'
                    });
                },

                complete: function () {
                    combo.prop('disabled', false);
                }
            });
        }, 

        cargarMedicos() {

            const combo = $('#idMedico');

            if (!combo.length) {
                return;
            }

            combo
                .prop('disabled', true)
                .empty()
                .append(`
            <option value="">
                Cargando médicos...
            </option>
        `);

            $.ajax({

                url: '/Cotizacion/ObtenerMedicos',
                type: 'GET',

                success: function (respuesta) {

                    combo.empty();

                    combo.append(`
                <option value="">
                    Seleccione un médico
                </option>
            `);

                    if (!respuesta?.esCorrecto ||
                        !Array.isArray(respuesta.dato)) {

                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: respuesta?.mensaje ??
                                'No fue posible cargar los médicos.'
                        });

                        return;
                    }

                    respuesta.dato.forEach(medico => {

                        const idMedico =
                            medico.idMedico ??
                            medico.id_Medico;

                        const nombre =
                            medico.nombreCompleto ??
                            [
                                medico.nombres ??
                                medico.usuario?.nombres ??
                                '',

                                medico.primerApellido ??
                                medico.usuario?.primerApellido ??
                                '',

                                medico.segundoApellido ??
                                medico.usuario?.segundoApellido ??
                                ''
                            ]
                                .join(' ')
                                .replace(/\s+/g, ' ')
                                .trim();

                        const especialidad =
                            medico.nombreEspecialidad ??
                            medico.especialidad?.nombre ??
                            '';

                        if (!idMedico) {
                            return;
                        }

                        const texto = especialidad
                            ? `${nombre} - ${especialidad}`
                            : nombre;

                        combo.append(`
                    <option value="${idMedico}">
                        ${texto || 'Médico'}
                    </option>
                `);
                    });
                },

                error: function (xhr) {

                    console.error(xhr.responseText);

                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: xhr.responseJSON?.mensaje ??
                            'No fue posible cargar los médicos.'
                    });
                },

                complete: function () {
                    combo.prop('disabled', false);
                }
            });
        },

        registrarSolicitud() {

            $('#formCotizacion').on('submit', function (e) {

                e.preventDefault();

                const formulario = $(this);
                const boton = formulario.find(
                    'button[type="submit"]'
                );

                const idProcedimiento =
                    parseInt($('#idProcedimiento').val());

                const idMedico =
                    parseInt($('#idMedico').val());

                if (!idProcedimiento || !idMedico) {

                    Swal.fire({
                        icon: 'warning',
                        title: 'Datos incompletos',
                        text: 'Seleccione un procedimiento y un médico.'
                    });

                    return;
                }

                const dto = {
                    idMedico: idMedico,
                    idProcedimiento: idProcedimiento,
                    observaciones:
                        $('#ObservacionesSolicitud').val()?.trim() || null
                };

                boton
                    .prop('disabled', true)
                    .html(`
                        <span class="spinner-border
                                     spinner-border-sm
                                     me-2"></span>
                        Enviando...
                    `);

                fetch('/Cotizacion/RegistrarCotizacion', {

                    method: 'POST',

                    headers: {
                        'Content-Type': 'application/json',
                        'RequestVerificationToken':
                            $('input[name="__RequestVerificationToken"]').val()
                    },

                    body: JSON.stringify(dto)

                })
                    .then(async response => {

                        const respuesta = await response.json();

                        if (!response.ok) {
                            throw new Error(
                                respuesta?.mensaje ??
                                'No fue posible registrar la solicitud.'
                            );
                        }

                        return respuesta;
                    })
                    .then(respuesta => {

                        if (!respuesta?.esCorrecto) {
                            throw new Error(
                                respuesta?.mensaje ??
                                'No fue posible registrar la solicitud.'
                            );
                        }

                        Swal.fire({
                            icon: 'success',
                            title: 'Solicitud enviada',
                            text: respuesta.mensaje,
                            confirmButtonText: 'Aceptar'
                        }).then(() => {

                            window.location.href =
                                '/Cotizacion/MisCotizaciones';
                        });
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
                                <i class="bi bi-send me-2"></i>
                                Solicitar Cotización
                            `);
                    });
            });
        },

        // =====================================================
        // GESTIÓN DE COTIZACIONES
        // =====================================================

        cargarGestionCotizaciones() {

            const tabla = $('#tbGestionCotizaciones');

            if (!tabla.length) {
                return;
            }

            this.tablaGestion = tabla.DataTable({

                ajax: {

                    url: '/Cotizacion/ObtenerCotizaciones',
                    type: 'GET',

                    dataSrc: function (respuesta) {

                        Cotizaciones.ocultarMensajes();

                        if (!respuesta?.esCorrecto) {

                            Cotizaciones.mostrarError(
                                respuesta?.mensaje ??
                                'No fue posible cargar las cotizaciones.'
                            );

                            return [];
                        }

                        const cotizaciones =
                            Array.isArray(respuesta.dato)
                                ? respuesta.dato
                                : [];

                        if (cotizaciones.length === 0) {

                            $('#sinCotizaciones')
                                .removeClass('d-none');

                            $('#contenedorTablaCotizaciones')
                                .addClass('d-none');

                            return [];
                        }

                        $('#contenedorTablaCotizaciones')
                            .removeClass('d-none');

                        return cotizaciones;
                    },

                    error: function (xhr) {

                        console.error(xhr.responseText);

                        Cotizaciones.mostrarError(
                            xhr.responseJSON?.mensaje ??
                            'No fue posible consultar las cotizaciones.'
                        );

                        $('#contenedorTablaCotizaciones')
                            .addClass('d-none');
                    }
                },

                columns: [

                    {
                        data: 'idCotizacion',
                        className: 'fw-semibold text-center'
                    },

                    {
                        data: 'fechaSolicitud',

                        render: function (fecha) {
                            return Cotizaciones.formatearFecha(fecha);
                        }
                    },

                    {
                        data: 'nombrePaciente',
                        defaultContent: 'No disponible'
                    },

                    {
                        data: 'nombreMedico',
                        defaultContent: 'No disponible'
                    },

                    {
                        data: 'nombreProcedimiento',
                        defaultContent: 'No disponible'
                    },

                    {
                        data: 'total',
                        className: 'text-end fw-semibold',

                        render: function (total) {

                            if (!total || Number(total) <= 0) {

                                return `
                                    <span class="text-muted">
                                        Pendiente
                                    </span>
                                `;
                            }

                            return Cotizaciones.formatearMoneda(total);
                        }
                    },

                    {
                        data: 'estado',
                        className: 'text-center',

                        render: function (estado) {
                            return Cotizaciones.renderEstado(estado);
                        }
                    },

                    {
                        data: null,
                        orderable: false,
                        searchable: false,
                        className: 'text-center',

                        render: function (data, type, row) {

                            return `
                                <div class="d-flex
                                            justify-content-center
                                            align-items-center
                                            gap-2
                                            flex-wrap">

                                    <button type="button"
                                            class="btn btn-sm
                                                   btn-outline-primary
                                                   rounded-2 px-2 py-1
                                                   btnEditarCotizacion"
                                            data-id="${row.idCotizacion}">

                                        <i class="bi bi-pencil-square me-1"></i>
                                        Editar

                                    </button>

                                    <a href="/Cotizacion/Detalle/${row.idCotizacion}"
                                       class="btn btn-sm
                                              btn-outline-info
                                              rounded-2 px-2 py-1">

                                        <i class="bi bi-eye me-1"></i>
                                        Ver

                                    </a>

                                </div>
                            `;
                        }
                    }
                ],

                order: [[1, 'desc']],

                pageLength: 10,

                responsive: true,

                autoWidth: false,

                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json',
                    emptyTable: 'No hay cotizaciones registradas.'
                }
            });
        },

        registrarEventosGestion() {

            $(document).on(
                'click',
                '.btnEditarCotizacion',
                function () {

                    const idCotizacion =
                        parseInt($(this).data('id'));

                    Cotizaciones.abrirModalEditar(
                        idCotizacion
                    );
                }
            );

            $('#PrecioBase, #Descuento, #Impuesto')
                .on('input', function () {

                    Cotizaciones.calcularTotal();
                });

            $('#Observaciones').on('input', function () {

                $('#contadorObservaciones')
                    .text($(this).val().length);
            });

            $('#formEditarCotizacion')
                .on('submit', function (e) {

                    e.preventDefault();

                    Cotizaciones.actualizarCotizacion(
                        $(this)
                    );
                });

            $('#editarCotizacionModal')
                .on('hidden.bs.modal', function () {

                    const formulario =
                        $('#formEditarCotizacion')[0];

                    if (formulario) {
                        formulario.reset();
                    }

                    $('#IdCotizacion').val('');
                    $('#contadorObservaciones').text('0');
                    $('#totalCotizacion').text('₡0,00');
                });
        },

        abrirModalEditar(idCotizacion) {

            if (!idCotizacion) {

                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'No se pudo identificar la cotización.'
                });

                return;
            }

            Swal.fire({
                title: 'Cargando',
                text: 'Consultando la cotización...',
                allowOutsideClick: false,

                didOpen: () => {
                    Swal.showLoading();
                }
            });

            $.ajax({

                url: '/Cotizacion/ObtenerCotizacion',
                type: 'GET',

                data: {
                    id: idCotizacion
                },

                success: function (respuesta) {

                    Swal.close();

                    if (!respuesta?.esCorrecto ||
                        !respuesta.dato) {

                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: respuesta?.mensaje ??
                                'No fue posible obtener la cotización.'
                        });

                        return;
                    }

                    const cotizacion = respuesta.dato;

                    $('#IdCotizacion')
                        .val(cotizacion.idCotizacion);

                    $('#PrecioBase')
                        .val(cotizacion.precioBase ?? 0);

                    $('#Descuento')
                        .val(cotizacion.descuento ?? 0);

                    $('#Impuesto')
                        .val(cotizacion.impuesto ?? 0);

                    $('#Observaciones')
                        .val(cotizacion.observaciones ?? '');

                    $('#Estado')
                        .val(String(cotizacion.estado));

                    $('#contadorObservaciones')
                        .text(
                            (cotizacion.observaciones ?? '')
                                .length
                        );

                    Cotizaciones.calcularTotal();

                    const elementoModal =
                        document.getElementById(
                            'editarCotizacionModal'
                        );

                    if (!elementoModal) {
                        return;
                    }

                    new bootstrap.Modal(
                        elementoModal
                    ).show();
                },

                error: function (xhr) {

                    Swal.close();

                    console.error(xhr.responseText);

                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: xhr.responseJSON?.mensaje ??
                            'No fue posible obtener la cotización.'
                    });
                }
            });
        },

        calcularTotal() {

            const precioBase =
                parseFloat($('#PrecioBase').val()) || 0;

            const descuento =
                parseFloat($('#Descuento').val()) || 0;

            const impuesto =
                parseFloat($('#Impuesto').val()) || 0;

            const total = Math.max(
                precioBase - descuento + impuesto,
                0
            );

            $('#totalCotizacion')
                .text(
                    Cotizaciones.formatearMoneda(total)
                );

            return total;
        },

        actualizarCotizacion(formulario) {

            const precioBase =
                parseFloat($('#PrecioBase').val()) || 0;

            const descuento =
                parseFloat($('#Descuento').val()) || 0;

            const impuesto =
                parseFloat($('#Impuesto').val()) || 0;

            if (precioBase <= 0) {

                Swal.fire({
                    icon: 'warning',
                    title: 'Precio requerido',
                    text: 'El precio base debe ser mayor que cero.'
                });

                return;
            }

            if (descuento < 0 || impuesto < 0) {

                Swal.fire({
                    icon: 'warning',
                    title: 'Montos inválidos',
                    text: 'El descuento y el impuesto no pueden ser negativos.'
                });

                return;
            }

            if (descuento > precioBase) {

                Swal.fire({
                    icon: 'warning',
                    title: 'Descuento inválido',
                    text: 'El descuento no puede superar el precio base.'
                });

                return;
            }

            const boton = $('#btnGuardarCotizacion');

            boton
                .prop('disabled', true)
                .html(`
                    <span class="spinner-border
                                 spinner-border-sm
                                 me-2"></span>
                    Guardando...
                `);

            $.ajax({

                url: formulario.attr('action'),
                type: 'POST',
                data: formulario.serialize(),

                success: function (respuesta) {

                    if (!respuesta?.esCorrecto) {

                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: respuesta?.mensaje ??
                                'No fue posible actualizar la cotización.'
                        });

                        return;
                    }

                    const elementoModal =
                        document.getElementById(
                            'editarCotizacionModal'
                        );

                    bootstrap.Modal
                        .getInstance(elementoModal)
                        ?.hide();

                    Swal.fire({
                        icon: 'success',
                        title: 'Cotización actualizada',
                        text: respuesta.mensaje,
                        confirmButtonText: 'Aceptar'
                    });

                    Cotizaciones.tablaGestion
                        ?.ajax.reload(null, false);
                },

                error: function (xhr) {

                    console.error(xhr.responseText);

                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: xhr.responseJSON?.mensaje ??
                            'No fue posible actualizar la cotización.'
                    });
                },

                complete: function () {

                    boton
                        .prop('disabled', false)
                        .html(`
                            <i class="bi bi-check-lg me-1"></i>
                            Guardar cotización
                        `);
                }
            });
        },

        // =====================================================
        // MIS COTIZACIONES
        // =====================================================

        cargarMisCotizaciones() {

            const tabla = $('#tbMisCotizaciones');

            if (!tabla.length) {
                return;
            }

            const url =
                '/Cotizacion/ObtenerMisCotizaciones';

            this.tablaMisCotizaciones =
                tabla.DataTable({

                    ajax: {

                        url: url,
                        type: 'GET',

                        dataSrc: function (respuesta) {

                            return respuesta?.esCorrecto &&
                                Array.isArray(respuesta.dato)
                                ? respuesta.dato
                                : [];
                        },

                        error: function (xhr) {

                            console.error(xhr.responseText);

                            Swal.fire({
                                icon: 'error',
                                title: 'Error',
                                text: xhr.responseJSON?.mensaje ??
                                    'No fue posible cargar sus cotizaciones.'
                            });
                        }
                    },

                    columns: [

                        {
                            data: 'fechaSolicitud',

                            render: function (fecha) {
                                return Cotizaciones
                                    .formatearFecha(fecha);
                            }
                        },

                        {
                            data: 'nombreProcedimiento',
                            defaultContent: 'No disponible'
                        },

                        {
                            data: 'nombreMedico',
                            defaultContent: 'No disponible'
                        },

                        {
                            data: 'total',
                            className: 'text-end fw-semibold',

                            render: function (total) {

                                return Number(total) > 0
                                    ? Cotizaciones
                                        .formatearMoneda(total)
                                    : `
                                        <span class="text-muted">
                                            Pendiente
                                        </span>
                                    `;
                            }
                        },

                        {
                            data: 'estado',
                            className: 'text-center',

                            render: function (estado) {
                                return Cotizaciones
                                    .renderEstado(estado);
                            }
                        },

                        {
                            data: null,
                            orderable: false,
                            searchable: false,
                            className: 'text-center',

                            render: function (data, type, row) {

                                return `
                                    <a href="/Cotizacion/Detalle/${row.idCotizacion}"
                                       class="btn btn-sm
                                              btn-outline-primary
                                              rounded-2 px-2 py-1">

                                        <i class="bi bi-eye me-1"></i>
                                        Ver

                                    </a>
                                `;
                            }
                        }
                    ],

                    order: [[0, 'desc']],

                    pageLength: 10,

                    responsive: true,

                    autoWidth: false,

                    language: {
                        url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json',
                        emptyTable: 'No tiene cotizaciones registradas.'
                    }
                });
        },

        // =====================================================
        // UTILIDADES
        // =====================================================

        renderEstado(estado) {

            switch (Number(estado)) {

                case 1:

                    return `
                        <span class="badge rounded-pill px-3 py-1"
                              style="background:#FEF3C7;
                                     color:#92400E;">

                            <i class="bi bi-clock-fill me-1"></i>
                            Pendiente

                        </span>
                    `;

                case 2:

                    return `
                        <span class="badge rounded-pill px-3 py-1"
                              style="background:#DBEAFE;
                                     color:#1E40AF;">

                            <i class="bi bi-send-check-fill me-1"></i>
                            Enviada

                        </span>
                    `;

                case 3:

                    return `
                        <span class="badge rounded-pill px-3 py-1"
                              style="background:#DCFCE7;
                                     color:#166534;">

                            <i class="bi bi-check-circle-fill me-1"></i>
                            Aceptada

                        </span>
                    `;

                case 4:

                    return `
                        <span class="badge rounded-pill px-3 py-1"
                              style="background:#FEE2E2;
                                     color:#991B1B;">

                            <i class="bi bi-x-circle-fill me-1"></i>
                            Rechazada

                        </span>
                    `;

                default:

                    return `
                        <span class="badge
                                     bg-secondary
                                     rounded-pill">

                            Desconocido

                        </span>
                    `;
            }
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

        formatearMoneda(valor) {

            return new Intl.NumberFormat(
                'es-CR',
                {
                    style: 'currency',
                    currency: 'CRC',
                    minimumFractionDigits: 2
                }
            ).format(Number(valor) || 0);
        },

        mostrarError(mensaje) {

            $('#textoAlertaCotizaciones')
                .text(mensaje);

            $('#alertaCotizaciones')
                .removeClass('d-none');
        },

        ocultarMensajes() {

            $('#alertaCotizaciones')
                .addClass('d-none');

            $('#sinCotizaciones')
                .addClass('d-none');
        }
    };

    $(function () {
        Cotizaciones.init();
    });

})();