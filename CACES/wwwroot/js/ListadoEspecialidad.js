(() => {

    const Especialidad = {

        tabla: null,

        init() {
            this.inicializarTabla();
            this.registrarEventos();
        },

        inicializarTabla() {

            this.tabla = $('#tbEspecialidades').DataTable({

                ajax: {
                    url: '/Especialidad/ObtenerListadoEspecialidades',
                    type: 'GET',
                    dataSrc: 'dato'
                },

                columns: [

                    
                    { data: 'nombre' },
                    { data: 'descripcion' },
                    { data: 'icono' },
                    { data: 'fechaDeRegistro' },

                    
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

                         

                                let botones = `
        <button
            type="button"
            class="btn btn-sm btn-warning btnEditar"
            data-id="${row.idEspecialidad}"
            data-nombre="${row.nombre}"
            data-icono="${row.icono}"
            data-descripcion="${row.descripcion}"
            data-estado="${row.estado}">
            <i class="bi bi-pencil"></i>
        </button>
    `;

                                if (row.estado === true) {
                                    botones += `
            <button
                type="button"
                class="btn btn-sm btn-danger desactivar"
                data-id="${row.idEspecialidad}">
                <i class="bi bi-trash"></i>
            </button>
        `;
                                }

                                return botones;
                            }
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

                $('#IdEspecialidad').val($(this).data('id'));
                $('#Nombre').val($(this).data('nombre'));
                $('#Icono').val($(this).data('icono'));
                $('#Descripcion').val($(this).data('descripcion'));



                $('#Estado').val($(this).data('estado'));

                const modal = new bootstrap.Modal(
                    document.getElementById('editarEspecialidadModal')
                );

                modal.show();



            });

            $('#formEditarEspecialidad').on('submit', function (e) {

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
                                document.getElementById('editarEspecialidadModal')
                            ).hide();

                            $('#tbEspecialidades').DataTable().ajax.reload();
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

            // Desactivar especialidad
            $(document).on('click', '.desactivar', function () {

                const id = $(this).data('id');

                Swal.fire({
                    title: '¿Estás seguro?',
                    text: 'No podrás revertir esta acción',
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Sí, desactivar',
                    cancelButtonText: 'Cancelar'
                }).then((result) => {

                    if (result.isConfirmed) {

                        $.ajax({
                            url: `/Especialidad/DesactivarEspecialidad/${id}`,
                            type: 'POST',

                            success: function (respuestaErrores) {

                                Swal.fire({
                                    title: 'Especialidad eliminado correctamente',
                                    text: respuestaErrores.mensaje,
                                    icon: 'success'
                                });

                                $('#tbEspecialidades').DataTable().ajax.reload();
                            },

                            error: function () {

                                Swal.fire({
                                    title: 'Error',
                                    text: 'No fue posible desactivar el especialidad.',
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
        Especialidad.init();
    });

})();