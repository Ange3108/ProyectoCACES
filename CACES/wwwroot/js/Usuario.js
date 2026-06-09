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
                            return `${data.nombres} ${data.primerApellido} ${data.segundoApellido}`;
                        }
                    },

                    { data: 'correoElectronico' },
                    { data: 'dui' },
                    { data: 'telefono' },
                    { data: 'direccion' },

                    {
                        data: 'nacimiento',
                        render: function (data) {

                            if (!data)
                                return '';

                            return data.substring(0, 10);
                        }
                    },

                    {
                        data: 'estado',
                        render: function (data) {

                            return data
                                ? '<span class="badge bg-success">Activo</span>'
                                : '<span class="badge bg-danger">Inactivo</span>';
                        }
                    },


                    {
                        data: null,
                       
                        orderable: false,
                        render: function (data, type, row) {

                            return `
                                <button
                                    type="button"
                                    class="btn btn-sm btn-warning btnEditar"
                                    data-id="${row.idUsuario}"
                                    data-nombre="${row.nombres}"
                                    data-primerapellido="${row.primerApellido}"
                                    data-segundoapellido="${row.segundoApellido}"
                                    data-correo="${row.correoElectronico}"
                                    data-telefono="${row.telefono}"
                                    data-direccion="${row.direccion}"
                                    data-nacimiento="${row.nacimiento}"
                                    data-estado="${row.estado}">
                                    <i class="bi bi-pencil"></i>
                                </button>

                                <button
                                    type="button"
                                    class="btn btn-sm btn-danger eliminar"
                                    data-id="${row.idUsuario}">
                                    <i class="bi bi-trash"></i>
                                </button>
                            `;
                        }
                    }
                ],

                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
                }
            });
        },

        registrarEventos() {

            // Abrir modal editar
            $(document).on('click', '.btnEditar', function () {

                $('#IdUsuario').val($(this).data('id'));
                $('#Nombres').val($(this).data('nombre'));
                $('#PrimerApellido').val($(this).data('primerapellido'));
                $('#SegundoApellido').val($(this).data('segundoapellido'));
                $('#CorreoElectronico').val($(this).data('correo'));
                $('#Telefono').val($(this).data('telefono'));
                $('#Direccion').val($(this).data('direccion'));

                const nacimiento = $(this).data('nacimiento');

                if (nacimiento) {
                    $('#Nacimiento').val(nacimiento.substring(0, 10));
                }

                $('#Estado').val($(this).data('estado'));

                const modal = new bootstrap.Modal(
                    document.getElementById('editarUsuarioModal')
                );

                modal.show();
     

               
            });

            $('#formEditarUsuario').on('submit', function (e) {

                e.preventDefault();

                $.ajax({
                    url: $(this).attr('action'),
                    type: 'POST',
                    data: $(this).serialize(),

                    success: function (respuesta) {

                        if (respuesta.esCorrecto) {

                            Swal.fire({
                                title: 'Correcto',
                                text: respuesta.mensaje,
                                icon: 'success'
                            });

                            bootstrap.Modal.getInstance(
                                document.getElementById('editarUsuarioModal')
                            ).hide();

                            $('#tbUsuarios').DataTable().ajax.reload();
                        }
                        else {

                            Swal.fire({
                                title: 'Error',
                                text: respuesta.mensaje,
                                icon: 'error'
                            });
                        }
                    },

                    error: function () {

                        Swal.fire({
                            title: 'Error',
                            text: 'Ocurrió un error al actualizar.',
                            icon: 'error'
                        });
                    }
                });
            });

            // Eliminar usuario
            $(document).on('click', '.eliminar', function () {

                const id = $(this).data('id');

                Swal.fire({
                    title: '¿Estás seguro?',
                    text: 'No podrás revertir esta acción',
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Sí, eliminar',
                    cancelButtonText: 'Cancelar'
                }).then((result) => {

                    if (result.isConfirmed) {

                        $.ajax({
                            url: `/Usuario/EliminarUsuario/${id}`,
                            type: 'POST',

                            success: function (respuestaErrores) {

                                Swal.fire({
                                    title: 'Usuario eliminado correctamente',
                                    text: respuestaErrores.mensaje,
                                    icon: 'success'
                                });

                                $('#tbUsuarios').DataTable().ajax.reload();
                            },

                            error: function () {

                                Swal.fire({
                                    title: 'Error',
                                    text: 'No fue posible eliminar el usuario.',
                                    icon: 'error'
                                });
                            }
                        });
                    }
                });
            });
        }
    };

    $(function () {
        Usuario.init();
    });

})();