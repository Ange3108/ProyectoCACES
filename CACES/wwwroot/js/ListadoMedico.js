(() => {

    const Medicos = {

        tabla: null,

        init() {
            this.inicializarTabla();
            this.registrarEventos();
        },

        inicializarTabla() {

            this.tabla = $('#tbMedicos').DataTable({

                ajax: {
                    url: '/Medico/GetMedicos',
                    type: 'GET',
                    dataSrc: 'dato'
                },

                columns: [
                    {
                        data: null,
                        render: function (data, type, row) {
                            const foto = row.usuario?.foto ?? '/imagenes/default.jpg';
                            const nombre = `${row.usuario?.nombres ?? ''} ${row.usuario?.primerApellido ?? ''} ${row.usuario?.segundoApellido ?? ''}`.trim();
                            return `
                                <div class="d-flex align-items-center gap-2">
                                    <img src="${foto}" alt="${nombre}"
                                         class="rounded-circle border"
                                         style="width:40px;height:40px;object-fit:cover;"
                                         onerror="this.src='/imagenes/default.jpg'" />
                                    <span>${nombre}</span>
                                </div>`;
                        }
                    },
                    { data: 'nombreEspecialidad' },
                    {
                        data: 'experiencia',
                        render: data => `${data} años`
                    },
                    { data: 'certificaciones' },
                    {
                        data: 'usuario.estado',
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
                                <a href="/Medico/EditarMedico?id=${row.idMedico}"
                                   class="btn btn-sm btn-warning"
                                   title="Editar médico">
                                    <i class="bi bi-pencil"></i>
                                </a>
                                <button type="button"
                                        class="btn btn-sm btn-danger btnDesactivar"
                                        data-id="${row.idMedico}"
                                        title="Desactivar médico">
                                    <i class="bi bi-person-dash"></i>
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

            $(document).on('click', '.btnDesactivar', function () {

                const id = $(this).data('id');

                Swal.fire({
                    title: '¿Desactivar médico?',
                    text: 'Se desactivará su cuenta y se eliminarán sus citas pendientes.',
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Sí, desactivar',
                    cancelButtonText: 'Cancelar',
                    confirmButtonColor: '#d33'
                }).then(result => {

                    if (!result.isConfirmed) return;

                    $.ajax({
                        url: `/Medico/DesactivarMedico/${id}`,
                        type: 'POST',
                        headers: {
                            'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                        },

                        success: function (respuesta) {
                            Swal.fire({
                                title: respuesta.esCorrecto ? 'Médico desactivado' : 'Error',
                                text: respuesta.mensaje,
                                icon: respuesta.esCorrecto ? 'success' : 'error'
                            });

                            if (respuesta.esCorrecto)
                                $('#tbMedicos').DataTable().ajax.reload();
                        },

                        error: function (xhr) {
                            const msg = xhr.responseJSON?.mensaje ?? 'No fue posible desactivar el médico.';
                            Swal.fire({ title: 'Error', text: msg, icon: 'error' });
                        }
                    });
                });
            });
        }
    };

    $(function () {
        Medicos.init();
    });

})();
