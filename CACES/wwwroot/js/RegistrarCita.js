(() => {
    const RegistrarCita = {

        init() {
            this.cargarEspecialidades();
            this.registrarEventos();
        },

        cargarEspecialidades() {
            $.ajax({
                url: '/Cita/ObtenerEspecialidadesActivas',
                type: 'GET',
                success: function (res) {
                    let html = '<option value="">Seleccione una especialidad</option>';
                    (res.dato || res).forEach(e => {
                        html += `<option value="${e.id}">${e.nombre}</option>`;
                    });
                    $('#IdEspecialidad').html(html);
                    $('#IdMedico').html('<option value="">Seleccione una especialidad primero</option>');
                    $('#IdHorario').html('<option value="">Seleccione un médico primero</option>');
                },
                error: function () {
                    Swal.fire({ title: 'Error', text: 'No fue posible cargar las especialidades.', icon: 'error' });
                }
            });
        },

        cargarMedicos(idEspecialidad) {
            $('#IdMedico').html('<option value="">Cargando...</option>');
            $('#IdHorario').html('<option value="">Seleccione un médico primero</option>');

            if (!idEspecialidad) {
                $('#IdMedico').html('<option value="">Seleccione una especialidad primero</option>');
                return;
            }

            $.ajax({
                url: `/Cita/ObtenerMedicos?idEspecialidad=${idEspecialidad}`,
                type: 'GET',
                success: function (res) {
                    let html = '<option value="">Seleccione un médico</option>';
                    (res.dato || res).forEach(m => {
                        html += `<option value="${m.id}">${m.nombre}</option>`;
                    });
                    $('#IdMedico').html(html);
                },
                error: function () {
                    Swal.fire({ title: 'Error', text: 'No fue posible cargar los médicos.', icon: 'error' });
                }
            });
        },

        cargarHorarios(idMedico) {
            $('#IdHorario').html('<option value="">Cargando...</option>');

            if (!idMedico) {
                $('#IdHorario').html('<option value="">Seleccione un médico primero</option>');
                return;
            }

            $.ajax({
                url: `/Cita/ObtenerHorariosPorMedico?idMedico=${idMedico}`,
                type: 'GET',
                success: function (res) {
                    let html = '<option value="">Seleccione un horario</option>';
                    (res.dato || []).forEach(h => {
                        html += `<option value="${h.idHorario}" data-hora="${h.horaInicio}">${h.horarioTexto}</option>`;
                    });
                    $('#IdHorario').html(html);
                },
                error: function () {
                    Swal.fire({ title: 'Error', text: 'No fue posible cargar los horarios.', icon: 'error' });
                }
            });
        },

        registrarEventos() {
            $('#IdEspecialidad').on('change', function () {
                RegistrarCita.cargarMedicos($(this).val());
            });

            $('#IdMedico').on('change', function () {
                RegistrarCita.cargarHorarios($(this).val());
            });

            $('#formRegistrarCita').on('submit', function (e) {
                e.preventDefault();
                $.ajax({
                    url: '/Cita/RegistrarCita',
                    type: 'POST',
                    data: $(this).serialize(),
                    success: function (respuesta) {
                        if (respuesta.esCorrecto) {
                            Swal.fire({ title: 'Correcto', text: respuesta.mensaje, icon: 'success' })
                                .then(() => window.location.href = '/Cita/MisCitas');
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