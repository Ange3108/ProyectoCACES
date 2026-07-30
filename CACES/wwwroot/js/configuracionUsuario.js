(() => {

    const configuracionUsuario = {

        idUsuarioActual: null, 

        init() {
            this.eventos();
            this.cargarNotificacionesPendientes();
        },

        eventos() {

          
                // ---- Toggle mostrar/ocultar contraseña ----
                $(document).on('click', '.toggle-password', function () {
                    const targetId = $(this).data('target');
                    const $input = $('#' + targetId);
                    const $icon = $(this).find('i');

                    if ($input.attr('type') === 'password') {
                        $input.attr('type', 'text');
                        $icon.removeClass('bi-eye').addClass('bi-eye-slash');
                    } else {
                        $input.attr('type', 'password');
                        $icon.removeClass('bi-eye-slash').addClass('bi-eye');
                    }
                });


            // ---- Notificaciones (bell / lista de pendientes) ----
            $(document).on('click', '.btn-marcar-leida', function () {
                const id = $(this).data('id');
                configuracionUsuario.marcarLeida(id);
            });

            $('#btnMarcarTodasLeidas').on('click', function () {
                configuracionUsuario.marcarTodasLeidas();
            });


            // Usa el endpoint que ya existe en UsuarioController: CambiarContrasena(id, CambiarContrasenaDTO)
            $('#formCambiarPassword').on('submit', function (e) {
                e.preventDefault();

                const actual = $('#passwordActual').val();
                const nueva = $('#passwordNueva').val();
                const confirmar = $('#passwordConfirmar').val();

                if (nueva !== confirmar) {
                    Swal.fire({ title: 'Atención', text: 'Las contraseñas nuevas no coinciden.', icon: 'warning' });
                    return;
                }

                if (!configuracionUsuario.idUsuarioActual) {
                    Swal.fire({ title: 'Error', text: 'No se pudo determinar el usuario actual.', icon: 'error' });
                    return;
                }

                $.ajax({
                    url: '/Usuario/CambiarContrasena',
                    type: 'POST',
                    data: {
                        id: configuracionUsuario.idUsuarioActual,
                        
                        PasswordActual: actual,
                        PasswordNueva: nueva
                    },
                    success: function (resp) {
                        if (resp.esCorrecto) {
                            Swal.fire({ title: 'Éxito', text: 'Contraseña actualizada correctamente', icon: 'success', timer: 1800, showConfirmButton: false });
                            $('#formCambiarPassword')[0].reset();
                        } else {
                            Swal.fire({ title: 'Error', text: resp.mensaje || 'No se pudo actualizar la contraseña', icon: 'error' });
                        }
                    },
                    error: function () {
                        Swal.fire({ title: 'Error', text: 'Error de conexión', icon: 'error' });
                    }
                });
            });

         
            $('#formSoporte').on('submit', function () {
              
                return true;
            });



            $('#confirmarEliminarInput').on('input', function () {
                $('#btnConfirmarEliminarCuenta').prop('disabled', $(this).val().trim() !== 'ELIMINAR');
            });

            $('#btnConfirmarEliminarCuenta').on('click', function () {
                if (!configuracionUsuario.idUsuarioActual) {
                    Swal.fire({ title: 'Error', text: 'No se pudo determinar el usuario actual.', icon: 'error' });
                    return;
                }

                $.ajax({
                    url: '/Usuario/DesactivarUsuario',
                    type: 'POST',
                    data: { id: configuracionUsuario.idUsuarioActual },
                    success: function (resp) {
                        if (resp.esCorrecto) {
                            window.location.href = '/Account/Login';
                        } else {
                            Swal.fire({ title: 'Error', text: resp.mensaje || 'No se pudo desactivar la cuenta', icon: 'error' });
                        }
                    },
                    error: function () {
                        Swal.fire({ title: 'Error', text: 'Error de conexión al desactivar la cuenta', icon: 'error' });
                    }
                });
            });
        },


        cargarNotificacionesPendientes() {
            $.ajax({
                url: '/NotificacionUsuario/ObtenerPendientes',
                type: 'GET',
                success: function (resp) {
                    if (!resp.esCorrecto) return;

                    $('#badgeNotificaciones').text(resp.noLeidas).toggle(resp.noLeidas > 0);
                    configuracionUsuario.renderizarNotificaciones(resp.dato ?? []);
                },
                error: function () {
                    console.error('No fue posible cargar las notificaciones pendientes.');
                }
            });
        },

        renderizarNotificaciones(lista) {
            if (!lista.length) {
                $('#listaNotificaciones').html('<div class="text-muted small p-2">Sin notificaciones pendientes.</div>');
                return;
            }

            let items = '';
            lista.forEach(n => {
                items += `
                    <div class="d-flex justify-content-between align-items-start border-bottom py-2">
                        <div>${n.mensaje ?? n.titulo ?? ''}</div>
                        <button class="btn btn-sm btn-link btn-marcar-leida" data-id="${n.idNotificacionUsuario ?? n.id}">
                            Marcar leída
                        </button>
                    </div>`;
            });

            $('#listaNotificaciones').html(items);
        },

        marcarLeida(id) {
            $.ajax({
                url: '/NotificacionUsuario/MarcarLeida',
                type: 'POST',
                data: { id: id },
                success: function (resp) {
                    if (resp.esCorrecto) {
                        configuracionUsuario.cargarNotificacionesPendientes();
                    } else {
                        Swal.fire({ title: 'Error', text: resp.mensaje || 'No se pudo marcar como leída', icon: 'error' });
                    }
                },
                error: function () {
                    Swal.fire({ title: 'Error', text: 'Error de conexión', icon: 'error' });
                }
            });
        },

        marcarTodasLeidas() {
            $.ajax({
                url: '/NotificacionUsuario/MarcarTodasLeidas',
                type: 'POST',
                success: function (resp) {
                    if (resp.esCorrecto) {
                        configuracionUsuario.cargarNotificacionesPendientes();
                    } else {
                        Swal.fire({ title: 'Error', text: resp.mensaje || 'No se pudo actualizar', icon: 'error' });
                    }
                },
                error: function () {
                    Swal.fire({ title: 'Error', text: 'Error de conexión', icon: 'error' });
                }
            });
        },

        // ---- Configuraciones globales (admin, ConfiguracionController) ----
        cargarConfiguraciones() {
            $.ajax({
                url: '/Configuracion/ObtenerTodos',
                type: 'GET',
                success: function (resp) {
                    if (!resp.esCorrecto) {
                        Swal.fire({ title: 'Error', text: resp.mensaje, icon: 'error' });
                        return;
                    }
                    console.log('Configuraciones:', resp.dato);
                },
                error: function () {
                    Swal.fire({ title: 'Error', text: 'No fue posible cargar las configuraciones.', icon: 'error' });
                }
            });
        }
    };

    window.configuracionUsuario = configuracionUsuario;

    $(function () {
        configuracionUsuario.init();
    });

})();