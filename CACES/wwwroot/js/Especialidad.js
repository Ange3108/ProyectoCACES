(() => {

    function escaparHtml(valor) {

        if (valor === null || valor === undefined) {
            return '';
        }

        return String(valor)
            .replaceAll('&', '&amp;')
            .replaceAll('<', '&lt;')
            .replaceAll('>', '&gt;')
            .replaceAll('"', '&quot;')
            .replaceAll("'", '&#039;');
    }

    function resolverIcono(icono) {

        if (!icono ||
            typeof icono !== 'string' ||
            icono.trim() === '') {

            return 'bi bi-hospital';
        }

        return icono.trim();
    }

    const Especialidad = {

        init() {
            this.cargarEspecialidades();
        },

        cargarEspecialidades() {

            const contenedor = $('#contenedorEspecialidades');

            if (!contenedor.length) {
                return;
            }

            this.mostrarCargando();

            $.ajax({

                url: '/Especialidad/ObtenerEspecialidades',

                type: 'GET',

                success: function (respuesta) {

                    if (!respuesta ||
                        respuesta.esCorrecto === false ||
                        !Array.isArray(respuesta.dato) ||
                        respuesta.dato.length === 0) {

                        Especialidad.mostrarSinResultados(
                            respuesta?.mensaje ||
                            'No hay especialidades disponibles actualmente.'
                        );

                        return;
                    }

                    let html = '';

                    respuesta.dato.forEach((especialidad, index) => {

                        const numero = String(index + 1)
                            .padStart(2, '0');

                        const idEspecialidad =
                            especialidad.idEspecialidad ?? 0;

                        const nombre = escaparHtml(
                            especialidad.nombre ||
                            'Especialidad'
                        );

                        const descripcion = escaparHtml(
                            especialidad.descripcion ||
                            'Sin descripción registrada.'
                        );

                        const icono = escaparHtml(
                            resolverIcono(
                                especialidad.nombreIcono
                            )
                        );

                        html += `
                            <a href="/Especialidad/Detalles?id=${idEspecialidad}"
                               class="directory-entry">

                                <span class="directory-number">
                                    ${numero}
                                </span>

                                <span class="directory-icon">

                                    <i class="${icono}"></i>

                                </span>

                                <span class="directory-entry-body">

                                    <h3>
                                        ${nombre}
                                    </h3>

                                    <p>
                                        ${descripcion}
                                    </p>

                                </span>

                                <span class="directory-entry-link">

                                    Ver detalle

                                    <i class="
                                        bi
                                        bi-arrow-right
                                        directory-entry-arrow
                                    "></i>

                                </span>

                            </a>
                        `;
                    });

                    contenedor.html(html);
                },

                error: function (xhr) {

                    console.error(xhr.responseText);

                    Especialidad.mostrarError(
                        xhr.responseJSON?.mensaje ||
                        'No fue posible cargar las especialidades.'
                    );
                }
            });
        },

        mostrarCargando() {

            $('#contenedorEspecialidades').html(`
                <div class="text-center py-5">

                    <div class="spinner-border mb-3"
                         style="color:#20C4D8;"
                         role="status">

                        <span class="visually-hidden">
                            Cargando especialidades...
                        </span>

                    </div>

                    <p class="text-muted mb-0">
                        Cargando especialidades...
                    </p>

                </div>
            `);
        },

        mostrarSinResultados(mensaje) {

            $('#contenedorEspecialidades').html(`
                <div class="
                    alert
                    alert-info
                    border-0
                    rounded-4
                    shadow-sm
                    text-center
                    py-4
                ">

                    <i class="
                        bi
                        bi-hospital
                        fs-2
                        d-block
                        mb-2
                    "></i>

                    ${escaparHtml(mensaje)}

                </div>
            `);
        },

        mostrarError(mensaje) {

            const textoSeguro = escaparHtml(mensaje);

            $('#contenedorEspecialidades').html(`
                <div class="
                    alert
                    alert-danger
                    border-0
                    rounded-4
                    shadow-sm
                    text-center
                    py-4
                ">

                    <i class="
                        bi
                        bi-exclamation-circle
                        fs-2
                        d-block
                        mb-2
                    "></i>

                    ${textoSeguro}

                </div>
            `);

            if (typeof Swal !== 'undefined') {

                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: mensaje,
                    confirmButtonText: 'Aceptar'
                });
            }
        }
    };

    $(function () {
        Especialidad.init();
    });

})();
