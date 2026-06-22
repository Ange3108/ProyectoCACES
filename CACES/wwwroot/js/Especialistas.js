(() => {

    const Especialistas = {

        init() {
            this.cargarEspecialistas();
        },

        cargarEspecialistas() {

            $.ajax({
                url: '/Medico/ObtenerEspecialistas',
                type: 'GET',

                success: function (respuesta) {

                    if (!respuesta.esCorrecto || !respuesta.dato || respuesta.dato.length === 0) {
                        $('#contenedorEspecialistas').html(
                            '<div class="alert alert-info text-center rounded-3">No hay especialistas disponibles actualmente.</div>'
                        );
                        return;
                    }

                    let html = '';

                    respuesta.dato.forEach(medico => {

                        const foto = medico.usuario?.foto
                            ? medico.usuario.foto
                            : '/imagenes/default.jpg';

                        const nombre = `${medico.usuario?.nombres ?? ''} ${medico.usuario?.primerApellido ?? ''} ${medico.usuario?.segundoApellido ?? ''}`.trim();

                        html += `
                            <div class="col-md-4">
                                <div class="card h-100 shadow-sm border-0 rounded-4 overflow-hidden">
                                    <div class="text-center pt-4">
                                        <img src="${foto}"
                                             alt="Foto de ${nombre}"
                                             class="rounded-circle border shadow-sm"
                                             style="width:130px;height:130px;object-fit:cover;"
                                             onerror="this.src='/imagenes/default.jpg'" />
                                    </div>
                                    <div class="card-body text-center p-4">
                                        <h5 class="fw-bold mb-1">Dr(a). ${nombre}</h5>
                                        <p class="text-primary fw-semibold mb-2">
                                            <i class="bi bi-hospital me-1"></i>${medico.nombreEspecialidad ?? 'Especialista'}
                                        </p>
                                        <p class="text-muted mb-2">
                                            <i class="bi bi-award me-1"></i>${medico.experiencia} años de experiencia
                                        </p>
                                        <p class="small text-muted">${medico.certificaciones ?? ''}</p>
                                        <span class="badge bg-success rounded-pill px-3 py-2">Disponible</span>
                                    </div>
                                </div>
                            </div>
                        `;
                    });

                    $('#contenedorEspecialistas').html(`<div class="row g-4">${html}</div>`);
                },

                error: function () {
                    Swal.fire({
                        title: 'Error',
                        text: 'No fue posible cargar los especialistas.',
                        icon: 'error'
                    });
                }
            });
        }
    };

    $(function () {
        Especialistas.init();
    });

})();
