(() => {

    const CitasMedico = {

        init() {
            this.cargarCitas();
            this.registrarEventos();
        },

        cargarCitas() {
            $.ajax({
                url: '/Cita/ObtenerCitasMedico',
                type: 'GET',
                success: function (respuesta) {

                    if (!respuesta.esCorrecto) {
                        Swal.fire({ title: 'Error', text: respuesta.mensaje, icon: 'error' });
                        return;
                    }

                    let html = '';
                    respuesta.dato.forEach(c => {
                        const estadoBadge = c.estadoTexto === 'Pendiente'
                            ? '<span class="badge rounded-pill px-3 py-2" style="background:#DDF7F8;color:#0B6F73;">Activa</span>'
                            : '<span class="badge rounded-pill bg-secondary px-3 py-2">Cancelada</span>';

                        const accion = c.estadoTexto === 'Pendiente'
                            ? `<button class="btn btn-sm text-white rounded-3 cancelarCita" style="background:#B42318;border:none;" data-id="${c.idCita}"><i class="bi bi-x-circle-fill me-1"></i>Cancelar</button>`
                            : '';

                        html += `
                            <tr>
                                <td class="fw-semibold">${c.idCita}</td>
                                <td>${c.hora}</td>
                                <td>${c.motivo}</td>
                                <td>${estadoBadge}</td>
                                <td>
                                    <button class="btn btn-sm btn-outline-primary btnVerDetalle"
                                            type="button"
                                            data-bs-toggle="collapse"
                                            data-bs-target="#detalle-${c.idCita}">
                                        Ver detalles
                                    </button>
                                </td>
                                <td>${accion}</td>
                            </tr>
                            <tr class="collapse" id="detalle-${c.idCita}">
                                <td colspan="6" class="bg-light">
                                    <div class="p-3">
                                        <strong>Paciente:</strong> ${c.nombrePaciente}<br>
                                        <strong>Especialidad:</strong> ${c.nombreEspecialidad}
                                    </div>
                                </td>
                            </tr>
                        `;
                    });

                    $('#tbCitasMedico tbody').html(html);
                    $('#sinCitasMedico').toggle(respuesta.dato.length === 0);
                },

                error: function () {
                    Swal.fire({ title: 'Error', text: 'No fue posible cargar las citas.', icon: 'error' });
                }
            });
        },

        registrarEventos() {
            $(document).on('click', '.cancelarCita', function () {
                const idCita = $(this).data('id');

                Swal.fire({
                    title: '¿Estás seguro?',
                    text: 'Esta acción cancelará la cita.',
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Sí, cancelar',
                    cancelButtonText: 'Volver'
                }).then((result) => {
                    if (!result.isConfirmed) return;

                    $.ajax({
                        url: '/Cita/CancelarCita',
                        type: 'POST',
                        data: { idCita: idCita },

                        success: function (respuesta) {
                            Swal.fire({
                                title: respuesta.esCorrecto ? 'Correcto' : 'Error',
                                text: respuesta.mensaje,
                                icon: respuesta.esCorrecto ? 'success' : 'error'
                            });
                            CitasMedico.cargarCitas();
                        },

                        error: function () {
                            Swal.fire({ title: 'Error', text: 'No fue posible cancelar la cita.', icon: 'error' });
                        }
                    });
                });
            });
        }
    };

    window.CitasMedico = CitasMedico;

    $(function () {
        CitasMedico.init();
    });

})();