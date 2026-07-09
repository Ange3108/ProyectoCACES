(() => {

    const GestionCitas = {

        init() {
            this.cargarCitas();
            this.registrarEventos();
        },

        cargarCitas() {
            $.ajax({
                url: '/Cita/ObtenerListadoCitas',
                type: 'GET',
                success: function (respuesta) {

                    if (!respuesta.esCorrecto) {
                        Swal.fire({ title: 'Error', text: respuesta.mensaje, icon: 'error' });
                        return;
                    }

                    let html = '';
                    respuesta.dato.forEach(c => {
                        html += `
    <tr>
        <td class="fw-semibold">${c.idCita}</td>
        <td>${c.idPaciente}</td>
        <td>${c.idMedico}</td>
        <td>${c.idEspecialidad}</td>
        <td>${new Date(c.fecha).toLocaleDateString()}</td>
        <td>${c.hora}</td>
        <td>${c.motivo}</td>
        <td>${c.estado === 1
                                ? '<span class="badge rounded-pill" style="background:#DDF7F8;color:#0B6F73;">Activa</span>'
                                : '<span class="badge rounded-pill bg-secondary">Cancelada</span>'}</td>
        <td>
            <button class="btn btn-sm text-white rounded-3 btnEditarFecha"
                    style="background:#20C4D8;border:none;"
                    data-id="${c.idCita}" data-fecha="${c.fecha}">
                Editar fecha
            </button>
        </td>
    </tr>
`;
                    });

                    $('#tbGestionCitas tbody').html(html);
                },

                error: function () {
                    Swal.fire({ title: 'Error', text: 'No fue posible cargar las citas.', icon: 'error' });
                }
            });
        },

        registrarEventos() {

            $(document).on('click', '.btnEditarFecha', function () {
                const idCita = $(this).data('id');

                Swal.fire({
                    title: 'Nueva fecha',
                    input: 'date',
                    inputValue: $(this).data('fecha'),
                    showCancelButton: true,
                    confirmButtonText: 'Actualizar'
                }).then((result) => {
                    if (!result.isConfirmed || !result.value) return;

                    $.ajax({
                        url: '/Cita/ActualizarFechaCita',
                        type: 'POST',
                        data: { idCita: idCita, nuevaFecha: result.value },

                        success: function (respuesta) {
                            Swal.fire({
                                title: respuesta.esCorrecto ? 'Correcto' : 'Error',
                                text: respuesta.mensaje,
                                icon: respuesta.esCorrecto ? 'success' : 'error'
                            });
                            GestionCitas.cargarCitas();
                        },

                        error: function () {
                            Swal.fire({ title: 'Error', text: 'No fue posible actualizar la fecha.', icon: 'error' });
                        }
                    });
                });
            });

            $('#formCancelarPorMedico').on('submit', function (e) {
                e.preventDefault();

                $.ajax({
                    url: '/Cita/CancelarCitasPorMedicoYFecha',
                    type: 'POST',
                    data: $(this).serialize(),

                    success: function (respuesta) {
                        Swal.fire({
                            title: respuesta.esCorrecto ? 'Correcto' : 'Error',
                            text: respuesta.mensaje,
                            icon: respuesta.esCorrecto ? 'success' : 'error'
                        });
                        GestionCitas.cargarCitas();
                    },

                    error: function () {
                        Swal.fire({ title: 'Error', text: 'Ocurrió un error al cancelar las citas.', icon: 'error' });
                    }
                });
            });
        }
    };

    window.GestionCitas = GestionCitas;

    $(function () {
        GestionCitas.init();
    });

})();