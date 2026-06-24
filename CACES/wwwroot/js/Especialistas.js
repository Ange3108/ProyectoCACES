(() => {

    const IMG_FALLBACK_HOMBRE = '/img/hombreD.png';
    const IMG_FALLBACK_MUJER = '/img/mujerD.jpg';

    function getFallback(nombre) {
        const prefijosF = ['dra', 'dra.', 'doctora'];
        const lower = nombre.toLowerCase();
        return prefijosF.some(p => lower.startsWith(p)) ? IMG_FALLBACK_MUJER : IMG_FALLBACK_HOMBRE;
    }

    /**
     * Construye la URL de la foto de forma segura:
     * - Si ya es una ruta absoluta o relativa válida, la usa.
     * - Si es nula/vacía/undefined, usa el fallback.
     */
    function resolverFoto(foto, nombre) {
        if (!foto || typeof foto !== 'string' || foto.trim() === '') {
            return getFallback(nombre);
        }
        // Evitar rutas que apunten solo a "/imagenes/default.jpg" si no existe
        return foto.trim();
    }

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
                        $('#contenedorEspecialistas').html(`
                            <div class="alert alert-info text-center rounded-3 py-4">
                                <i class="bi bi-person-x fs-2 d-block mb-2 text-muted"></i>
                                No hay especialistas disponibles actualmente.
                            </div>`
                        );
                        return;
                    }

                    let html = '';

                    respuesta.dato.forEach(medico => {
                        const nombre = [
                            medico.usuario?.nombres ?? '',
                            medico.usuario?.primerApellido ?? '',
                            medico.usuario?.segundoApellido ?? ''
                        ].join(' ').replace(/\s+/g, ' ').trim();

                        const foto = resolverFoto(medico.usuario?.foto, nombre);
                        const fallback = getFallback(nombre);

                        const especialidad = medico.nombreEspecialidad ?? 'Especialista';
                        const experiencia = medico.experiencia ?? 0;
                        const certificaciones = medico.certificaciones ?? '';

                        html += `
                        <div class="col-md-4">
                            <div class="card h-100 shadow-sm border-0 rounded-4 overflow-hidden">
                                <div class="text-center pt-4">
                                    <img src="${foto}"
                                         alt="Foto de ${nombre}"
                                         class="rounded-circle border border-2 shadow-sm"
                                         style="width:130px;height:130px;object-fit:cover;border-color:#72CEF2 !important;"
                                         onerror="this.onerror=null;this.src='${fallback}';" />
                                </div>
                                <div class="card-body text-center p-4">
                                    <h5 class="fw-bold mb-1">Dr(a). ${nombre}</h5>
                                    <p class="fw-semibold mb-2" style="color:#049DD9;">
                                        <i class="bi bi-hospital me-1"></i>${especialidad}
                                    </p>
                                    <p class="text-muted mb-2">
                                        <i class="bi bi-award me-1"></i>${experiencia} año${experiencia !== 1 ? 's' : ''} de experiencia
                                    </p>
                                    ${certificaciones ? `<p class="small text-muted">${certificaciones}</p>` : ''}
                                    <span class="badge bg-success rounded-pill px-3 py-2">
                                        <i class="bi bi-check-circle me-1"></i>Disponible
                                    </span>
                                </div>
                            </div>
                        </div>`;
                    });

                    $('#contenedorEspecialistas').html(`<div class="row g-4">${html}</div>`);
                },

                error: function (xhr) {
                    // Evita crash mostrando mensaje en lugar de Swal (que podría no estar cargado)
                    const mensajeError = (typeof Swal !== 'undefined')
                        ? () => Swal.fire({ title: 'Error', text: 'No fue posible cargar los especialistas.', icon: 'error' })
                        : () => {
                            $('#contenedorEspecialistas').html(`
                                <div class="alert alert-danger text-center rounded-3 py-4">
                                    <i class="bi bi-exclamation-circle fs-2 d-block mb-2"></i>
                                    No fue posible cargar los especialistas. Intente más tarde.
                                </div>`);
                        };
                    mensajeError();
                }
            });
        }
    };

    $(function () {
        Especialistas.init();
    });

})();