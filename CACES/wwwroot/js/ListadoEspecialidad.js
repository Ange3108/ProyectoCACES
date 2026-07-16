function formatearIcono(opcion) {

    if (!opcion.id) {
        return opcion.text;
    }

    const codigo = $(opcion.element).data('codigo')
        || 'bi bi-question-circle';

    return $(`
        <span>
            <i class="${codigo}" style="margin-right:8px;"></i>
            ${opcion.text}
        </span>
    `);
}

(() => {

    const Especialidad = {

        tabla: null,

        init() {
            this.inicializarTabla();
            this.registrarEventos();
        },

        inicializarTabla() {

            const tabla = $('#tbEspecialidades');

            if (!tabla.length) {
                return;
            }

            this.tabla = tabla.DataTable({

                ajax: {

                    url: '/Especialidad/ObtenerListadoEspecialidades',

                    type: 'GET',

                    dataSrc: function (respuesta) {

                        if (!respuesta ||
                            respuesta.esCorrecto === false ||
                            !Array.isArray(respuesta.dato)) {

                            return [];
                        }

                        return respuesta.dato;
                    },

                    error: function (xhr) {

                        console.error(xhr.responseText);

                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: 'No fue posible cargar las especialidades.'
                        });
                    }
                },

                columns: [

                    {
                        data: 'nombre',

                        render: function (nombre) {

                            return `
                                <span class="fw-semibold text-dark">
                                    ${nombre ?? 'Sin nombre'}
                                </span>
                            `;
                        }
                    },

                    {
                        data: 'descripcion',

                        render: function (descripcion) {

                            if (!descripcion ||
                                descripcion.trim() === '') {

                                return `
                                    <span class="text-muted">
                                        Sin descripción
                                    </span>
                                `;
                            }

                            return `
                                <span class="text-secondary">
                                    ${descripcion}
                                </span>
                            `;
                        }
                    },

                    {
                        data: 'icono',
                        orderable: false,
                        searchable: false,
                        className: 'text-center',

                        render: function (icono) {

                            const claseIcono =
                                icono && icono.trim() !== ''
                                    ? icono.trim()
                                    : 'bi bi-question-circle';

                            return `
                                <span class="d-inline-flex
                                             align-items-center
                                             justify-content-center
                                             rounded-circle"
                                      style="
                                          width:42px;
                                          height:42px;
                                          background:#DDF7F8;
                                          color:#0B6F73;
                                      ">

                                    <i class="${claseIcono} fs-5"></i>

                                </span>
                            `;
                        }
                    },

                    {
                        data: 'fechaDeRegistro',

                        render: function (fecha) {

                            if (!fecha) {
                                return `
                                    <span class="text-muted">
                                        No registrada
                                    </span>
                                `;
                            }

                            const fechaObj = new Date(fecha);

                            if (Number.isNaN(fechaObj.getTime())) {
                                return fecha;
                            }

                            return fechaObj.toLocaleDateString('es-CR');
                        }
                    },

                    {
                        data: 'estado',
                        className: 'text-center',

                        render: function (estado) {

                            return estado
                                ? `
                                    <span class="badge rounded-pill px-3 py-1"
                                          style="
                                              background:#DCFCE7;
                                              color:#166534;
                                          ">

                                        <i class="bi bi-check-circle-fill me-1"></i>
                                        Activa

                                    </span>
                                `
                                : `
                                    <span class="badge rounded-pill px-3 py-1"
                                          style="
                                              background:#FEE2E2;
                                              color:#991B1B;
                                          ">

                                        <i class="bi bi-x-circle-fill me-1"></i>
                                        Inactiva

                                    </span>
                                `;
                        }
                    },

                    {
                        data: null,
                        orderable: false,
                        searchable: false,
                        className: 'text-center',

                        render: function (data, type, row) {

                            let botones = `
                                <button type="button"
                                        class="btn btn-sm
                                               btn-outline-primary
                                               rounded-2
                                               px-2 py-1
                                               btnEditar"
                                        data-id="${row.idEspecialidad}"
                                        data-nombre="${row.nombre ?? ''}"
                                        data-idicono="${row.idIcono ?? ''}"
                                        data-descripcion="${row.descripcion ?? ''}"
                                        data-estado="${row.estado}">

                                    <i class="bi bi-pencil-square me-1"></i>
                                    Editar

                                </button>
                            `;

                            if (row.estado === true) {

                                botones += `
                                    <button type="button"
                                            class="btn btn-sm
                                                   btn-outline-danger
                                                   rounded-2
                                                   px-2 py-1
                                                   desactivar"
                                            data-id="${row.idEspecialidad}">

                                        <i class="bi bi-slash-circle me-1"></i>
                                        Desactivar

                                    </button>
                                `;
                            }

                            return `
                                <div class="d-flex
                                            justify-content-center
                                            align-items-center
                                            gap-2
                                            flex-wrap">

                                    ${botones}

                                </div>
                            `;
                        }
                    }
                ],

                order: [[0, 'asc']],

                pageLength: 10,

                responsive: true,

                autoWidth: false,

                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json',
                    emptyTable: 'No hay especialidades registradas.'
                }
            });
        },

        cargarIconos(selector, callback) {

            $.ajax({

                url: '/Icono/ObtenerIconos',

                type: 'GET',

                success: function (respuesta) {

                    const select = $(selector);

                    if (!select.length) {
                        return;
                    }

                    if (select.hasClass('select2-hidden-accessible')) {
                        select.select2('destroy');
                    }

                    select.empty();

                    select.append(`
                        <option value="">
                            Seleccione un ícono
                        </option>
                    `);

                    const iconos = Array.isArray(respuesta?.dato)
                        ? respuesta.dato
                        : [];

                    iconos.forEach(icono => {

                        select.append(`
                            <option value="${icono.idIcono}"
                                    data-codigo="${icono.codigo ?? ''}">

                                ${icono.nombre ?? 'Sin nombre'}

                            </option>
                        `);
                    });

                    select.select2({

                        width: '100%',

                        dropdownParent: select.closest('.modal'),

                        templateResult: formatearIcono,

                        templateSelection: formatearIcono,

                        language: {
                            noResults: function () {
                                return 'No se encontraron íconos';
                            }
                        }
                    });

                    if (typeof callback === 'function') {
                        callback();
                    }
                },

                error: function (xhr) {

                    console.error(xhr.responseText);

                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'No fue posible cargar los íconos.'
                    });
                }
            });
        },

        registrarEventos() {

            // ABRIR MODAL DE EDICIÓN
            $(document).on('click', '.btnEditar', function () {

                const idIconoActual = $(this).data('idicono');
                const estadoActual = $(this).data('estado');

                $('#IdEspecialidad').val(
                    $(this).data('id')
                );

                $('#Nombre').val(
                    $(this).data('nombre') ?? ''
                );

                $('#Descripcion').val(
                    $(this).data('descripcion') ?? ''
                );

                $('#Estado').val(
                    String(estadoActual)
                );

                Especialidad.cargarIconos('#Icono', function () {

                    $('#Icono')
                        .val(String(idIconoActual))
                        .trigger('change');
                });

                const elementoModal =
                    document.getElementById(
                        'editarEspecialidadModal'
                    );

                if (!elementoModal) {
                    return;
                }

                new bootstrap.Modal(elementoModal).show();
            });

            // ACTUALIZAR ESPECIALIDAD
            $('#formEditarEspecialidad').on('submit', function (e) {

                e.preventDefault();

                const formulario = $(this);

                const boton = formulario.find(
                    'button[type="submit"]'
                );

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
                                    'No fue posible actualizar la especialidad.'
                            });

                            return;
                        }

                        const elementoModal =
                            document.getElementById(
                                'editarEspecialidadModal'
                            );

                        bootstrap.Modal
                            .getInstance(elementoModal)
                            ?.hide();

                        Swal.fire({
                            icon: 'success',
                            title: 'Especialidad actualizada',
                            text: respuesta.mensaje,
                            confirmButtonText: 'Aceptar'
                        });

                        Especialidad.tabla?.ajax.reload(null, false);
                    },

                    error: function (xhr) {

                        console.error(xhr.responseText);

                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: xhr.responseJSON?.mensaje ??
                                'Ocurrió un error al actualizar la especialidad.'
                        });
                    },

                    complete: function () {

                        boton
                            .prop('disabled', false)
                            .html(`
                                <i class="bi bi-check-lg me-1"></i>
                                Guardar cambios
                            `);
                    }
                });
            });

            // DESACTIVAR ESPECIALIDAD
            $(document).on('click', '.desactivar', function () {

                const id = $(this).data('id');

                if (!id) {

                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'No se pudo identificar la especialidad.'
                    });

                    return;
                }

                Swal.fire({

                    icon: 'warning',

                    title: '¿Desactivar especialidad?',

                    text: 'La especialidad dejará de estar disponible para nuevos registros.',

                    showCancelButton: true,

                    confirmButtonText: 'Sí, desactivar',

                    cancelButtonText: 'Cancelar',

                    confirmButtonColor: '#dc3545',

                    reverseButtons: true

                }).then(resultado => {

                    if (!resultado.isConfirmed) {
                        return;
                    }

                    $.ajax({

                        url: `/Especialidad/DesactivarEspecialidad/${id}`,

                        type: 'POST',

                        success: function (respuesta) {

                            if (respuesta?.esCorrecto === false) {

                                Swal.fire({
                                    icon: 'error',
                                    title: 'Error',
                                    text: respuesta.mensaje ??
                                        'No fue posible desactivar la especialidad.'
                                });

                                return;
                            }

                            Swal.fire({
                                icon: 'success',
                                title: 'Especialidad desactivada',
                                text: respuesta?.mensaje ??
                                    'La especialidad fue desactivada correctamente.',
                                confirmButtonText: 'Aceptar'
                            });

                            Especialidad.tabla?.ajax.reload(null, false);
                        },

                        error: function (xhr) {

                            console.error(xhr.responseText);

                            Swal.fire({
                                icon: 'error',
                                title: 'Error',
                                text: xhr.responseJSON?.mensaje ??
                                    'No fue posible desactivar la especialidad.'
                            });
                        }
                    });
                });
            });
        }
    };

    $(function () {
        Especialidad.init();
    });

})();