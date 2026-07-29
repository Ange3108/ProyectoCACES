(() => {

    const INTERVALO_MS = 30000; // cada 30s; ajusta según la carga que quieras generar en el servidor

    const Campanita = {

        init() {
            this.cargar();
            setInterval(() => Campanita.cargar(), INTERVALO_MS);
            this.eventos();
        },

        eventos() {
            $(document).on('click', '.item-campanita', function () {
                const id = $(this).data('id');
                Campanita.marcarLeida(id, $(this));
            });

            $('#btnMarcarTodasLeidas').on('click', function (e) {
                e.stopPropagation();
                Campanita.marcarTodasLeidas();
            });
        },

        cargar() {
            $.ajax({
                url: '/NotificacionUsuario/ObtenerPendientes',
                type: 'GET',
                success: function (respuesta) {
                    if (!respuesta.esCorrecto) return;
                    Campanita.actualizarBadge(respuesta.noLeidas ?? 0);
                    Campanita.renderizarLista(respuesta.dato ?? []);
                },
                error: function () {
                    // Falla silenciosa: no interrumpimos al usuario por un polling fallido.
                }
            });
        },

        actualizarBadge(cantidad) {
            const badge = $('#badgeCampanita');
            if (cantidad > 0) {
                badge.text(cantidad > 99 ? '99+' : cantidad).removeClass('d-none');
            } else {
                badge.addClass('d-none');
            }
        },

        renderizarLista(notificaciones) {
            if (notificaciones.length === 0) {
                $('#listaCampanita').html('<div class="text-center text-muted py-4">No tienes notificaciones.</div>');
                return;
            }

            let html = '';
            notificaciones.forEach(n => {
                const claseNoLeida = n.leido ? '' : 'bg-light';
                html += `
                    <button type="button" class="item-campanita dropdown-item border-bottom py-2 ${claseNoLeida}"
                            data-id="${n.idNotificacionUsuario}">
                        <div class="fw-semibold small">${n.titulo}</div>
                        <div class="text-muted small text-wrap">${n.mensaje}</div>
                    </button>`;
            });

            $('#listaCampanita').html(html);
        },

        marcarLeida(id, elemento) {
            elemento.removeClass('bg-light');
            $.ajax({
                url: `/NotificacionUsuario/MarcarLeida?id=${id}`,
                type: 'POST',
                success: function () {
                    Campanita.cargar();
                }
            });
        },

        marcarTodasLeidas() {
            $.ajax({
                url: '/NotificacionUsuario/MarcarTodasLeidas',
                type: 'POST',
                success: function () {
                    Campanita.cargar();
                }
            });
        }
    };

    $(function () {
        Campanita.init();
    });

})();
