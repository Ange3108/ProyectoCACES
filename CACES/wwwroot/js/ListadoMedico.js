(() => {

    function resolverFoto(foto) {

        if (!foto ||
            typeof foto !== 'string' ||
            foto.trim() === '') {

            return '/img/default.jpg';
        }

        foto = foto.trim();

        if (foto.startsWith('/') ||
            foto.startsWith('http')) {

            return foto;
        }

        if (foto.startsWith('img/') ||
            foto.startsWith('uploads/')) {

            return '/' + foto;
        }

        return '/img/' + foto;
    }

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

                    dataSrc: function (respuesta) {
                        return respuesta?.esCorrecto === false
                            ? []
                            : respuesta?.dato ?? [];
                    },

                    error: function (xhr) {

                        console.error(xhr.responseText);

                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: 'No fue posible cargar los médicos.'
                        });
                    }
                },

                columns: [

                    // MÉDICO
                    {
                        data: null,

                        render: function (data, type, row) {

                            const foto = resolverFoto(
                                row.usuario?.foto
                            );

                            const nombre = [
                                row.usuario?.nombres ?? '',
                                row.usuario?.primerApellido ?? '',
                                row.usuario?.segundoApellido ?? ''
                            ]
                                .join(' ')
                                .replace(/\s+/g, ' ')
                                .trim();

                            return `
                                <div class="d-flex align-items-center gap-3">

                                    <img src="${foto}"
                                         alt="${nombre}"
                                         class="rounded-circle border"
                                         style="
                                            width:42px;
                                            height:42px;
                                            object-fit:cover;
                                         "
                                         onerror="
                                            this.onerror=null;
                                            this.src='/img/default.jpg';
                                         " />

                                    <div>

                                        <div class="fw-semibold text-dark">
                                            ${nombre || 'Nombre no disponible'}
                                        </div>

                                        <small class="text-muted">
                                            Médico #${row.idMedico}
                                        </small>

                                    </div>

                                </div>
                            `;
                        }
                    },

                    // ESPECIALIDAD
                    {
                        data: 'nombreEspecialidad',

                        render: function (data) {

                            return `
                                <span class="badge rounded-pill px-3 py-1"
                                      style="
                                        background:#DDF7F8;
                                        color:#0B6F73;
                                      ">

                                    <i class="bi bi-hospital me-1"></i>
                                    ${data ?? 'Sin especialidad'}

                                </span>
                            `;
                        }
                    },

                    // EXPERIENCIA
                    {
                        data: 'experiencia',

                        render: function (data) {

                            const cantidad = data ?? 0;

                            return `
                                <span class="text-secondary">

                                    <i class="bi bi-award me-1"></i>

                                    ${cantidad}
                                    año${cantidad !== 1 ? 's' : ''}

                                </span>
                            `;
                        }
                    },

                    // CERTIFICACIONES
                    {
                        data: 'certificaciones',

                        render: function (data) {

                            if (!data ||
                                typeof data !== 'string' ||
                                data.trim() === '') {

                                return `
                                    <span class="text-muted">
                                        No registradas
                                    </span>
                                `;
                            }

                            return `
                                <span class="text-secondary">
                                    ${data}
                                </span>
                            `;
                        }
                    },

                    // ESTADO
                    {
                        data: 'usuario.estado',
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
                                        Activo

                                    </span>
                                `
                                : `
                                    <span class="badge rounded-pill px-3 py-1"
                                          style="
                                            background:#FEE2E2;
                                            color:#991B1B;
                                          ">

                                        <i class="bi bi-x-circle-fill me-1"></i>
                                        Inactivo

                                    </span>
                                `;
                        }
                    },

                    // ACCIONES
                    {
                        data: null,
                        orderable: false,
                        searchable: false,
                        className: 'text-center',

                        render: function (data, type, row) {

                            let botones = `
                                <a href="/Medico/EditarMedico?id=${row.idMedico}"
                                   class="
                                        btn
                                        btn-sm
                                        btn-outline-primary
                                        rounded-2
                                        px-2
                                        py-1
                                   ">

                                    <i class="bi bi-pencil-square me-1"></i>
                                    Editar

                                </a>
                            `;

                            if (row.usuario?.estado === true) {

                                botones += `
                                    <button type="button"
                                            class="
                                                btn
                                                btn-sm
                                                btn-outline-danger
                                                rounded-2
                                                px-2
                                                py-1
                                                btnDesactivar
                                            "
                                            data-id="${row.idMedico}">

                                        <i class="bi bi-person-dash me-1"></i>
                                        Desactivar

                                    </button>
                                `;
                            }

                            return `
                                <div class="
                                    d-flex
                                    justify-content-center
                                    align-items-center
                                    gap-2
                                    flex-wrap
                                ">

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
                    emptyTable: 'No hay médicos registrados.'
                }
            });
        },

        registrarEventos() {

            $(document).on('click', '.btnDesactivar', function () {

                const idMedico = $(this).data('id');

                if (!idMedico) {

                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'No se pudo identificar al médico.'
                    });

                    return;
                }

                Swal.fire({

                    icon: 'warning',

                    title: '¿Desactivar médico?',

                    text: 'El médico no podrá acceder al sistema ni recibir nuevas citas.',

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

                        url: `/Medico/DesactivarMedico/${idMedico}`,

                        type: 'POST',

                        headers: {
                            RequestVerificationToken:
                                $('input[name="__RequestVerificationToken"]').val()
                        },

                        success: function (respuesta) {

                            if (!respuesta?.esCorrecto) {

                                Swal.fire({
                                    icon: 'error',
                                    title: 'Error',
                                    text: respuesta?.mensaje ??
                                        'No se pudo desactivar el médico.'
                                });

                                return;
                            }

                            Swal.fire({
                                icon: 'success',
                                title: 'Médico desactivado',
                                text: respuesta.mensaje,
                                confirmButtonText: 'Aceptar'
                            });

                            Medicos.tabla.ajax.reload(null, false);
                        },

                        error: function (xhr) {

                            console.error(xhr.responseText);

                            const mensaje =
                                xhr.responseJSON?.mensaje ??
                                'No fue posible desactivar el médico.';

                            Swal.fire({
                                icon: 'error',
                                title: 'Error',
                                text: mensaje
                            });
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