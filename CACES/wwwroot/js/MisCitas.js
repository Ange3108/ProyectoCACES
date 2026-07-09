(() => {

    const MisCitas = {

        init() {
            this.cargarCitas();
        },

        cargarCitas() {
            $.ajax({
                url: '/Cita/ObtenerMisCitas',
                type: 'GET',
                success: function (respuesta) {

                    if (!respuesta.esCorrecto) {
                        Swal.fire({ title: 'Error', text: respuesta.mensaje, icon: 'error' });
                        return;
                    }

                    let html = '';
                    respuesta.dato.forEach(c => {
                        const estadoBadge = c.estado === 1
                            ? '<span class="badge bg-success">Pendiente</span>'
                            : '<span class="badge bg-danger">Cancelada</span>';

                        html += `
                            <tr>
                                <td>${new Date(c.fecha).toLocaleDateString()}</td>
                                <td>${c.hora}</td>
                                <td>${c.motivo}</td>
                                <td>${estadoBadge}</td>
                                <td>
                                    <a href="/Cita/Ticket?id=${c.idCita}" class="btn btn-sm btn-primary">
                                        Ver ticket
                                    </a>
                                </td>
                            </tr>
                        `;
                    });

                    $('#tbMisCitas tbody').html(html);
                    $('#sinCitas').toggle(respuesta.dato.length === 0);
                },

                error: function () {
                    Swal.fire({ title: 'Error', text: 'No fue posible cargar tus citas.', icon: 'error' });
                }
            });
        }
    };

    $(function () {
        MisCitas.init();
    });

})();