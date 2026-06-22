(() => {

    const RegistrarMedico = {

        init() {
            this.registrarEventos();
        },

        registrarEventos() {

            $('#formRegistrarMedico').on('submit', function (e) {
                e.preventDefault();

                // FormData en vez de serialize() porque el form tiene un archivo (foto)
                // serialize() ignora inputs type="file" — FormData los incluye
                const formData = new FormData(this);

                $.ajax({
                    url: $(this).attr('action'),
                    type: 'POST',
                    data: formData,
                    processData: false, // le dice a jQuery que NO convierta FormData a string
                    contentType: false, // le dice a jQuery que NO ponga Content-Type manual
                    // (el browser lo pone solo con el boundary correcto para multipart)

                    success: function (respuesta) {

                        if (respuesta.esCorrecto) {
                            Swal.fire({
                                title: '¡Médico registrado!',
                                text: respuesta.mensaje,
                                icon: 'success',
                                confirmButtonText: 'Ver listado'
                            }).then(() => {
                                window.location.href = '/Medico/Medicos';
                            });
                        } else {
                            Swal.fire({
                                title: 'Error',
                                text: respuesta.mensaje ?? 'No se pudo registrar el médico.',
                                icon: 'error'
                            });
                        }
                    },

                    error: function (xhr) {
                        // Si el servidor devolvió 400 con los errores de ModelState
                        const errores = xhr.responseJSON;
                        let mensaje = 'Ocurrió un error al registrar.';

                        if (errores?.mensaje) {
                            mensaje = errores.mensaje;
                        } else if (errores?.errors) {
                            // ModelState devuelve un objeto con los errores por campo
                            mensaje = Object.values(errores.errors)
                                .flat()
                                .join('\n');
                        }

                        Swal.fire({ title: 'Error de validación', text: mensaje, icon: 'error' });
                    }
                });
            });
        }
    };

    $(function () {
        RegistrarMedico.init();
    });

})();
