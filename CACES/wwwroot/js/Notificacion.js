(() => {

    const NotificacionAdmin = {

        modoEdicion: false,

        init() {
            this.cargar();
            this.eventos();
        },

        eventos() {
            $('#btnNuevaNotificacion').on('click', function () {
                NotificacionAdmin.modoEdicion = false;
                $('#tituloModalNotificacion').html('<i class="bi bi-plus-lg me-1"></i>Nuevo evento');
                $('#idNotificacion').val('');
                $('#notifEvento').val('').prop('disabled', false);
                $('#notifCanalPlataforma').prop('checked', true);
                $('#notifCanalEmail').prop('checked', false);
                $('#notifEstado').prop('checked', true);
                $('#modalNotificacion').modal('show');
            });

            $(document).on('click', '.btn-editar-notificacion', function () {
                const id = $(this).data('id');
                NotificacionAdmin.abrirModalEditar(id);
            });

            $(document).on('click', '.btn-cambiar-estado-notificacion', function () {
                const id = $(this).data('id');
                NotificacionAdmin.cambiarEstado(id);
            });

            $('#btnGuardarNotificacion').on('click', function () {
                NotificacionAdmin.guardar();
            });
        },

        cargar() {
            $.ajax({
                url: '/Notificacion/ObtenerTodos',
                type: 'GET',
                success: function (respuesta) {
                    if (!respuesta.esCorrecto) {
                        Swal.fire({ title: 'Error', text: respuesta.mensaje, icon: 'error' });
                        return;
                    }
                    NotificacionAdmin.renderizar(respuesta.dato ?? []);
                },
                error: function () {
                    Swal.fire({ title: 'Error', text: 'No fue posible cargar las notificaciones.', icon: 'error' });
                }
            });
        },

        renderizar(lista) {
            if (lista.length === 0) {
                $('#contenedorNotificaciones').html(`
                    <div class="alert alert-info text-center rounded-3 py-4">
                        <i class="bi bi-inbox fs-2 d-block mb-2 text-muted"></i>
                        No hay eventos de notificación registrados.
                    </div>`
                );
                return;
            }

            let filas = '';
            lista.forEach(n => {
                const chip = activo => activo
                    ? '<i class="bi bi-check-circle-fill text-success"></i>'
                    : '<i class="bi bi-x-circle text-muted"></i>';
                const estado = n.estado
                    ? '<span class="badge bg-success">Activo</span>'
                    : '<span class="badge bg-secondary">Inactivo</span>';

                filas += `
                    <tr>
                        <td><code>${n.evento}</code></td>
                        <td class="text-center">${chip(n.canalPlataforma)}</td>
                        <td class="text-center">${chip(n.canalEmail)}</td>
                        <td>${estado}</td>
                        <td>
                            <button class="btn btn-sm btn-outline-primary btn-editar-notificacion me-1"
                                data-id="${n.id_Notificacion}"
                                data-evento="${n.evento}"
                                data-plataforma="${n.canalPlataforma}"
                                data-email="${n.canalEmail}"
                                data-estado="${n.estado}">
                                <i class="bi bi-pencil"></i> Editar
                            </button>
                            <button class="btn btn-sm btn-outline-secondary btn-cambiar-estado-notificacion"
                                data-id="${n.id_Notificacion}">
                                <i class="bi bi-toggle2-on"></i> ${n.estado ? 'Desactivar' : 'Activar'}
                            </button>
                        </td>
                    </tr>`;
            });

            $('#contenedorNotificaciones').html(`
                <table class="table table-hover align-middle mt-2">
                    <thead class="table-light">
                        <tr>
                            <th>Evento</th>
                            <th class="text-center">Campanita</th>
                            <th class="text-center">Correo</th>
                            <th>Estado</th>
                            <th style="width:220px;">Acciones</th>
                        </tr>
                    </thead>
                    <tbody>${filas}</tbody>
                </table>`
            );
        },

        abrirModalEditar(id) {
            const btn = $(`.btn-editar-notificacion[data-id="${id}"]`);
            NotificacionAdmin.modoEdicion = true;
            $('#tituloModalNotificacion').html('<i class="bi bi-pencil me-1"></i>Editar evento');
            $('#idNotificacion').val(id);
            $('#notifEvento').val(btn.data('evento')).prop('disabled', true);
            $('#notifCanalPlataforma').prop('checked', btn.data('plataforma') === true || btn.data('plataforma') === 'true');
            $('#notifCanalEmail').prop('checked', btn.data('email') === true || btn.data('email') === 'true');
            $('#notifEstado').prop('checked', btn.data('estado') === true || btn.data('estado') === 'true');
            $('#modalNotificacion').modal('show');
        },

        guardar() {
            const evento = $('#notifEvento').val()?.trim();
            if (!evento) {
                Swal.fire({ title: 'Atención', text: 'Ingrese el nombre del evento.', icon: 'warning' });
                return;
            }

            const datos = {
                Evento: evento,
                CanalPlataforma: $('#notifCanalPlataforma').is(':checked'),
                CanalEmail: $('#notifCanalEmail').is(':checked'),
                Estado: $('#notifEstado').is(':checked')
            };

            const esEdicion = NotificacionAdmin.modoEdicion;
            if (esEdicion) {
                datos.Id_Notificacion = parseInt($('#idNotificacion').val());
            }

            $.ajax({
                url: esEdicion ? '/Notificacion/ActualizarNotificacion' : '/Notificacion/CrearNotificacion',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(datos),
                success: function (respuesta) {
                    if (respuesta.esCorrecto) {
                        Swal.fire({ title: 'Éxito', text: respuesta.mensaje, icon: 'success', timer: 1800, showConfirmButton: false });
                        $('#modalNotificacion').modal('hide');
                        NotificacionAdmin.cargar();
                    } else {
                        Swal.fire({ title: 'Error', text: respuesta.mensaje, icon: 'error' });
                    }
                },
                error: function (xhr) {
                    const msg = xhr.responseJSON?.mensaje ?? 'No fue posible guardar la notificación.';
                    Swal.fire({ title: 'Error', text: msg, icon: 'error' });
                }
            });
        },

        cambiarEstado(id) {
            $.ajax({
                url: `/Notificacion/CambiarEstado?id=${id}`,
                type: 'POST',
                success: function (respuesta) {
                    if (respuesta.esCorrecto) {
                        Swal.fire({ title: 'Éxito', text: respuesta.mensaje, icon: 'success', timer: 1500, showConfirmButton: false });
                        NotificacionAdmin.cargar();
                    } else {
                        Swal.fire({ title: 'Error', text: respuesta.mensaje, icon: 'error' });
                    }
                },
                error: function () {
                    Swal.fire({ title: 'Error', text: 'No fue posible cambiar el estado.', icon: 'error' });
                }
            });
        }
    };

    $(function () {
        NotificacionAdmin.init();
    });

})();
