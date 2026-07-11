(() => {

    const Icono = {

        init() {
            this.inicializarTabla();
            this.registrarEventos();
        },

        inicializarTabla() {

            $('#tbIconos').DataTable({

                ajax: {
                    url: '/Icono/ObtenerIconos',
                    type: 'GET',
                    dataSrc: 'dato'
                },

                columns: [
                    {
                        data: 'codigo',
                        orderable: false,
                        render: function (data) {
                            return `<i class="${data} fs-4"></i>`;
                        }
                    },
                    { data: 'nombre' },
                    { data: 'codigo' },
                    {
                        data: null,
                        orderable: false,
                        render: function (data, type, row) {
                            return `
                                <button type="button" class="btn btn-sm btn-warning btnEditar"
                                    data-id="${row.idIcono}"
                                    data-nombre="${row.nombre}"
                                    data-codigo="${row.codigo}">
                                    <i class="bi bi-pencil"></i>
                                </button>`;
                        }
                    }
                ],

                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
                }
            });
        },

        registrarEventos() {

            // Preview en vivo al escribir la clase
            $(document).on('input', '#CodigoRegistro', function () {
                $('#previewRegistro').attr('class', $(this).val() || 'bi bi-question-circle');
            });

            $(document).on('input', '#CodigoEditar', function () {
                $('#previewEditar').attr('class', $(this).val() || 'bi bi-question-circle');
            });

            // Abrir modal editar
            $(document).on('click', '.btnEditar', function () {

                $('#IdIcono').val($(this).data('id'));
                $('#NombreEditar').val($(this).data('nombre'));
                $('#CodigoEditar').val($(this).data('codigo'));
                $('#previewEditar').attr('class', $(this).data('codigo'));

                new bootstrap.Modal(document.getElementById('editarIconoModal')).show();
            });

            // Registrar
            $('#formRegistroIcono').on('submit', function (e) {
                e.preventDefault();

                $.ajax({
                    url: $(this).attr('action'),
                    type: 'POST',
                    data: $(this).serialize(),

                    success: function (respuesta) {
                        if (respuesta.esCorrecto) {
                            Swal.fire({ title: 'Correcto', text: respuesta.mensaje, icon: 'success' });
                            bootstrap.Modal.getInstance(document.getElementById('registroIconoModal')).hide();
                            $('#formRegistroIcono')[0].reset();
                            $('#previewRegistro').attr('class', 'bi bi-question-circle');
                            $('#tbIconos').DataTable().ajax.reload();
                        } else {
                            Swal.fire({ title: 'Error', text: respuesta.mensaje, icon: 'error' });
                        }
                    },

                    error: function () {
                        Swal.fire({ title: 'Error', text: 'No fue posible registrar el ícono.', icon: 'error' });
                    }
                });
            });

            // Editar
            $('#formEditarIcono').on('submit', function (e) {
                e.preventDefault();

                $.ajax({
                    url: $(this).attr('action'),
                    type: 'POST',
                    data: $(this).serialize(),

                    success: function (respuesta) {
                        if (respuesta.esCorrecto) {
                            Swal.fire({ title: 'Correcto', text: respuesta.mensaje, icon: 'success' });
                            bootstrap.Modal.getInstance(document.getElementById('editarIconoModal')).hide();
                            $('#tbIconos').DataTable().ajax.reload();
                        } else {
                            Swal.fire({ title: 'Error', text: respuesta.mensaje, icon: 'error' });
                        }
                    },

                    error: function () {
                        Swal.fire({ title: 'Error', text: 'No fue posible actualizar el ícono.', icon: 'error' });
                    }
                });
            });
        }
    };

    $(function () {
        Icono.init();
    });

})();