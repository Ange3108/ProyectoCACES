(() => {

    const Usuario = {

        tabla: null,

        init() {
            this.inicializarTabla();
            this.registrarEventos();
        },

        inicializarTabla() {

            this.tabla = $('#tbUsuarios').DataTable({

                ajax: {
                    url: '/Usuario/ObtenerUsuarios',
                    type: 'GET',
                    dataSrc: 'dato'
                },

                columns: [

                    {
                        data: null,
                        render: function (data) {

                            const nombreCompleto = `
                                ${data.nombres ?? ''}
                                ${data.primerApellido ?? ''}
                                ${data.segundoApellido ?? ''}
                            `.replace(/\s+/g, ' ').trim();

                            const foto = data.foto && data.foto.trim() !== ''
                                ? data.foto
                                : '/img/default.jpg';

                            return `
                                <div class="d-flex align-items-center gap-2">
                                    <img src="${foto}"
                                         alt="${nombreCompleto}"
                                         class="rounded-circle border"
                                         style="width:38px;height:38px;object-fit:cover;"
                                         onerror="this.onerror=null;this.src='/img/default.jpg';" />

                                    <span class="fw-semibold">
                                        ${nombreCompleto}
                                    </span>
                                </div>
                            `;
                        }
                    },

                    {
                        data: 'correoElectronico',
                        render: function (data) {
                            return `
                                <span class="text-secondary">
                                    <i class="bi bi-envelope me-1"></i>
                                    ${data ?? ''}
                                </span>
                            `;
                        }
                    },

                    {
                        data: 'dui',
                        defaultContent: ''
                    },

                    {
                        data: 'telefono',
                        defaultContent: ''
                    },

                    {
                        data: 'direccion',
                        defaultContent: ''
                    },

                    {
                        data: 'nacimiento',
                        render: function (data) {

                            if (!data) {
                                return '<span class="text-muted">No registrada</span>';
                            }

                            const fecha = new Date(data);

                            return fecha.toLocaleDateString('es-CR');
                        }
                    },

                    {
                        data: 'estado',
                        className: 'text-center',

                        render: function (estado) {

                            return estado
                                ? `
                                    <span class="badge rounded-pill px-3 py-1"
                                          style="background:#DDF7F8;color:#0B6F73;">
                                        <i class="bi bi-check-circle-fill me-1"></i>
                                        Activo
                                    </span>
                                `
                                : `
                                    <span class="badge rounded-pill px-3 py-1"
                                          style="background:#FEE2E2;color:#991B1B;">
                                        <i class="bi bi-x-circle-fill me-1"></i>
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

                        render: function (data, type, row) {

                            let botones = `
                                <button type="button"
                                        class="btn btn-sm btn-outline-primary rounded-2 btnEditar"
                                        data-id="${row.idUsuario}"
                                        data-foto="${row.foto ?? ''}"
                                        data-nombre="${row.nombres ?? ''}"
                                        data-primerapellido="${row.primerApellido ?? ''}"
                                        data-segundoapellido="${row.segundoApellido ?? ''}"
                                        data-correo="${row.correoElectronico ?? ''}"
                                        data-telefono="${row.telefono ?? ''}"
                                        data-direccion="${row.direccion ?? ''}"
                                        data-nacimiento="${row.nacimiento ?? ''}"
                                        data-estado="${row.estado}">

                                    <i class="bi bi-pencil-square me-1"></i>
                                    Editar

                                </button>
                            `;

                            if (row.estado === true) {

                                botones += `
                                    <button type="button"
                                            class="btn btn-sm btn-outline-danger rounded-2 desactivar"
                                            data-id="${row.idUsuario}">

                                        <i class="bi bi-person-dash me-1"></i>
                                        Desactivar

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

                order: [[0, 'asc']],

                pageLength: 10,

                responsive: true,

                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json',
                    emptyTable: 'No hay usuarios registrados.'
                }
            });
        },

        registrarEventos() {

            $(document).on('click', '.btnEditar', function () {

                $('#IdUsuario').val($(this).data('id'));
                $('#Foto').val($(this).data('foto'));
                $('#Nombres').val($(this).data('nombre'));
                $('#PrimerApellido').val($(this).data('primerapellido'));
                $('#SegundoApellido').val($(this).data('segundoapellido'));
                $('#CorreoElectronico').val($(this).data('correo'));
                $('#Telefono').val($(this).data('telefono'));
                $('#Direccion').val($(this).data('direccion'));

                const nacimiento = $(this).data('nacimiento');

                if (nacimiento) {
                    $('#Nacimiento').val(
                        nacimiento.toString().substring(0, 10)
                    );
                } else {
                    $('#Nacimiento').val('');
                }

                $('#Estado').val(
                    $(this).data('estado').toString()
                );

                new bootstrap.Modal(
                    document.getElementById('editarUsuarioModal')
                ).show();
            });

            $('#formEditarUsuario').on('submit', function (e) {

                e.preventDefault();

                $.ajax({

                    url: $(this).attr('action'),

                    type: 'POST',

                    data: $(this).serialize(),

                    success: function (respuesta) {

                        if (!respuesta.esCorrecto) {

                            Swal.fire({
                                title: 'Error',
                                text: respuesta.mensaje,
                                icon: 'error'
                            });

                            return;
                        }

                        bootstrap.Modal.getInstance(
                            document.getElementById('editarUsuarioModal')
                        )?.hide();

                        Swal.fire({
                            title: 'Usuario actualizado',
                            text: respuesta.mensaje,
                            icon: 'success',
                            confirmButtonText: 'Aceptar'
                        });

                        Usuario.tabla.ajax.reload(null, false);
                    },

                    error: function () {

                        Swal.fire({
                            title: 'Error',
                            text: 'No fue posible actualizar el usuario.',
                            icon: 'error'
                        });
                    }
                });
            });

            $(document).on('click', '.desactivar', function () {

                const id = $(this).data('id');

                Swal.fire({

                    title: '¿Desactivar usuario?',

                    text: 'El usuario no podrá acceder al sistema.',

                    icon: 'warning',

                    showCancelButton: true,

                    confirmButtonText: 'Sí, desactivar',

                    cancelButtonText: 'Cancelar',

                    confirmButtonColor: '#dc3545'

                }).then(resultado => {

                    if (!resultado.isConfirmed) {
                        return;
                    }

                    $.ajax({

                        url: `/Usuario/DesactivarUsuario/${id}`,

                        type: 'POST',

                        success: function (respuesta) {

                            if (!respuesta.esCorrecto) {

                                Swal.fire({
                                    title: 'Error',
                                    text: respuesta.mensaje,
                                    icon: 'error'
                                });

                                return;
                            }

                            Swal.fire({
                                title: 'Usuario desactivado',
                                text: respuesta.mensaje,
                                icon: 'success',
                                confirmButtonText: 'Aceptar'
                            });

                            Usuario.tabla.ajax.reload(null, false);
                        },

                        error: function () {

                            Swal.fire({
                                title: 'Error',
                                text: 'No fue posible desactivar el usuario.',
                                icon: 'error'
                            });
                        }
                    });
                });
            });
        }
    };

    $(function () {
        Usuario.init();
    });

})();