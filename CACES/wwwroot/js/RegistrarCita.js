(() => {

    const RegistrarCita = {

        init() {
            this.cargarMedicos();
            this.cargarEspecialidades();
            this.registrarEventos();
        },

        cargarMedicos() {
            $.ajax({
                url: '/Cita/ObtenerMedicos',
                type: 'GET',
                success: function (medicos) {
                    let html = '<option value="">Seleccione un médico</option>';
                    medicos.forEach(m => {
                        html += `<option value="${m.idMedico}">${m.nombre}</option>`;
                    });
                    $('#IdMedico').html(html);
                },
                error: function () {
                    Swal.fire({ title: 'Error', text: 'No fue posible cargar los médicos.', icon: 'error' });
                }
            });
        },

        cargarEspecialidades() {
            $.ajax({
                url: '/Cita/ObtenerEspecialidadesActivas',
                type: 'GET',
                success: function (especialidades) {
                    let html = '<option value="">Seleccione una especialidad</option>';
                    especialidades.forEach(e => {
                        html += `<option value="${e.idEspecialidad}">${e.nombre}</option>`;
                    });
                    $('#IdEspecialidad').html(html);
                },
                error: function () {
                    Swal.fire({ title: 'Error', text: 'No fue posible cargar las especialidades.', icon: 'error' });
                }
            });
        },

        registrarEventos() {
            $('#formRegistrarCita').on('submit', function (e) {
                e.preventDefault();

                $.ajax({
                    url: '/Cita/RegistrarCita',
                    type: 'POST',
                    data: $(this).serialize(),

                    success: function (respuesta) {
                        if (respuesta.esCorrecto) {
                            Swal.fire({
                                title: 'Correcto',
                                text: respuesta.mensaje,
                                icon: 'success'
                            }).then(() => {
                                window.location.href = '/Cita/MisCitas';
                            });
                        } else {
                            Swal.fire({ title: 'Error', text: respuesta.mensaje, icon: 'error' });
                        }
                    },

                    error: function () {
                        Swal.fire({ title: 'Error', text: 'Ocurrió un error al registrar la cita.', icon: 'error' });
                    }
                });
            });
        }
    };

    $(function () {
        RegistrarCita.init();
    });

})();