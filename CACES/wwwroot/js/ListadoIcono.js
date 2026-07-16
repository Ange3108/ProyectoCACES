(() => {

    const Icono = {

        tabla: null,

        init() {
            this.inicializarTabla();
            this.registrarEventos();
        },

        inicializarTabla() {

            const tabla = $('#tbIconos');

            if (!tabla.length) {
                return;
            }

            this.tabla = tabla.DataTable({

                ajax: {
                    url: '/Icono/ObtenerIconos',
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
                            text: 'No fue posible cargar los íconos.'
                        });
                    }
                },

                columns: [

                    {
                        data: 'codigo',
                        orderable: false,
                        searchable: false,
                        className: 'text-center',

                        render: function (codigo) {

                            const claseIcono =
                                codigo && codigo.trim() !== ''
                                    ? codigo.trim()
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
                        data: 'codigo',

                        render: function (codigo) {

                            if (!codigo || codigo.trim() === '') {

                                return `
                                    <span class="text-muted">
                                        No registrado
                                    </span>
                                `;
                            }

                            return `
                                <code class="px-2 py-1 rounded-2"
                                      style="
                                          background:#F1F5F9;
                                          color:#334155;
                                      ">
                                    ${codigo}
                                </code>
                            `;
                        }
                    },

                    {
                        data: null,
                        orderable: false,
                        searchable: false,
                        className: 'text-center',

                        render: function (data, type, row) {

                            return `
                                <button type="button"
                                        class="btn btn-sm
                                               btn-outline-primary
                                               rounded-2
                                               px-2 py-1
                                               btnEditar"
                                        data-id="${row.idIcono}"
                                        data-nombre="${row.nombre ?? ''}"
                                        data-codigo="${row.codigo ?? ''}">

                                    <i class="bi bi-pencil-square me-1"></i>
                                    Editar

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
                    url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json',
                    emptyTable: 'No hay íconos registrados.'
                }
            });
        },

        registrarEventos() {

            // Vista previa del ícono al registrar
            $(document).on('input', '#CodigoRegistro', function () {

                const codigo = $(this).val()?.trim();

                $('#previewRegistro')
                    .attr(
                        'class',
                        codigo || 'bi bi-question-circle'
                    );
            });

            // Vista previa del ícono al editar
            $(document).on('input', '#CodigoEditar', function () {

                const codigo = $(this).val()?.trim();

                $('#previewEditar')
                    .attr(
                        'class',
                        codigo || 'bi bi-question-circle'
                    );
            });

            // Abrir modal de edición
            $(document).on('click', '.btnEditar', function () {

                const id = $(this).data('id');
                const nombre = $(this).data('nombre') ?? '';
                const codigo = $(this).data('codigo') ?? '';

                $('#IdIcono').val(id);
                $('#NombreEditar').val(nombre);
                $('#CodigoEditar').val(codigo);

                $('#previewEditar')
                    .attr(
                        'class',
                        codigo || 'bi bi-question-circle'
                    );

                const elementoModal =
                    document.getElementById('editarIconoModal');

                if (!elementoModal) {
                    return;
                }

                new bootstrap.Modal(elementoModal).show();
            });

            // Registrar ícono
            $('#formRegistroIcono').on('submit', function (e) {

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
                                    'No fue posible registrar el ícono.'
                            });

                            return;
                        }

                        const elementoModal =
                            document.getElementById(
                                'registroIconoModal'
                            );

                        bootstrap.Modal
                            .getInstance(elementoModal)
                            ?.hide();

                        formulario[0].reset();

                        $('#previewRegistro')
                            .attr(
                                'class',
                                'bi bi-question-circle'
                            );

                        Swal.fire({
                            icon: 'success',
                            title: 'Ícono registrado',
                            text: respuesta.mensaje,
                            confirmButtonText: 'Aceptar'
                        });

                        Icono.tabla?.ajax.reload(null, false);
                    },

                    error: function (xhr) {

                        console.error(xhr.responseText);

                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: xhr.responseJSON?.mensaje ??
                                'No fue posible registrar el ícono.'
                        });
                    },

                    complete: function () {

                        boton
                            .prop('disabled', false)
                            .html(`
                                <i class="bi bi-check-lg me-1"></i>
                                Guardar
                            `);
                    }
                });
            });

            // Actualizar ícono
            $('#formEditarIcono').on('submit', function (e) {

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
                                    'No fue posible actualizar el ícono.'
                            });

                            return;
                        }

                        const elementoModal =
                            document.getElementById(
                                'editarIconoModal'
                            );

                        bootstrap.Modal
                            .getInstance(elementoModal)
                            ?.hide();

                        Swal.fire({
                            icon: 'success',
                            title: 'Ícono actualizado',
                            text: respuesta.mensaje,
                            confirmButtonText: 'Aceptar'
                        });

                        Icono.tabla?.ajax.reload(null, false);
                    },

                    error: function (xhr) {

                        console.error(xhr.responseText);

                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: xhr.responseJSON?.mensaje ??
                                'No fue posible actualizar el ícono.'
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
        }
    };

    $(function () {
        Icono.init();
    });

})();