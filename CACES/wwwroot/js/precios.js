(() => {

    const Precios = {

        tabla: null,
        precioSeleccionado: null,

        init() {

            if (!$('#tbPrecios').length) {
                return;
            }

            this.cargarTabla();
            this.registrarEventos();
        },

        // =====================================================
        // TABLA
        // =====================================================

        cargarTabla() {

            this.tabla = $('#tbPrecios').DataTable({

                ajax: {
                    url: '/Precio/ObtenerPrecios',
                    type: 'GET',

                    dataSrc: function (respuesta) {

                        Precios.ocultarMensajes();

                        if (!respuesta?.esCorrecto) {

                            Precios.mostrarError(
                                respuesta?.mensaje ??
                                'No fue posible cargar los precios.'
                            );

                            return [];
                        }

                        const datos = Array.isArray(respuesta.dato)
                            ? respuesta.dato
                            : [];

                        if (datos.length === 0) {

                            $('#sinPrecios').removeClass('d-none');
                            $('#contenedorTablaPrecios').addClass('d-none');

                            return [];
                        }

                        $('#contenedorTablaPrecios').removeClass('d-none');

                        return datos;
                    },

                    error: function (xhr) {

                        console.error(xhr.responseText);

                        Precios.mostrarError(
                            xhr.responseJSON?.mensaje ??
                            'Ocurrió un error al consultar los precios.'
                        );

                        $('#contenedorTablaPrecios').addClass('d-none');
                    }
                },

                columns: [

                    {
                        data: 'idPrecio',
                        className: 'text-center fw-semibold'
                    },

                    {
                        data: 'nombreProcedimiento',
                        defaultContent: 'No disponible',

                        render: function (valor) {

                            return `
                                <span class="fw-semibold"
                                      style="color:#0B2E59;">
                                    ${Precios.escaparHtml(valor)}
                                </span>
                            `;
                        }
                    },

                    {
                        data: 'nombreEspecialidad',
                        defaultContent: 'Sin especialidad'
                    },

                    {
                        data: 'nombreMedico',
                        defaultContent: 'Sin médico'
                    },

                    {
                        data: 'precioBase',
                        className: 'text-end fw-semibold',

                        render: function (valor) {
                            return Precios.formatearMoneda(valor);
                        }
                    },

                    {
                        data: 'honorariosMedico',
                        className: 'text-end fw-semibold',

                        render: function (valor) {

                            return `
                                <span style="color:#148989;">
                                    ${Precios.formatearMoneda(valor)}
                                </span>
                            `;
                        }
                    },

                    {
                        data: 'estado',
                        className: 'text-center',

                        render: function (estado) {

                            return estado
                                ? `
                                    <span class="badge rounded-pill px-3 py-2"
                                          style="background:#DCFCE7;color:#166534;">
                                        <i class="bi bi-check-circle-fill me-1"></i>
                                        Activo
                                    </span>
                                `
                                : `
                                    <span class="badge rounded-pill px-3 py-2"
                                          style="background:#E5E7EB;color:#4B5563;">
                                        <i class="bi bi-dash-circle-fill me-1"></i>
                                        Inactivo
                                    </span>
                                `;
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
                                        class="btn btn-sm btn-outline-primary
                                               rounded-3 btn-editar-precio"
                                        title="Editar precio"
                                        data-id="${fila.idPrecio}">

                                    <i class="bi bi-pencil-square"></i>
                                </button>
                            `;
                        }
                    }
                ],

                order: [[1, 'asc']],

                pageLength: 10,

                responsive: true,

                autoWidth: false,

                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json',
                    emptyTable: 'No hay precios médicos registrados.'
                }
            });
        },

        // =====================================================
        // EVENTOS
        // =====================================================

        registrarEventos() {

            $(document).on(
                'click',
                '.btn-editar-precio',
                function () {

                    const idPrecio = Number($(this).data('id'));

                    const fila = Precios.tabla
                        ?.row($(this).closest('tr'))
                        .data();

                    Precios.abrirEditar(idPrecio, fila);
                }
            );

            $('#editarDetalles').on('input', function () {

                $('#contadorDetallesPrecio')
                    .text($(this).val().length);
            });

            $('#btnGuardarPrecio').on('click', function () {

                Precios.guardarCambios();
            });

            $('#modalEditarPrecio').on(
                'hidden.bs.modal',
                function () {

                    Precios.limpiarModal();
                }
            );
        },

        // =====================================================
        // ABRIR MODAL
        // =====================================================

        abrirEditar(idPrecio, fila) {

            if (!idPrecio) {

                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'No se pudo identificar el precio seleccionado.'
                });

                return;
            }

            Precios.precioSeleccionado = fila ?? null;

            Swal.fire({
                title: 'Cargando',
                text: 'Consultando la información del precio...',
                allowOutsideClick: false,

                didOpen: () => {
                    Swal.showLoading();
                }
            });

            Promise.all([
                fetch(`/Precio/ObtenerPrecio?idPrecio=${idPrecio}`)
                    .then(async response => {

                        const respuesta = await response.json();

                        if (!response.ok || !respuesta?.esCorrecto) {

                            throw new Error(
                                respuesta?.mensaje ??
                                'No fue posible obtener el precio.'
                            );
                        }

                        return respuesta.dato;
                    }),

                Precios.obtenerMedicos()
            ])
                .then(([precio, medicos]) => {

                    Swal.close();

                    $('#editarIdPrecio').val(precio.idPrecio);

                    $('#editarProcedimiento').val(
                        fila?.nombreProcedimiento ?? ''
                    );

                    $('#editarEspecialidad').val(
                        fila?.nombreEspecialidad ?? ''
                    );

                    $('#editarPrecioBase').val(
                        fila?.precioBase ?? 0
                    );

                    $('#editarHonorarios').val(
                        precio.honorariosMedico ?? 0
                    );

                    $('#editarDetalles').val(
                        precio.detalles ?? ''
                    );

                    $('#editarEstado').val(
                        String(precio.estado)
                    );

                    $('#contadorDetallesPrecio').text(
                        (precio.detalles ?? '').length
                    );

                    Precios.llenarMedicos(
                        medicos,
                        precio.idMedico
                    );

                    const elementoModal =
                        document.getElementById('modalEditarPrecio');

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

        // =====================================================
        // MÉDICOS
        // =====================================================

        obtenerMedicos() {

            return fetch('/Cotizacion/ObtenerMedicos')
                .then(async response => {

                    const respuesta = await response.json();

                    if (!response.ok || !respuesta?.esCorrecto) {

                        throw new Error(
                            respuesta?.mensaje ??
                            'No fue posible cargar los médicos.'
                        );
                    }

                    return Array.isArray(respuesta.dato)
                        ? respuesta.dato
                        : [];
                });
        },

        llenarMedicos(medicos, idSeleccionado) {

            const combo = $('#editarMedico');

            combo.empty();

            if (!Array.isArray(medicos) || medicos.length === 0) {

                combo.append(`
                    <option value="">
                        No hay médicos disponibles
                    </option>
                `);

                return;
            }

            combo.append(`
                <option value="">
                    Seleccione un médico
                </option>
            `);

            medicos.forEach(medico => {

                const idMedico =
                    medico.idMedico ??
                    medico.id_Medico;

                const nombre =
                    medico.nombreCompleto ??
                    'Médico';

                const especialidad =
                    medico.nombreEspecialidad ??
                    medico.especialidad ??
                    '';

                const texto = especialidad
                    ? `${nombre} - ${especialidad}`
                    : nombre;

                combo.append(`
                    <option value="${idMedico}">
                        ${Precios.escaparHtml(texto)}
                    </option>
                `);
            });

            combo.val(String(idSeleccionado));
        },

        // =====================================================
        // GUARDAR
        // =====================================================

        guardarCambios() {

            const idPrecio =
                Number($('#editarIdPrecio').val());

            const idMedico =
                Number($('#editarMedico').val());

            const honorarios =
                Number($('#editarHonorarios').val());

            const detalles =
                $('#editarDetalles').val()?.trim() ?? '';

            const estado =
                $('#editarEstado').val() === 'true';

            if (!idPrecio) {

                Swal.fire({
                    icon: 'error',
                    title: 'Precio inválido',
                    text: 'No se pudo identificar el precio.'
                });

                return;
            }

            if (!idMedico) {

                Swal.fire({
                    icon: 'warning',
                    title: 'Médico requerido',
                    text: 'Debe seleccionar un médico.'
                });

                return;
            }

            if (!honorarios || honorarios <= 0) {

                Swal.fire({
                    icon: 'warning',
                    title: 'Honorarios inválidos',
                    text: 'Los honorarios deben ser mayores a cero.'
                });

                return;
            }

            if (!detalles) {

                Swal.fire({
                    icon: 'warning',
                    title: 'Detalles requeridos',
                    text: 'Debe indicar los detalles del precio.'
                });

                return;
            }

            if (detalles.length > 100) {

                Swal.fire({
                    icon: 'warning',
                    title: 'Texto demasiado largo',
                    text: 'Los detalles no pueden superar los 100 caracteres.'
                });

                return;
            }

            const dto = {
                idPrecio: idPrecio,
                idMedico: idMedico,
                honorariosMedico: honorarios,
                detalles: detalles,
                estado: estado
            };

            const boton = $('#btnGuardarPrecio');

            boton
                .prop('disabled', true)
                .html(`
                    <span class="spinner-border
                                 spinner-border-sm
                                 me-2"></span>
                    Guardando...
                `);

            fetch('/Precio/ActualizarPrecio', {

                method: 'POST',

                headers: {
                    'Content-Type': 'application/json'
                },

                body: JSON.stringify(dto)
            })
                .then(async response => {

                    let respuesta;

                    try {
                        respuesta = await response.json();
                    }
                    catch {
                        throw new Error(
                            'El servidor devolvió una respuesta no válida.'
                        );
                    }

                    if (!response.ok || !respuesta?.esCorrecto) {

                        throw new Error(
                            respuesta?.mensaje ??
                            'No fue posible actualizar el precio.'
                        );
                    }

                    return respuesta;
                })
                .then(respuesta => {

                    const elementoModal =
                        document.getElementById('modalEditarPrecio');

                    bootstrap.Modal
                        .getInstance(elementoModal)
                        ?.hide();

                    Swal.fire({
                        icon: 'success',
                        title: 'Precio actualizado',
                        text:
                            respuesta.mensaje ??
                            'Los cambios se guardaron correctamente.',
                        confirmButtonText: 'Aceptar'
                    });

                    Precios.tabla
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
                        .html(`
                            <i class="bi bi-check-circle me-1"></i>
                            Guardar cambios
                        `);
                });
        },

        // =====================================================
        // UTILIDADES
        // =====================================================

        limpiarModal() {

            $('#editarIdPrecio').val('');
            $('#editarProcedimiento').val('');
            $('#editarEspecialidad').val('');
            $('#editarPrecioBase').val('');
            $('#editarMedico').empty();
            $('#editarHonorarios').val('');
            $('#editarDetalles').val('');
            $('#editarEstado').val('true');
            $('#contadorDetallesPrecio').text('0');

            this.precioSeleccionado = null;
        },

        formatearMoneda(valor) {

            return new Intl.NumberFormat(
                'en-US',
                {
                    style: 'currency',
                    currency: 'USD',
                    minimumFractionDigits: 2
                }
            ).format(Number(valor) || 0);
        },

        escaparHtml(valor) {

            return $('<div>')
                .text(valor ?? '')
                .html();
        },

        mostrarError(mensaje) {

            $('#textoAlertaPrecios').text(mensaje);
            $('#alertaPrecios').removeClass('d-none');
        },

        ocultarMensajes() {

            $('#alertaPrecios').addClass('d-none');
            $('#sinPrecios').addClass('d-none');
        }
    };

    $(function () {
        Precios.init();
    });

})();