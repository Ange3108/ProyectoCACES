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

            return `/${foto}`;
        }

        return `/img/${foto}`;
    }

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

    const Especialistas = {

        init() {
            this.cargarEspecialistas();
        },

        cargarEspecialistas() {

            const contenedor = $('#contenedorEspecialistas');

            if (!contenedor.length) {
                return;
            }

            this.mostrarCargando();

            $.ajax({

                url: '/Medico/ObtenerEspecialistas',

                type: 'GET',

                success: function (respuesta) {

                    if (!respuesta ||
                        respuesta.esCorrecto === false ||
                        !Array.isArray(respuesta.dato) ||
                        respuesta.dato.length === 0) {

                        Especialistas.mostrarSinResultados(
                            respuesta?.mensaje ||
                            'No hay especialistas disponibles actualmente.'
                        );

                        return;
                    }

                    let tarjetas = '';

                    respuesta.dato.forEach(medico => {

                        const nombre = [
                            medico.usuario?.nombres ?? '',
                            medico.usuario?.primerApellido ?? '',
                            medico.usuario?.segundoApellido ?? ''
                        ]
                            .join(' ')
                            .replace(/\s+/g, ' ')
                            .trim();

                        const nombreSeguro = escaparHtml(
                            nombre || 'Especialista'
                        );

                        const foto = resolverFoto(
                            medico.usuario?.foto
                        );

                        const especialidad = escaparHtml(
                            medico.nombreEspecialidad ||
                            'Especialista'
                        );

                        const experiencia = Number(
                            medico.experiencia ?? 0
                        );

                        const certificaciones = escaparHtml(
                            medico.certificaciones || ''
                        );

                        tarjetas += `
                            <div class="col-12 col-md-6 col-lg-4">

                                <div class="
                                    card
                                    h-100
                                    border-0
                                    shadow-sm
                                    rounded-4
                                    overflow-hidden
                                    tarjeta-especialista
                                ">

                                    <div class="text-center pt-4 px-4">

                                        <img src="${foto}"
                                             alt="Foto de ${nombreSeguro}"
                                             class="
                                                rounded-circle
                                                border
                                                shadow-sm
                                             "
                                             style="
                                                width:125px;
                                                height:125px;
                                                object-fit:cover;
                                                border-color:#D9EEF2 !important;
                                             "
                                             onerror="
                                                this.onerror=null;
                                                this.src='/img/default.jpg';
                                             " />

                                    </div>

                                    <div class="card-body text-center p-4">

                                        <h5 class="fw-bold text-dark mb-2">
                                            Dr(a). ${nombreSeguro}
                                        </h5>

                                        <span class="
                                            badge
                                            rounded-pill
                                            px-3
                                            py-2
                                            mb-3
                                        "
                                              style="
                                                background:#DDF7F8;
                                                color:#0B6F73;
                                              ">

                                            <i class="bi bi-hospital me-1"></i>
                                            ${especialidad}

                                        </span>

                                        <p class="text-secondary mb-2">

                                            <i class="bi bi-award me-1"></i>

                                            ${experiencia}
                                            año${experiencia !== 1 ? 's' : ''}
                                            de experiencia

                                        </p>

                                        ${certificaciones
                                ? `
                                                    <p class="
                                                        small
                                                        text-muted
                                                        mb-3
                                                        certificaciones-especialista
                                                    ">

                                                        <i class="
                                                            bi
                                                            bi-patch-check
                                                            me-1
                                                        "></i>

                                                        ${certificaciones}

                                                    </p>
                                                `
                                : `
                                                    <p class="
                                                        small
                                                        text-muted
                                                        mb-3
                                                    ">

                                                        <i class="
                                                            bi
                                                            bi-info-circle
                                                            me-1
                                                        "></i>

                                                        Sin certificaciones registradas

                                                    </p>
                                                `
                            }

                                        <span class="
                                            badge
                                            rounded-pill
                                            px-3
                                            py-2
                                        "
                                              style="
                                                background:#DCFCE7;
                                                color:#166534;
                                              ">

                                            <i class="
                                                bi
                                                bi-check-circle-fill
                                                me-1
                                            "></i>

                                            Disponible

                                        </span>

                                    </div>

                                </div>

                            </div>
                        `;
                    });

                    contenedor.html(`
                        <div class="row g-4">
                            ${tarjetas}
                        </div>
                    `);
                },

                error: function (xhr) {

                    console.error(xhr.responseText);

                    Especialistas.mostrarError(
                        xhr.responseJSON?.mensaje ||
                        'No fue posible cargar los especialistas. Intente nuevamente.'
                    );
                }
            });
        },

        mostrarCargando() {

            $('#contenedorEspecialistas').html(`
                <div class="text-center py-5">

                    <div class="
                        spinner-border
                        mb-3
                    "
                         style="color:#20C4D8;"
                         role="status">

                        <span class="visually-hidden">
                            Cargando especialistas...
                        </span>

                    </div>

                    <p class="text-muted mb-0">
                        Cargando especialistas...
                    </p>

                </div>
            `);
        },

        mostrarSinResultados(mensaje) {

            $('#contenedorEspecialistas').html(`
                <div class="
                    alert
                    alert-info
                    border-0
                    text-center
                    rounded-4
                    shadow-sm
                    py-4
                ">

                    <i class="
                        bi
                        bi-person-x
                        fs-2
                        d-block
                        mb-2
                    "></i>

                    ${escaparHtml(mensaje)}

                </div>
            `);
        },

        mostrarError(mensaje) {

            const texto = escaparHtml(mensaje);

            if (typeof Swal !== 'undefined') {

                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: mensaje,
                    confirmButtonText: 'Aceptar'
                });

                $('#contenedorEspecialistas').html(`
                    <div class="
                        alert
                        alert-danger
                        border-0
                        text-center
                        rounded-4
                        shadow-sm
                        py-4
                    ">

                        <i class="
                            bi
                            bi-exclamation-circle
                            fs-2
                            d-block
                            mb-2
                        "></i>

                        ${texto}

                    </div>
                `);

                return;
            }

            $('#contenedorEspecialistas').html(`
                <div class="
                    alert
                    alert-danger
                    border-0
                    text-center
                    rounded-4
                    shadow-sm
                    py-4
                ">

                    <i class="
                        bi
                        bi-exclamation-circle
                        fs-2
                        d-block
                        mb-2
                    "></i>

                    ${texto}

                </div>
            `);
        }
    };

    $(function () {
        Especialistas.init();
    });

})();