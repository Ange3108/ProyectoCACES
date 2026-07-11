function formatearIcono(opcion) {

    if (!opcion.id) {
        return opcion.text; // opción "Seleccione un icono"
    }

    const codigo = $(opcion.element).data('codigo');

    return $(`
        <span>
            <i class="${codigo}" style="margin-right: 8px;"></i>
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

            this.tabla = $('#tbEspecialidades').DataTable({

                ajax: {
                    url: '/Especialidad/ObtenerListadoEspecialidades',
                    type: 'GET',
                    dataSrc: 'dato'
                },

                columns: [
                    { data: 'nombre' },
                    { data: 'descripcion' },
                    {
                        data: 'icono',
                        orderable: false,
                        render: function (data) {
                            return `<i class="${data} fs-4"></i>`;
                        }
                    },
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
                                    data-idicono="${row.idIcono}"
                                    data-descripcion="${row.descripcion}"
                                    data-estado="${row.estado}">
                                    <i class="bi bi-pencil"></i>
                                </button>`;

                            if (row.estado === true) {
                                botones += `
                                    <button
                                        type="button"
                                        class="btn btn-sm btn-danger desactivar"
                                        data-id="${row.idEspecialidad}">
                                        <i class="bi bi-trash"></i>
                                    </button>`;
                            }

                            return botones;
                        }
                    }
                ],

                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
                }
            });
        },

        cargarIconos(selector, callback) {

            $.ajax({
                url: '/Icono/ObtenerIconos',
                type: 'GET',
                success: function (respuesta) {

                    let select = $(selector);

                    // Destruir instancia previa de Select2 si existe, para evitar duplicados
                    if (select.hasClass('select2-hidden-accessible')) {
                        select.select2('destroy');
                    }

                    select.empty();
                    select.append('<option value="">Seleccione un icono</option>');

                    (respuesta.dato || []).forEach(c => {
                        select.append(`
                            <option value="${c.idIcono}" data-codigo="${c.codigo}">
                                ${c.nombre}
                            </option>
                        `);
                    });

                    select.select2({
                        width: '100%',
                        dropdownParent: select.closest('.modal'),
                        templateResult: formatearIcono,
                        templateSelection: formatearIcono
                    });

                    if (callback) callback();
                }
            });
        },

        registrarEventos() {

            // Abrir modal editar
            $(document).on('click', '.btnEditar', function () {

                const idIconoActual = $(this).data('idicono');

                $('#IdEspecialidad').val($(this).data('id'));
                $('#Nombre').val($(this).data('nombre'));
                $('#Descripcion').val($(this).data('descripcion'));
                $('#Estado').val($(this).data('estado'));

                Especialidad.cargarIconos('#Icono', function () {
                    $('#Icono').val(idIconoActual).trigger('change');
                });

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

                            success: function (respuesta) {

                                Swal.fire({
                                    title: 'Especialidad desactivada correctamente',
                                    text: respuesta.mensaje,
                                    icon: 'success'
                                });

                                $('#tbEspecialidades').DataTable().ajax.reload();
                            },

                            error: function () {

                                Swal.fire({
                                    title: 'Error',
                                    text: 'No fue posible desactivar la especialidad.',
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