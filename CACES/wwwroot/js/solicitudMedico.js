(() => {

    const SolicitudMedico = {

        maximoArchivoBytes: 5 * 1024 * 1024,

        init() {

            if (!$('#formSolicitudMedico').length) {
                return;
            }

            this.cargarEspecialidades();
            this.registrarContadores();
            this.registrarValidacionArchivos();
            this.registrarEnvio();
        },

        // =====================================================
        // ESPECIALIDADES
        // =====================================================

        cargarEspecialidades() {

            const combo = $('#IdEspecialidad');

            combo
                .prop('disabled', true)
                .empty()
                .append(`
                    <option value="">
                        Cargando especialidades...
                    </option>
                `);

            fetch('/SolicitudMedico/ObtenerEspecialidades')
                .then(async response => {

                    const respuesta = await response.json();

                    if (!response.ok ||
                        !respuesta?.esCorrecto) {

                        throw new Error(
                            respuesta?.mensaje ??
                            'No fue posible cargar las especialidades.'
                        );
                    }

                    return Array.isArray(respuesta.dato)
                        ? respuesta.dato
                        : [];
                })
                .then(especialidades => {

                    combo.empty();

                    combo.append(`
                        <option value="">
                            Seleccione una especialidad
                        </option>
                    `);

                    especialidades.forEach(especialidad => {

                        const id =
                            especialidad.idEspecialidad;

                        const nombre =
                            especialidad.nombre;

                        if (!id || !nombre) {
                            return;
                        }

                        combo.append(`
                            <option value="${id}">
                                ${SolicitudMedico.escaparHtml(nombre)}
                            </option>
                        `);
                    });
                })
                .catch(error => {

                    console.error(error);

                    combo
                        .empty()
                        .append(`
                            <option value="">
                                No fue posible cargar las especialidades
                            </option>
                        `);

                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: error.message
                    });
                })
                .finally(() => {
                    combo.prop('disabled', false);
                });
        },

        // =====================================================
        // CONTADORES
        // =====================================================

        registrarContadores() {

            $('#Certificaciones').on('input', function () {

                $('#contadorCertificaciones')
                    .text($(this).val().length);
            });

            $('#Motivo').on('input', function () {

                $('#contadorMotivo')
                    .text($(this).val().length);
            });
        },

        // =====================================================
        // ARCHIVOS
        // =====================================================

        registrarValidacionArchivos() {

            $('#Curriculum').on('change', function () {

                const archivo = this.files?.[0];

                if (!archivo) {
                    return;
                }

                const extension =
                    SolicitudMedico.obtenerExtension(
                        archivo.name
                    );

                if (extension !== '.pdf') {

                    SolicitudMedico.limpiarArchivo(this);

                    Swal.fire({
                        icon: 'warning',
                        title: 'Currículum inválido',
                        text: 'El currículum debe estar en formato PDF.'
                    });

                    return;
                }

                if (archivo.size >
                    SolicitudMedico.maximoArchivoBytes) {

                    SolicitudMedico.limpiarArchivo(this);

                    Swal.fire({
                        icon: 'warning',
                        title: 'Archivo demasiado grande',
                        text: 'El currículum no puede superar los 5 MB.'
                    });
                }
            });

            $('#Foto').on('change', function () {

                const archivo = this.files?.[0];

                if (!archivo) {
                    return;
                }

                const extensionesPermitidas = [
                    '.jpg',
                    '.jpeg',
                    '.png',
                    '.webp'
                ];

                const extension =
                    SolicitudMedico.obtenerExtension(
                        archivo.name
                    );

                if (!extensionesPermitidas.includes(extension)) {

                    SolicitudMedico.limpiarArchivo(this);

                    Swal.fire({
                        icon: 'warning',
                        title: 'Fotografía inválida',
                        text: 'La fotografía debe ser JPG, PNG o WEBP.'
                    });

                    return;
                }

                if (archivo.size >
                    SolicitudMedico.maximoArchivoBytes) {

                    SolicitudMedico.limpiarArchivo(this);

                    Swal.fire({
                        icon: 'warning',
                        title: 'Archivo demasiado grande',
                        text: 'La fotografía no puede superar los 5 MB.'
                    });
                }
            });
        },

        // =====================================================
        // ENVÍO
        // =====================================================

        registrarEnvio() {

            $('#formSolicitudMedico').on(
                'submit',
                function (e) {

                    e.preventDefault();

                    const formulario = this;

                    if (!formulario.checkValidity()) {
                        formulario.reportValidity();
                        return;
                    }

                    const validacion =
                        SolicitudMedico.validarFormulario();

                    if (!validacion.esCorrecto) {

                        Swal.fire({
                            icon: 'warning',
                            title: 'Revise la información',
                            text: validacion.mensaje
                        });

                        return;
                    }

                    SolicitudMedico.enviarFormulario(
                        formulario
                    );
                }
            );
        },

        validarFormulario() {

            const curriculum =
                $('#Curriculum')[0]?.files?.[0];

            if (!curriculum) {
                return {
                    esCorrecto: false,
                    mensaje:
                        'Debe adjuntar el currículum en formato PDF.'
                };
            }

            const aniosExperiencia =
                Number($('#AniosExperiencia').val());

            if (Number.isNaN(aniosExperiencia) ||
                aniosExperiencia < 0 ||
                aniosExperiencia > 60) {

                return {
                    esCorrecto: false,
                    mensaje:
                        'Los años de experiencia deben estar entre 0 y 60.'
                };
            }

            const motivo =
                $('#Motivo').val()?.trim() ?? '';

            if (!motivo) {
                return {
                    esCorrecto: false,
                    mensaje:
                        'Debe indicar por qué desea formar parte de CACES.'
                };
            }

            if (motivo.length > 500) {
                return {
                    esCorrecto: false,
                    mensaje:
                        'El motivo no puede superar los 500 caracteres.'
                };
            }

            const certificaciones =
                $('#Certificaciones').val()?.trim() ?? '';

            if (certificaciones.length > 500) {
                return {
                    esCorrecto: false,
                    mensaje:
                        'Las certificaciones no pueden superar los 500 caracteres.'
                };
            }

            if (!$('#aceptaTerminos').is(':checked')) {
                return {
                    esCorrecto: false,
                    mensaje:
                        'Debe aceptar la autorización para revisar la información.'
                };
            }

            return {
                esCorrecto: true,
                mensaje: ''
            };
        },

        enviarFormulario(formulario) {

            const boton =
                $('#btnEnviarSolicitud');

            const formData =
                new FormData(formulario);

            boton
                .prop('disabled', true)
                .html(`
                    <span class="spinner-border
                                 spinner-border-sm
                                 me-2">
                    </span>
                    Enviando solicitud...
                `);

            fetch('/SolicitudMedico/RegistrarSolicitud', {

                method: 'POST',

                headers: {
                    'RequestVerificationToken':
                        $('input[name="__RequestVerificationToken"]')
                            .val()
                },

                body: formData
            })
                .then(async response => {

                    let respuesta;

                    try {
                        respuesta = await response.json();
                    }
                    catch {
                        throw new Error(
                            'El servidor devolvió una respuesta no válida.'
                        );
                    }

                    if (!response.ok ||
                        !respuesta?.esCorrecto) {

                        throw new Error(
                            respuesta?.mensaje ??
                            'No fue posible enviar la solicitud.'
                        );
                    }

                    return respuesta;
                })
                .then(respuesta => {

                    Swal.fire({
                        icon: 'success',
                        title: 'Solicitud enviada',
                        text:
                            respuesta.mensaje ??
                            'La solicitud fue enviada correctamente.',
                        confirmButtonText: 'Aceptar',
                        allowOutsideClick: false
                    })
                        .then(() => {

                            SolicitudMedico
                                .limpiarFormulario(formulario);
                        });
                })
                .catch(error => {

                    console.error(error);

                    Swal.fire({
                        icon: 'error',
                        title: 'No fue posible enviar la solicitud',
                        text: error.message
                    });
                })
                .finally(() => {

                    boton
                        .prop('disabled', false)
                        .html(`
                            <i class="bi bi-send-check me-2"></i>
                            Enviar solicitud
                        `);
                });
        },

        // =====================================================
        // UTILIDADES
        // =====================================================

        limpiarFormulario(formulario) {

            formulario.reset();

            $('#IdEspecialidad').val('');
            $('#contadorCertificaciones').text('0');
            $('#contadorMotivo').text('0');
        },

        limpiarArchivo(input) {

            input.value = '';
        },

        obtenerExtension(nombreArchivo) {

            const indice =
                nombreArchivo.lastIndexOf('.');

            return indice >= 0
                ? nombreArchivo
                    .substring(indice)
                    .toLowerCase()
                : '';
        },

        escaparHtml(valor) {

            return $('<div>')
                .text(valor ?? '')
                .html();
        }
    };

    $(function () {
        SolicitudMedico.init();
    });

})();