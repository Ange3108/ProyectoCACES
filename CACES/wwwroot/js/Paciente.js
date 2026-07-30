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

    const Pacientes = {

        tabla: null,
        esAdministrador: false,

        init(esAdministrador) {
            this.esAdministrador = esAdministrador === true;
            this.inicializarTabla();
            this.registrarEventos();
        },

        inicializarTabla() {

            this.tabla = $('#tbPacientes').DataTable({

                ajax: {
                    url: '/Paciente/ObtenerPacientes',
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
                            text: 'No fue posible cargar los pacientes.'
                        });
                    }
                },

                columns: [

                    // PACIENTE
                    {
                        data: null,

                        render: function (data, type, row) {

                            const foto = resolverFoto(row.usuario?.foto);

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
                                         style="width:45px;height:45px;object-fit:cover;"
                                         onerror="this.onerror=null;this.src='/img/default.jpg';" />

                                    <div>
                                        <div class="fw-semibold">
                                            ${nombre || 'Nombre no disponible'}
                                        </div>
                                        <small class="text-muted">
                                            Paciente #${row.idPaciente}
                                        </small>
                                    </div>

                                </div>
                            `;
                        }
                    },

                    // DUI
                    {
                        data: 'usuario.dui',
                        defaultContent: ''
                    },

                    // CORREO
                    {
                        data: 'usuario.correoElectronico',
                        render: function (data) {
                            return `
                                <span class="text-secondary">
                                    <i class="bi bi-envelope me-1"></i>
                                    ${data ?? ''}
                                </span>
                            `;
                        }
                    },

                    // TELÉFONO
                    {
                        data: 'usuario.telefono',
                        defaultContent: ''
                    },

                    // DIRECCIÓN
                    {
                        data: 'usuario.direccion',
                        defaultContent: ''
                    },

                    // ESTADO
                    {
                        data: 'usuario.estado',
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
                                          style="background:#FEE2E2;color:#991B1B;">
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
                                <a href="/HistorialMedico/Detalle?id=${row.idHistorial}"
                                   class="btn btn-sm btn-outline-primary rounded-2">
                                    <i class="bi bi-eye-fill me-1"></i>
                                    Historial
                                </a>

                                <a href="/Perfil/ActualizarPerfil?id=${row.idPaciente}"
                                   class="btn btn-sm btn-outline-info rounded-2">
                                    <i class="bi bi-pencil-square me-1"></i>
                                    Editar
                                </a>
                            `;

                            if (Pacientes.esAdministrador) {

                                if (row.usuario?.estado === true) {

                                    botones += `
                                        <button type="button"
                                                class="btn btn-sm btn-outline-danger rounded-2 btnDesactivar"
                                                data-id="${row.idPaciente}">
                                            <i class="bi bi-person-dash me-1"></i>
                                            Desactivar
                                        </button>
                                    `;
                                } else {

                                    botones += `
                                        <button type="button"
                                                class="btn btn-sm btn-outline-success rounded-2 btnActivar"
                                                data-id="${row.idPaciente}">
                                            <i class="bi bi-person-check me-1"></i>
                                            Activar
                                        </button>
                                    `;
                                }
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

                autoWidth: false,

                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json',
                    emptyTable: 'No hay pacientes registrados.'
                }
            });
        },

        registrarEventos() {

            $(document).on('click', '.btnDesactivar', function () {

                const idPaciente = $(this).data('id');

                Swal.fire({
                    icon: 'warning',
                    title: '¿Desactivar paciente?',
                    text: 'El paciente no podrá acceder al sistema.',
                    showCancelButton: true,
                    confirmButtonText: 'Sí, desactivar',
                    cancelButtonText: 'Cancelar',
                    confirmButtonColor: '#dc3545',
                    reverseButtons: true
                }).then(resultado => {

                    if (!resultado.isConfirmed) return;

                    $.ajax({
                        url: `/Paciente/DesactivarPaciente`,
                        type: 'POST',
                        data: { id: idPaciente },
                        headers: {
                            RequestVerificationToken:
                                $('input[name="__RequestVerificationToken"]').val()
                        },
                        success: function (respuesta) {

                            if (!respuesta?.esCorrecto) {
                                Swal.fire({
                                    icon: 'error',
                                    title: 'Error',
                                    text: respuesta?.mensaje ?? 'No se pudo desactivar el paciente.'
                                });
                                return;
                            }

                            Swal.fire({
                                icon: 'success',
                                title: 'Paciente desactivado',
                                text: respuesta.mensaje,
                                confirmButtonText: 'Aceptar'
                            });

                            Pacientes.tabla.ajax.reload(null, false);
                        },
                        error: function (xhr) {
                            console.error(xhr.responseText);
                            Swal.fire({
                                icon: 'error',
                                title: 'Error',
                                text: xhr.responseJSON?.mensaje ?? 'No fue posible desactivar el paciente.'
                            });
                        }
                    });
                });
            });

            $(document).on('click', '.btnActivar', function () {

                const idPaciente = $(this).data('id');

                $.ajax({
                    url: `/Paciente/ActivarPaciente`,
                    type: 'POST',
                    data: { id: idPaciente },
                    headers: {
                        RequestVerificationToken:
                            $('input[name="__RequestVerificationToken"]').val()
                    },
                    success: function (respuesta) {

                        if (!respuesta?.esCorrecto) {
                            Swal.fire({
                                icon: 'error',
                                title: 'Error',
                                text: respuesta?.mensaje ?? 'No se pudo activar el paciente.'
                            });
                            return;
                        }

                        Swal.fire({
                            icon: 'success',
                            title: 'Paciente activado',
                            text: respuesta.mensaje,
                            confirmButtonText: 'Aceptar'
                        });

                        Pacientes.tabla.ajax.reload(null, false);
                    },
                    error: function (xhr) {
                        console.error(xhr.responseText);
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: xhr.responseJSON?.mensaje ?? 'No fue posible activar el paciente.'
                        });
                    }
                });
            });
        }
    };

    window.Pacientes = Pacientes;

})();